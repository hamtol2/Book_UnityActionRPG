using UnityEngine;
using UnityEngine.Events;

namespace RPGGame
{
    // 플레이어의 공격 스테이트를 담당하는 스크립트
    public class PlayerAttackState : PlayerStateBase
    {
        // 공격 판정을 시작할 때 발행할 이벤트
        [SerializeField] private UnityEvent OnAttackBegin;

        // 공격 판정을 종료할 때 발행할 이벤트
        [SerializeField] private UnityEvent OnAttackCheckEnd;

        // 공격을 종료할 때 발행할 이벤트
        [SerializeField] private UnityEvent OnAttackEnd;

        protected override void OnDisable()
        {
            base.OnDisable();

            // 공격 스테이트가 종료되면, 다음 콤보를 나타내는 변수를 값을 초기 값으로 설정
            animationController.SetAttackComboState((int)PlayerStateManager.AttackCombo.None);
        }

        // 애니메이션 이벤트 리스너 함수
        // 콤보1, 콤보2, 콤보3 공격 애니메이션에는 AttackStart/AttackEnd/ComboCheck 이벤트가 추가돼 있고, 콤보4는 ComboCheck 이벤트는 없다.
        private void AttackStart()
        {
            // 공격 판정 시작 이벤트 발행
            OnAttackBegin?.Invoke();
        }

        // 공격 시작 이벤트(AttackBegin)에 리스너 함수를 등록(구독)할 때 사용하는 함수
        public void SubscribeOnAttackBegin(UnityAction listener)
        {
            // OnAttackBegin 이벤트에 구독할 함수 추가
            OnAttackBegin?.AddListener(listener);
        }

        // 공격 애니메이션에서 AttackCheckEnd 이벤트가 발생할 때 실행될 함수
        private void AttackCheckEnd()
        {
            // 공격 판정 종료 이벤트 발행
            OnAttackCheckEnd?.Invoke();
        }

        // 공격 판정 종료 이벤트(OnAttackCheckEnd)에 리스너 함수를 등록(구독)할 때 사용하는 함수
        public void SubscribeOnAttackCheckEnd(UnityAction listener)
        {
            // OnAttackCheckEnd 이벤트에 구독할 함수 추가
            OnAttackCheckEnd?.AddListener(listener);
        }

        // 공격 애니메이션에서 ComboCheck 이벤트가 발생할 때 실행될 함수
        private void ComboCheck()
        {
            // 스테이트 관리자에 설정된 다음 콤보 단계 값을 읽어와 애니메이션에 전달
            // manager.NextAttackCombo 다음에 재생될 콤보 애니메이션을 결정
            animationController.SetAttackComboState((int)manager.NextAttackCombo);
        }

        // 공격 애니메이션에서 AttackEnd 이벤트가 발생할 때 실행될 함수
        private void AttackEnd()
        {
            // 공격 판정 종료 이벤트 발행
            OnAttackEnd?.Invoke();

            // 스테이트 관리자를 통해 플레이어의 스테이트를 대기(Idle)로 전환
            manager.SetState(PlayerStateManager.State.Idle);
        }

        // 공격 종료 이벤트(OnAttackEnd)에 리스너 함수를 등록(구독)할 때 사용하는 함수
        public void SubscribeOnAttackEnd(UnityAction listener)
        {
            // OnAttackEnd 이벤트에 구독할 함수 추가
            OnAttackEnd?.AddListener(listener);
        }
    }
}