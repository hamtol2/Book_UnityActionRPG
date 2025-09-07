using UnityEngine;
using UnityEngine.Events;

namespace RPGGame
{
    // 보스 몬스터의 근접 공격 스테이트 스크립트
    public class GrenadierAttackState : GrenadierStateBase
    {
        // 근접 공격을 시작할 때 발생하는 이벤트
        [SerializeField] private UnityEvent OnMeleeAttackStart;

        // 근접 공격 충돌 확인을 시작할 때 발생하는 이벤트
        [SerializeField] private UnityEvent OnMeleeAttackCheckStart;

        // 범위 공격을 시작할 때 발생하는 이벤트
        [SerializeField] private UnityEvent OnRangeAttackStart;

        // 공격을 종료할 때 발생하는 이벤트
        [SerializeField] private UnityEvent OnAttackEnd;

        // 애니메이션 컨트롤러 참조 변수
        [SerializeField] private GrenadierAnimationController animationController;

        protected override void OnEnable()
        {
            base.OnEnable();

            // 플레이어가 죽었을 때는 더 이상 공격을 하지 않도록 처리
            if (manager.IsPlayerDead)
            {
                manager.SetState(GrenadierStateManager.State.Idle);
            }

            // 애니메이션 컨트롤러 참조 변수 설정
            if (animationController == null)
            {
                animationController = GetComponentInChildren<GrenadierAnimationController>();
            }

            // 공격 스테이트를 시작할 때 공격 타입을 확인한 후, 공격 타입이 근접 공격인 경우 이벤트 발행
            if (manager.CurrentAttackType == GrenadierStateManager.AttackType.Melee)
            {
                // 근접 공격 시작 이벤트 발행
                OnMeleeAttackStart?.Invoke();
            }
        }

        protected override void Update()
        {
            base.Update();

            // 공격 스테이트일 때는 플레이어를 향하도록 회전(한 번에 회전 적용)
            manager.RotateToPlayer();

            // 공격 가능 거리에서 벗어났으면 Idle 스테이트로 전환
            if (!Util.IsInSight(refTransform, manager.PlayerTransform, manager.data.sightAngle, manager.data.rangeAttackRange))
            {
                // Idle 스테이트로 전환
                manager.SetState(GrenadierStateManager.State.Idle);
                return;
            }
        }

        // 보스 몬스터의 공격 애니메이션에서 발생하는 이벤트
        // 공격 판정을 시작할 때 발행된다.
        public void StartAttack()
        {
            // 현재 공격 타입이 근접 공격이면 근접 공격 시작 이벤트 발행
            if (manager.CurrentAttackType == GrenadierStateManager.AttackType.Melee)
            {
                OnMeleeAttackCheckStart?.Invoke();
            }
        }

        // 보스 몬스터의 원거리 공격 애니메이션에서 발행하는 이벤트
        private void ActivateShield()
        {
            OnRangeAttackStart?.Invoke();
        }

        // 보스 몬스터의 공격 애니메이션에서 발생하는 이벤트
        // 공격 판정을 종료할 때 발행
        private void EndAttack()
        {
            // 몬스터 스테이트를 기본으로 전환
            manager.SetState(GrenadierStateManager.State.Idle);

            // 공격 종료 이벤트 발행
            OnAttackEnd?.Invoke();

            // Hit 트리거 파라미터 리셋
            // 공격 애니메이션이 재생되기 이전에 Hit 파라미터가 설정돼 있는 경우
            // 공격 애니메이션이 종료된 후 플레이어에게 맞지 않았는데 Hit 애니메이션이 재생될 수 있으므로 기존 Hit 파라미터는 리셋
            animationController.ResetHit();
        }

        // OnMeleeAttackStart 이벤트에 함수를 등록할 때 사용
        public void SubscribeOnMeleeAttackStart(UnityAction listener)
        {
            OnMeleeAttackStart?.AddListener(listener);
        }

        // OnMeleeAttackCheckStart 이벤트에 함수를 등록할 때 사용
        public void SubscribeOnMeleeAttackCheckStart(UnityAction listener)
        {
            OnMeleeAttackCheckStart?.AddListener(listener);
        }

        // OnRangeAttackStart 이벤트에 함수를 등록할 때 사용
        public void SubscribeOnRangeAttackStart(UnityAction listener)
        {
            OnRangeAttackStart?.AddListener(listener);
        }

        // OnAttackEnd 이벤트에 함수를 등록할 때 사용
        public void SubscribeOnAttackEnd(UnityAction listener)
        {
            OnAttackEnd?.AddListener(listener);
        }
    }
}