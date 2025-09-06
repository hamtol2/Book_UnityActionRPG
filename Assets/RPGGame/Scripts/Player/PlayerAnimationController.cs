using UnityEngine;

namespace RPGGame
{
    // 플레이어의 애니메이션 처리를 담당하는 스크립트
    public class PlayerAnimationController : MonoBehaviour
    {
        // Animator 컴포넌트 참조 변수
        [SerializeField] private Animator refAnimator;

        private void OnEnable()
        {
            // Animator 컴포넌트 참조 변수 설정
            // 이 컴포넌트는 Player 게임 오브젝트 하위에 추가할 것이기 때문에 부모 계층까지 검색하는
            // GetComponentInParent 함수를 사용해 컴포넌트를 검색한다.
            if (refAnimator == null)
            {
                refAnimator = GetComponentInParent<Animator>();
            }
        }

        // 플레이어의 스테이트가 변경될 때 발생하는 이벤트에 등록하고, 이벤트가 발생할 때마다
        // 애니메이터 컨트롤러의 State 파라미터에 값을 설정하는 함수
        public void OnStateChanged(PlayerStateManager.State newState)
        {
            // Animator 컴포넌트가 설정되어 있지 않으면 함수 종료
            if (refAnimator == null)
            {
                return;
            }

            // State 파라미터에 값을 설정
            refAnimator.SetInteger("State", (int)newState);
        }

        // 플레이어가 점프했다가 착지할 때 설정하는 함수
        public void OnLanding()
        {
            // Landing 트리거 파라미터 설정
            refAnimator.SetTrigger("Landing");
        }

        // 공격 콤보 값을 변경할 때 사용하는 함수
        public void SetAttackComboState(int attackCombo)
        {
            // AttackCombo 정수 파라미터에 값 설정
            refAnimator.SetInteger("AttackCombo", attackCombo);
        }

        // 현재 재생 중인 애니메이션 스테이트를 반환하는 함수
        public AnimatorStateInfo GetCurrentStateInfo()
        {
            // 애니메이터에서 재생하는 스테이트 정보를 반환
            return refAnimator.GetCurrentAnimatorStateInfo(0);
        }
    }
}