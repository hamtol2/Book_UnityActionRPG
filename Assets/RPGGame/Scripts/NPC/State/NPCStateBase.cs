using UnityEngine;

namespace RPGGame
{
    // NPC 스테이트 기반 스크립트
    public class NPCStateBase : MonoBehaviour
    {
        // 트랜스폼 컴포넌트 참조 변수
        protected Transform refTransform;

        // 캐릭터 컨트롤러 참조 변수
        protected CharacterController characterController;

        // NPC 스테이트 관리자 참조 변수
        protected NPCStateManager manager;

        protected virtual void OnEnable()
        {
            // 트랜스폼 참조 변수 설정
            if (refTransform == null)
            {
                refTransform = transform;
            }

            // 캐릭터 컨트롤러 참조 변수 설정
            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }

            // NPC 스테이트 관리자 참조 변수 설정
            if (manager == null)
            {
                manager = GetComponent<NPCStateManager>();
            }
        }

        protected virtual void Update()
        {
            // 중력 적용
            characterController.Move(Physics.gravity * Time.deltaTime);
        }

        // 플레이어가 NPC와 대화가 가능한 거리에 있는지 확인하는 함수
        protected bool CanTalk()
        {
            return Vector3.Distance(manager.PlayerTransform.position, refTransform.position) <= manager.data.interactionSight;
        }
    }
}