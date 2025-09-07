using UnityEngine;

namespace RPGGame
{
    // 보스 몬스터의 기본 스테이트 스크립트
    public class GrenadierIdleState : GrenadierStateBase
    {
        protected override void Update()
        {
            base.Update();

            // 플레이어와의 거리 계산
            // 계산량을 줄이기 위해 제곱근(루트)을 사용하지 않는 방식으로 계산
            float distanceToPlayer = (manager.PlayerTransform.position - refTransform.position).sqrMagnitude;

            // 플레이어가 시야 거리 안에 들어왔지만, 아직 공격 가능 거리는 아닐 때는 회전 스테이트로 전환
            if (distanceToPlayer <= manager.data.sightRange * manager.data.sightRange &&
                distanceToPlayer > manager.data.rangeAttackRange * manager.data.rangeAttackRange)
            {
                manager.SetState(GrenadierStateManager.State.Rotate);
                return;
            }

            // 공격 가능 거리에 들어왔으면 공격 스테이트로 전환
            if (Util.IsInSight(refTransform, manager.PlayerTransform, manager.data.sightAngle, manager.data.rangeAttackRange))
            {
                // 공격 스테이트로 전환
                manager.ChangeToAttack();
                return;
            }
        }
    }
}