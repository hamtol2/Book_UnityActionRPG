using UnityEngine;

namespace RPGGame
{
    // 몬스터의 애니메이션을 관리하는 스크립트
    public class MonsterAnimationController : MonoBehaviour
    {
        // Aniamtor 컴포넌트 참조 변수
        private Animator refAnimator;

        private void Awake()
        {
            // Animator 컴포넌트 참조 변수 설정
            if (refAnimator == null)
            {
                // 자기 자신부터 부모 계층까지 포함해 검색
                refAnimator = GetComponentInParent<Animator>();
            }

            // 몬스터 스테이트 관리자를 찾은 후, 스테이트 변경 이벤트에 함수 등록
            MonsterStateManager manager = GetComponentInParent<MonsterStateManager>();
            if (manager != null)
            {
                manager.SubscribeOnStateChanged(OnStateChanged);
            }
        }

        // 몬스터의 스테이트가 변경될 때 실행되는 함수
        public void OnStateChanged(MonsterStateManager.State state)
        {
            // Animator 컴포넌트가 없으면 함수 종료
            if (refAnimator == null)
            {
                return;
            }

            // 함수의 인자로 전달된 열거형 값을 정수로 변환한 후, int 파라미터인 State에 설정
            refAnimator.SetInteger("State", (int)state);
        }
    }
}