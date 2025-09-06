using UnityEngine;

namespace RPGGame
{
    // 몬스터 스테이트의 기본(부모) 스크립트
    public class MonsterStateBase : MonoBehaviour
    {
        // 트랜스폼 참조 변수
        protected Transform refTransform;

        // Animator 컴포넌트 참조 변수
        // OnAnimatorMove 함수를 사용하기 위함
        protected Animator refAnimator;

        // 캐릭터 컨트롤러 참조 변수
        // 몬스터의 이동 처리를 할 때 캐릭터 컨트롤러를 활용
        protected CharacterController characterController;

        // 몬스터가 사용할 데이터 변수
        // 이 변수는 몬스터 스테이트 관리자가 전달해서 설정
        protected MonsterData data;

        // 몬스터 스테이트 관리자 참조 변수
        protected MonsterStateManager manager;

        protected virtual void OnEnable()
        {
            // 트랜스폼 참조 변수 설정
            if (refTransform == null)
            {
                refTransform = transform;
            }

            // 애니메이션 컴포넌트 참조 변수 설정
            if (refAnimator == null)
            {
                refAnimator = GetComponent<Animator>();
            }

            // 캐릭터 컨트롤러 참조 변수 설정
            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }
            
            // 스테이트 관리자 설정
            if (manager == null)
            {
                manager = GetComponent<MonsterStateManager>();
            }
        }

        protected virtual void Update()
        {
            // 캐릭터 컨트롤러는 중력 처리를 해주지 않기 때문에 중력을 직접 적용
            if (characterController.enabled)
            {
                // 캐릭터 컨트롤러의 이동 함수를 사용해 중력 적용
                characterController.Move(Physics.gravity * Time.deltaTime);
            }
        }

        protected virtual void OnDisable()
        {

        }

        // 몬스터 데이터를 설정할 때 사용할 함수
        // 몬스터 스테이트 관리자가 각 스테이트를 초기화할 때 이 함수를 통해 데이터를 전달
        public void SetData(MonsterData data)
        {
            this.data = data;
        }

        // Animator가 루트 모션을 통해 이동할 때 실행되는 함수
        protected virtual void OnAnimatorMove()
        {
            // 캐릭터 컨트롤러가 동작하면
            if (characterController.enabled)
            {
                // Animator 컴포넌트로부터 이동한 거리 값을 가져와 이동 처리
                characterController.Move(refAnimator.deltaPosition);
            }
        }
    }
}