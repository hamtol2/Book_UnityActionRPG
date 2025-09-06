using UnityEngine;
using UnityEngine.Events;

namespace RPGGame
{
    // 몬스터의 공격 스테이트 스크립트
    public class MonsterAttackState : MonsterStateBase
    {
        // 공격 판정을 시작할 때 발행하는 이벤트(애니메이션 프레임에 연동돼 이벤트를 발행함)
        [SerializeField] private UnityEvent OnAttackBegin;

        // 공격 판정을 종료할 때 발행하는 이벤트(애니메이션 프레임에 연동돼 이벤트를 발행함)
        [SerializeField] private UnityEvent OnAttackEnd;

        protected override void Update()
        {
            base.Update();

            // 회전 처리
            // 플레이어로 향하는 방향 벡터 구하기
            Vector3 direction = manager.PlayerTransform.position - refTransform.position;
            direction.y = 0f;

            // 공격 스테이트에서는 플레이어를 바라보도록 회전을 한 번에 설정
            refTransform.rotation = Quaternion.LookRotation(direction);

            // 공격 가능 거리에서 벗어나면 다시 추격 스테이트로 전환
            if (Vector3.Distance(refTransform.position, manager.PlayerTransform.position) > data.attackRange)
            {
                // 추격 스테이트로 전환
                manager.SetState(MonsterStateManager.State.Chase);
            }
        }

        // Chomper_Attack 애니메이션에서 AttackBegin 이벤트가 발생할 때 실행되는 함수
        public void AttackBegin()
        {
            // 애니메이션에서 AttackBegin 이벤트가 발생되면, OnAttackBegin 이벤트를 발행
            OnAttackBegin?.Invoke();
        }

        // Chomper_Attack 애니메이션에서 AttackEnd 이벤트가 발생할 때 실행되는 함수
        public void AttackEnd()
        {
            // 애니메이션에서 AttackEnd 이벤트가 발생되면, OnAttackEnd 이벤트를 발행
            OnAttackEnd?.Invoke();
        }
    }
}