using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid3D : MonoBehaviour {
    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public LayerMask ascendableMask;
    public float nodeRadius;
    float nodeDiameter, correctionX, correctionY, correctionZ;

    public Vector3 gridWorldSize;
    int gridSizeX, gridSizeY, gridSizeZ;
    Node3D[,,] grid;

    public TerrainType[] walkableRegions;
    public LayerMask walkableMask;
    Dictionary<int, int> walkableRegionsDictionary = new();

    public int obstacleProximityPenalty = 10;

    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;

    void Awake() {
        nodeDiameter = nodeRadius * 2;

        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        //Debug.Log("X축 그리드의 개수 : " + gridSizeX);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        //Debug.Log("Y축 그리드의 개수 : " + gridSizeY);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
        //Debug.Log("Z축 그리드의 개수 : " + gridSizeZ);

        // 월드 좌표를 그리드 좌표로 변환할 때 중심값을 맞춰주기 위한 보정값
        // GridCenterPosition = transform.position + Correction
        // => Correction = GridCenterPosition - transform.position;
        correctionX = gridSizeX / 2 * nodeDiameter - transform.position.x;
        correctionY = gridSizeY / 2 * nodeDiameter - transform.position.y;
        correctionZ = gridSizeZ / 2 * nodeDiameter - transform.position.z;
        //Debug.Log("correctionX : " + correctionX);
        //Debug.Log("correctionY : " + correctionY);
        //Debug.Log("correctionZ : " + correctionZ);

        foreach (TerrainType region in walkableRegions) {
            walkableMask.value += region.terrainMask.value;
            //Debug.Log("레이어 인덱스 : " + region.terrainMask.value);
            walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }

        CreateGrid();
    }

    public int MaxSize {
        get { return gridSizeX * gridSizeY * gridSizeZ; }
    }

    Vector3 worldBottomLeft;
    int highestY = int.MinValue;
    void CreateGrid() {
        grid = new Node3D[gridSizeX, gridSizeY, gridSizeZ];
        worldBottomLeft = transform.position
            - Vector3.right * gridWorldSize.x / 2
            - Vector3.up * gridWorldSize.y / 2
            - Vector3.forward * gridWorldSize.z / 2;

        for (int y = 0; y < gridSizeY; y++) {
            for (int x = 0; x < gridSizeX; x++) {
                for (int z = 0; z < gridSizeZ; z++) {
                    Vector3 worldPoint = worldBottomLeft
                        + Vector3.right * (x * nodeDiameter + nodeRadius)
                        + Vector3.up * (y * nodeDiameter + nodeRadius)
                        + Vector3.forward * (z * nodeDiameter + nodeRadius);
                    bool walkable = false;
                    bool ascendable = false;
                    int penalty = 0;

                    if (Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask)) {
                        walkable = false;
                        ascendable = false;
                    } else {
                        if (Physics.CheckSphere(worldPoint, nodeRadius, walkableMask)) {
                            if (y > 0 && grid[x, y - 1, z].isWalkable) {
                                grid[x, y - 1, z].isWalkable = false;
                                grid[x, y - 1, z].isAscendable = false;
                                grid[x, y - 1, z].movementPenalty = obstacleProximityPenalty;
                            }
                            walkable = true;
                        }

                        if (Physics.CheckSphere(worldPoint, nodeRadius, ascendableMask)) {
                            ascendable = true;
                        }
                    }

                    // 이동 가능한 노드에 패널티 값 부여
                    Ray ray = new(worldPoint + Vector3.up * nodeRadius, Vector3.down);
                    if (Physics.Raycast(ray, out RaycastHit hit, nodeDiameter, walkableMask)) {
                        walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out penalty);
                    }

                    if (!walkable && !ascendable) {
                        penalty += obstacleProximityPenalty;
                        //Debug.Log("이동 불가 노드의 패널티 : " + penalty);
                    } else {
                        if (y > highestY) highestY = y;
                    }

                    grid[x, y, z] = new(ascendable, walkable, worldPoint, x, y, z, penalty);
                }
            }
        }
        SetAdditionalNodeOnSlope();
        //Debug.Log("이동 가능 노드의 수 : " + walkableCount);
        //BlurPenaltyMap(1);
    }

    void SetAdditionalNodeOnSlope() {
        for (int y = 0; y < highestY; y++) {
            for (int x = 0; x < gridSizeX; x++) {
                for (int z = 0; z < gridSizeZ; z++) {
                    if (grid[x, y, z].isAscendable) {
                        grid[x, y + 1, z].isWalkable = true;
                        //grid[x, y + 1, z].isAscendable = true;
                        grid[x, y + 1, z].movementPenalty = grid[x, y, z].movementPenalty;
                    }
                }
            }
        }
    }

    // Optimized Box Blur Algorithm
    void BlurPenaltyMap(int blurSize) {
        // 셀을 블러처리할때 계산이 수행되는 범위를 커널
        // 범위 안 중앙에 셀이 존재해야 하므로 커널의 크기는 항상 홀수
        int kernelSize = blurSize * 2 + 1;
        // 가장자리의 셀과 범위 중앙 셀 사이에 몇개의 셀이 있는지; 3x3 의 경우 한개의 셀이 존재
        // 셀을 블러처리할때 해당 크기만큼 양옆의 셀값을 더한다
        int kernelExtents = blurSize;

        //int y = 0;
        for (int y = 0; y < gridSizeY; y++) {
            int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeZ];
            int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeZ];
            // 영역의 각 행 방향 계산을 수행
            // 블러처리할 영역에 대해 셀하나씩 양옆 kernelExtents 개수 만큼의 셀을 더함
            // 만약 현재 블러처리하는 셀이 가장자리일 경우 셀이 존재하지 않는 방향에 대해 자신의 값을 활용하여 더함
            for (int z = 0; z < gridSizeZ; z++) {
                for (int x = -kernelExtents; x <= kernelExtents; x++) {
                    // x 값이 음수라면 x = 0인 셀의 값을 활용
                    int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                    penaltiesHorizontalPass[0, z] += grid[sampleX, y, z].movementPenalty;
                }

                // 현재 행의 남은 열들에 대해 계산 수행
                // 0 번째 열에대한 계산은 수행했으므로 1부터
                for (int x = 1; x < gridSizeX; x++) {
                    // 현재 회차의 커널 범위에서 제외된 셀의 인덱스
                    // 마찬가지로 인덱스값이 음수일 경우 인덱스 값이 0일때와 값이 같으므로 0으로 처리
                    int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                    // 현재 회차에서 새로 추가된 셀의 인덱스
                    int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);
                    penaltiesHorizontalPass[x, z] = penaltiesHorizontalPass[x - 1, z] - grid[removeIndex, y, z].movementPenalty + grid[addIndex, y, z].movementPenalty;
                }
            }

            // 행 방향 계산이 끝난후
            // 영역의 각 열 방향 계산을 수행
            for (int x = 0; x < gridSizeX; x++) {
                for (int z = -kernelExtents; z <= kernelExtents; z++) {
                    // z 값이 음수라면 z = 0인 셀의 값을 활용
                    int sampleZ = Mathf.Clamp(z, 0, kernelExtents);
                    penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleZ];
                }

                // z = 0 일때도 blurredPenalty 를 줘야하므로
                int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
                grid[x, y, 0].movementPenalty = blurredPenalty;

                for (int z = 1; z < gridSizeZ; z++) {
                    int removeIndex = Mathf.Clamp(z - kernelExtents - 1, 0, gridSizeZ - 1);
                    int addIndex = Mathf.Clamp(z + kernelExtents, 0, gridSizeZ - 1);
                    penaltiesVerticalPass[x, z] = penaltiesVerticalPass[x, z - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
                    blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, z] / (kernelSize * kernelSize));
                    grid[x, y, z].movementPenalty = blurredPenalty;

                    if (blurredPenalty > penaltyMax) penaltyMax = blurredPenalty;
                    if (blurredPenalty < penaltyMin) penaltyMin = blurredPenalty;
                }
            }
        }
        //Debug.Log("패널티 최대값 : " + penaltyMax);
        //Debug.Log("패널티 최소값 : " + penaltyMin);
    }

    public List<Node3D> GetNeighbours(Node3D node) {
        List<Node3D> neighbours = new();

        for (int y = -1; y <= 1; y++) {
            if (y != 0 && !node.isAscendable) continue;
            for (int x = -1; x <= 1; x++) {
                for (int z = -1; z <= 1; z++) {
                    if (x == 0 && y == 0 && z == 0) continue;
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    int checkZ = node.gridZ + z;

                    if (checkX >= 0 && checkX < gridSizeX
                        && checkY >= 0 && checkY < gridSizeY
                        && checkZ >= 0 && checkZ < gridSizeZ) {
                        neighbours.Add(grid[checkX, checkY, checkZ]);
                    }
                }
            }
        }
        return neighbours;
    }

    public Node3D GetNodeFromWorldPoint(Vector3 worldPosition) {
        //Debug.Log("월드 좌표 : " + worldPosition);
        float percentX = (worldPosition.x + correctionX) / gridWorldSize.x;
        //Debug.Log("PercentX : " + percentX);
        float percentY = (worldPosition.y + correctionY) / gridWorldSize.y;
        //Debug.Log("PercentY : " + percentY);
        float percentZ = (worldPosition.z + correctionZ) / gridWorldSize.z;
        //Debug.Log("PercentZ : " + percentZ);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        //Debug.Log("그리드 x좌표 : " + x);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        //Debug.Log("그리드 y좌표 : " + y);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
        //Debug.Log("그리드 z좌표 : " + z);
        return grid[x, y, z];
    }

    public List<Node3D> path;
    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, gridWorldSize.z));

        if (grid != null && displayGridGizmos) {
            foreach (Node3D n in grid) {
                Gizmos.color = Color.white;
                //Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));
                Gizmos.color = (n.isWalkable) ? Gizmos.color : Color.red;
                //if (path != null) {
                //    if (path.Contains(n))
                //        Gizmos.color = Color.blue;
                //}
                if (n.isWalkable)
                    Gizmos.DrawCube(n.worldPos, Vector3.one * nodeDiameter);
                else if (n.gridY == 0) Gizmos.DrawCube(n.worldPos, Vector3.one * nodeDiameter);
            }
        }
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(worldBottomLeft, nodeDiameter);
    }

    [System.Serializable]
    public class TerrainType {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}
