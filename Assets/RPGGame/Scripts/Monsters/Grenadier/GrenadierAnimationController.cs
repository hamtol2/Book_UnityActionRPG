using UnityEngine;

namespace RPGGame
{
    // 보스 몬스터의 애니메이션 컨트롤러 스크립트
    public class GrenadierAnimationController : MonoBehaviour
    {
        // Animator 컴포넌트 참조 변수
        [SerializeField] private Animator refAnimator;

        private void Awake()
        {
            // Animator 컴포넌트 참조 변수 설정
            if (refAnimator == null)
            {
                refAnimator = transform.root.GetComponentInChildren<Animator>();
            }

            // 스테이트 변경 및 공격 타입 변경 이벤트에 함수 등록
            var stateManager = transform.root.GetComponentInChildren<GrenadierStateManager>();
            if (stateManager != null)
            {
                stateManager.SubscribeOnStateChanged(OnStateChanged);
                stateManager.SubscribeOnAttackTypeChanged(OnAttackTypeChanged);
            }
        }

        // 보스 몬스터의 회전 파라미터를 설정하는 함수
        public void SetAngle(float angle)
        {
            refAnimator.SetFloat("Angle", angle);
        }

        // 보스 몬스터가 맞았을 때 Hit 파라미터를 설정하는 함수
        public void Hit()
        {
            refAnimator.SetTrigger("Hit");
        }

        // Hit 트리거 파라미터 리셋 함수
        // 보스 몬스터가 공격 직전에 플레이어에게 맞았을 때 Hit 파라미터가 설정돼 있는데,
        // 이때 보스 몬스터가 공격을 하면 공격 애니메이션 재생 후에 Hit 애니메이션이 재생돼 이를 방지하기 위해 사용한다.
        public void ResetHit()
        {
            refAnimator.ResetTrigger("Hit");
        }

        // 보스 몬스터의 스테이트가 변경될 때 호출되는 함수
        // 스테이트 변경 이벤트에 등록해 사용
        private void OnStateChanged(GrenadierStateManager.State state)
        {
            refAnimator.SetInteger("State", (int)state);
        }

        // 보스 몬스터의 공격 타입이 변경될 때 호출되는 함수
        private void OnAttackTypeChanged(GrenadierStateManager.AttackType attackType)
        {
            refAnimator.SetInteger("AttackType", (int)attackType);
        }
    }
}