using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

// Skinned Mesh Renderer를 가진 모델 프리팹을 다른 오브젝트에 부착하기 위해
// Root Bone을 포함한 Skinned Mesh Renderer의 bones 를 수정함
public class SkinnedMeshRebinder : MonoBehaviour {
    [MenuItem("Tools/Rebind Skinned Mesh to Target")]
    static void RebindSkinnedMesh() {
        // Hierarchy 에서 모델 프리팹을 선택
        GameObject selected = Selection.activeGameObject;
        if (selected == null) {
            Debug.LogError("먼저 SkinnedMeshRenderer를 포함한 프리팹 또는 GameObject를 선택하세요.");
            return;
        }

        // 선택된 모델 프리팹의 SkinnedMeshRenderer
        SkinnedMeshRenderer smr = selected.GetComponent<SkinnedMeshRenderer>();
        if (smr == null) {
            Debug.LogError("SkinnedMeshRenderer를 찾을 수 없습니다.");
            return;
        }

        // 부착하고자 하는 타겟 오브젝트(캐릭터)
        GameObject targetCharacter = GameObject.Find("Player");
        if (targetCharacter == null) {
            Debug.LogError("TargetCharacter라는 이름의 GameObject를 Hierarchy에서 찾을 수 없습니다.");
            return;
        }

        // 캐릭터의 애니메이터(현재 캐릭터의 Root Bone과 bones 정보를 가져오기 위함)
        Animator animator = targetCharacter.GetComponent<Animator>();
        if (animator == null) {
            Debug.LogError("TargetCharacter에 Animator가 없습니다.");
            return;
        }

        // 본 이름, Transform 매핑
        // 타겟의 애니메이터로부터 bone의 이름을 key, bone의 Transform 을 value로 딕셔너리 생성
        Dictionary<string, Transform> boneMap = new();
        foreach (var bone in animator.GetComponentsInChildren<Transform>())
            boneMap[bone.name] = bone;

        // 주의할 점은 기존 모델 프리팹의 bone의 이름들과 새롭게 부착하려 하는 타겟의 bone의 이름들이 동일해야 함
        // 기존 SkinnedMeshRenderer의 bone들의 수 만큼 배열의 크기를 설정함
        // 이 배열은 타겟의 bone Transform 의 정보를 갖고, 기존 모델 프리팹 Skinne Mesh Renderer의 새로운 bones이 됨
        // 기존 bone 이름을 Key로 타겟에서 bone의 Transform을 찾고 순서대로 newBones에 저장
        Transform[] newBones = new Transform[smr.bones.Length];
        for (int i = 0; i < smr.bones.Length; i++) {
            string boneName = smr.bones[i].name;
            if (!boneMap.TryGetValue(boneName, out newBones[i])) {
                Debug.LogError($"타겟 캐릭터에서 본 '{boneName}'을(를) 찾을 수 없습니다.");
                return;
            }
        }

        // 기존 모델 프리팹의 Skinned Mesh Renderer의 bones 을 newBones 로 바꿔줌
        smr.bones = newBones;

        // 기존 모델 프리팹의 Skinned Mesh Renderer 의 RootBone 의 이름을 Key로 타겟에서 Root Bone을 찾은후 바꿔줌
        if (boneMap.TryGetValue(smr.rootBone.name, out var newRoot))
            smr.rootBone = newRoot;

        Debug.Log("SkinnedMeshRenderer 재바인딩 완료!");

        // 수정된 내용을 프리팹에 저장하려면, 적용
#if UNITY_EDITOR
        EditorUtility.SetDirty(smr);
        PrefabUtility.RecordPrefabInstancePropertyModifications(smr);
#endif
    }
}