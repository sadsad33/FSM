using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

// Skinned Mesh Renderer�� ���� �� �������� �ٸ� ������Ʈ�� �����ϱ� ����
// Root Bone�� ������ Skinned Mesh Renderer�� bones �� ������
public class SkinnedMeshRebinder : MonoBehaviour {
    [MenuItem("Tools/Rebind Skinned Mesh to Target")]
    static void RebindSkinnedMesh() {
        // Hierarchy ���� �� �������� ����
        GameObject selected = Selection.activeGameObject;
        if (selected == null) {
            Debug.LogError("���� SkinnedMeshRenderer�� ������ ������ �Ǵ� GameObject�� �����ϼ���.");
            return;
        }

        // ���õ� �� �������� SkinnedMeshRenderer
        SkinnedMeshRenderer smr = selected.GetComponent<SkinnedMeshRenderer>();
        if (smr == null) {
            Debug.LogError("SkinnedMeshRenderer�� ã�� �� �����ϴ�.");
            return;
        }

        // �����ϰ��� �ϴ� Ÿ�� ������Ʈ(ĳ����)
        GameObject targetCharacter = GameObject.Find("Player");
        if (targetCharacter == null) {
            Debug.LogError("TargetCharacter��� �̸��� GameObject�� Hierarchy���� ã�� �� �����ϴ�.");
            return;
        }

        // ĳ������ �ִϸ�����(���� ĳ������ Root Bone�� bones ������ �������� ����)
        Animator animator = targetCharacter.GetComponent<Animator>();
        if (animator == null) {
            Debug.LogError("TargetCharacter�� Animator�� �����ϴ�.");
            return;
        }

        // �� �̸�, Transform ����
        // Ÿ���� �ִϸ����ͷκ��� bone�� �̸��� key, bone�� Transform �� value�� ��ųʸ� ����
        Dictionary<string, Transform> boneMap = new();
        foreach (var bone in animator.GetComponentsInChildren<Transform>())
            boneMap[bone.name] = bone;

        // ������ ���� ���� �� �������� bone�� �̸���� ���Ӱ� �����Ϸ� �ϴ� Ÿ���� bone�� �̸����� �����ؾ� ��
        // ���� SkinnedMeshRenderer�� bone���� �� ��ŭ �迭�� ũ�⸦ ������
        // �� �迭�� Ÿ���� bone Transform �� ������ ����, ���� �� ������ Skinne Mesh Renderer�� ���ο� bones�� ��
        // ���� bone �̸��� Key�� Ÿ�ٿ��� bone�� Transform�� ã�� ������� newBones�� ����
        Transform[] newBones = new Transform[smr.bones.Length];
        for (int i = 0; i < smr.bones.Length; i++) {
            string boneName = smr.bones[i].name;
            if (!boneMap.TryGetValue(boneName, out newBones[i])) {
                Debug.LogError($"Ÿ�� ĳ���Ϳ��� �� '{boneName}'��(��) ã�� �� �����ϴ�.");
                return;
            }
        }

        // ���� �� �������� Skinned Mesh Renderer�� bones �� newBones �� �ٲ���
        smr.bones = newBones;

        // ���� �� �������� Skinned Mesh Renderer �� RootBone �� �̸��� Key�� Ÿ�ٿ��� Root Bone�� ã���� �ٲ���
        if (boneMap.TryGetValue(smr.rootBone.name, out var newRoot))
            smr.rootBone = newRoot;

        Debug.Log("SkinnedMeshRenderer ����ε� �Ϸ�!");

        // ������ ������ �����տ� �����Ϸ���, ����
#if UNITY_EDITOR
        EditorUtility.SetDirty(smr);
        PrefabUtility.RecordPrefabInstancePropertyModifications(smr);
#endif
    }
}