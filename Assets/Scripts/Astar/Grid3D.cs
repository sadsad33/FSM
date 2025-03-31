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
    LayerMask walkableMask;
    Dictionary<int, int> walkableRegionsDictionary = new();

    public int obstacleProximityPenalty = 10;

    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;

    void Awake() {
        nodeDiameter = nodeRadius * 2;

        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        //Debug.Log("X�� ũ�� : " + gridSizeX);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        //Debug.Log("Y�� ũ�� : " + gridSizeY);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
        //Debug.Log("Z�� ũ�� : " + gridSizeZ);

        // ���� ��ǥ�� �׸��� ��ǥ�� ��ȯ�� �� �߽ɰ��� �����ֱ� ���� ������
        // GridCenterPosition = transform.position + Correction
        // => Correction = GridCenterPosition - transform.position;
        correctionX = gridSizeX / 2 - transform.position.x;
        correctionY = gridSizeY / 2 - transform.position.y;
        correctionZ = gridSizeZ / 2 - transform.position.z;
        //Debug.Log("correctionX : " + correctionX);
        //Debug.Log("correctionY : " + correctionY);
        //Debug.Log("correctionZ : " + correctionZ);

        foreach (TerrainType region in walkableRegions) {
            walkableMask.value += region.terrainMask.value;
            //Debug.Log("���̾� �ε��� : " + region.terrainMask.value);
            walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }

        CreateGrid();
    }

    public int MaxSize {
        get { return gridSizeX * gridSizeY * gridSizeZ; }
    }

    Vector3 worldBottomLeft;
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
                            if (y > 0 && grid[x, y - 1, z].isWalkable) grid[x, y - 1, z].isWalkable = false;
                            walkable = true;
                        }

                        if (Physics.CheckSphere(worldPoint, nodeRadius, ascendableMask)) {
                            ascendable = true;
                        }
                    }

                    Ray ray = new(worldPoint + Vector3.up * 50, Vector3.down);
                    if (Physics.Raycast(ray, out RaycastHit hit, 100, walkableMask)) {
                        walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out penalty);
                    }

                    if (!walkable && !ascendable) {
                        penalty += obstacleProximityPenalty;
                    }

                    grid[x, y, z] = new(ascendable, walkable, worldPoint, x, y, z, penalty);
                }
            }
        }
        BlurPenaltyMap(1);
    }

    // Optimized Box Blur Algorithm
    void BlurPenaltyMap(int blurSize) {
        // ���� ��ó���Ҷ� ����� ����Ǵ� ������ Ŀ��
        // ���� �� �߾ӿ� ���� �����ؾ� �ϹǷ� �׻� Ȧ��
        int kernelSize = blurSize * 2 + 1;
        // �����ڸ��� ���� ���� �߾� �� ���̿� ��� ���� �ִ��� 3x3 �� ��� �Ѱ��� ���� ����
        // ���� ��ó���Ҷ� �ش� ũ�⸸ŭ �翷�� ������ ���Ѵ�
        int kernelExtents = (kernelSize - 1) / 2;

        int y = 0;
        //for (int y = 0; y < gridSizeY; y++) {
        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeZ];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeZ];
        // ������ �� �� ���� ����� ����
        // ��ó���� ������ ���� ���ϳ��� �翷 kernelExtents ���� ��ŭ�� ���� ����
        // ���� ���� ��ó���ϴ� ���� �����ڸ��� ��� ���� �������� �ʴ� ���⿡ ���� �ڽ��� ���� Ȱ���Ͽ� ����
        for (int z = 0; z < gridSizeZ; z++) {
            for (int x = -kernelExtents; x <= kernelExtents; x++) {
                // x ���� ������� x = 0�� ���� ���� Ȱ��
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                penaltiesHorizontalPass[0, z] += grid[sampleX, y, z].movementPenalty;
            }

            // ���� ���� ���� ���鿡 ���� ��� ����
            // 0 ��° �������� ����� ���������Ƿ� 1����
            for (int x = 1; x < gridSizeX; x++) {
                // ���� ȸ���� Ŀ�� �������� ���ܵ� ���� �ε���
                // ���������� �ε������� ������ ��� �ε��� ���� 0�϶��� ���� �����Ƿ� 0���� ó��
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                // ���� ȸ������ ���� �߰��� ���� �ε���
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);
                penaltiesHorizontalPass[x, z] = penaltiesHorizontalPass[x - 1, z] - grid[removeIndex, y, z].movementPenalty + grid[addIndex, y, z].movementPenalty;
            }
        }

        // �� ���� ����� ������
        // ������ �� �� ���� ����� ����
        for (int x = 0; x < gridSizeX; x++) {
            for (int z = -kernelExtents; z <= kernelExtents; z++) {
                // z ���� ������� z = 0�� ���� ���� Ȱ��
                int sampleZ = Mathf.Clamp(z, 0, kernelExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleZ];
            }

            // z = 0 �϶��� blurredPenalty �� ����ϹǷ�
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
        //}
    }

    public List<Node3D> GetNeighbours(Node3D node) {
        List<Node3D> neighbours = new();
        if (node.isAscendable) {
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
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
        } else {
            for (int x = -1; x <= 1; x++) {
                for (int z = -1; z <= 1; z++) {
                    if (x == 0 && z == 0) continue;
                    int checkX = node.gridX + x;
                    int checkZ = node.gridZ + z;

                    if (checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ) {
                        neighbours.Add(grid[checkX, node.gridY, checkZ]);
                    }
                }
            }
        }
        return neighbours;
    }

    public Node3D GetNodeFromWorldPoint(Vector3 worldPosition) {

        float percentX = (worldPosition.x + correctionX) / gridWorldSize.x;
        float percentY = (worldPosition.y + correctionY) / gridWorldSize.y;
        float percentZ = (worldPosition.z + correctionZ) / gridWorldSize.z;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
        return grid[x, y, z];
    }

    public List<Node3D> path;
    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, gridWorldSize.z));

        if (grid != null && displayGridGizmos) {
            foreach (Node3D n in grid) {
                //Gizmos.color = Color.white;
                Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));
                Gizmos.color = (n.isWalkable || n.isAscendable) ? Gizmos.color : Color.red;
                if (path != null) {
                    if (path.Contains(n))
                        Gizmos.color = Color.blue;
                }
                if (n.isWalkable || n.isAscendable)
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
