using UnityEngine;

// 프로젝트에서 사용하는 네임 스페이스
namespace RPGGame
{
    // 플레이어가 사용하는 모든 스테이트(상태)의 기반 클래스
    public class PlayerStateBase : MonoBehaviour
    {
        // 트랜스폼 컴포넌트 참조 변수
        protected Transform refTransform;

        // 캐릭터 컨트롤러 컴포넌트 참조 변수
        protected CharacterController characterController;

        // Animator 컴포넌트 참조 변수
        protected Animator refAnimator;

        // PlayerStateManager 참조 변수
        protected PlayerStateManager manager;

        // PlayerAnimationController 참조 변수
        protected PlayerAnimationController animationController;

        // 플레이어가 사용할 데이터 변수.
        // 이 변수는 플레이어 스테이트 관리자가 전달해서 설정.
        protected PlayerData data;

        // 스테이트의 진입 함수
        // 스크립트가 비활성화 상태에서 다시 활성화되면 그 때마다 한 번씩 실행된다.
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

            // Animator 참조 변수 설정
            if (refAnimator == null)
            {
                refAnimator = GetComponent<Animator>();
            }

            // PlayerStateManager 참조 변수 설정
            if (manager == null)
            {
                manager = GetComponent<PlayerStateManager>();
            }

            // PlayerAnimationController 참조 변수 설정
            if (animationController == null)
            {
                animationController = GetComponentInChildren<PlayerAnimationController>();
            }
        }

        // 스테이트의 업데이트 함수
        // 스크립트가 활성화되면 프레임마다 반복 실행된다.
        protected virtual void Update()
        {
            // 캐릭터 컨트롤러는 충돌 처리는 하지만, 중력을 적용하지 않는다.
            // 중력이 필요한 경우에는 원하는 중력 가속도를 사용해 이동 기능에 포함시킨다.
            characterController.Move(Physics.gravity * Time.deltaTime);
        }

        // 스테이트의 종료 함수
        // 스크립트가 활성화 상태에서 다시 비활성화되면 그때마다 한 번씩 실행된다.
        protected virtual void OnDisable()
        {

        }

        // Root Motion을 사용해 Animator가 이동할 때 실행되는 함수
        // Animator의 Root Motion 옵션을 체크하면, 애니메이션 시스템이 트랜스폼을 직접 제어한다.
        // 하지만 애니메이션 시스템이 제어하지 않고, 이동한 거리 값을 전달 받아 직접 이동 기능을 구현하고자 할 때 이 함수를 활용한다.
        protected virtual void OnAnimatorMove()
        {
            // Root Motion을 통해 애니메이션이 이동한 거리는 Animator.deltaPosition으로 얻을 수 있다.
            // 이 deltaPosition 값을 characterController.Move 함수에 전달해 이동 기능을 구현.
            characterController.Move(refAnimator.deltaPosition);
        }

        // 플레이어 데이터를 설정할 때 사용할 함수.
        // 플레이어 스테이트 관리자가 각 스테이트를 초기화할 때 이 함수를 사용해서 플레이어 데이터를 전달해줌.
        public void SetData(PlayerData data)
        {
            this.data = data;
        }
    }
}