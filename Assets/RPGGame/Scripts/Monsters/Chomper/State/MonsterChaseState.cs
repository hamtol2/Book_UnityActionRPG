using UnityEngine;

namespace RPGGame
{
    // 몬스터의 추격 스테이트 스크립트
    public class MonsterChaseState : MonsterStateBase
    {
        protected override void Update()
        {
            base.Update();

            // 추격하다가 플레이어가 죽었으면 추격 중지
            if (manager.IsPlayerDead)
            {
                // 대기 스테이트로 전환
                manager.SetState(MonsterStateManager.State.Idle);
                return;
            }

            // 공격이 가능한 거리에 접근했으면 공격 스테이트로 전환
            if (Vector3.Distance(refTransform.position, manager.PlayerTransform.position) <= data.attackRange)
            {
                // 공격 스테이트로 전환
                manager.SetState(MonsterStateManager.State.Attack);
            }

            // 플레이어로 향하는 방향 벡터 구하기
            Vector3 direction = manager.PlayerTransform.position - refTransform.position;
            direction.y = 0f;

            // 플레이어로 향하는 방향이 0벡터가 아니면 회전 설정
            if (direction != Vector3.zero)
            {
                // 앞서 구한 방향 벡터를 사용해 쿼터니언 회전 구하기
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // 부드럽게 회전을 적용하기 위해 Quaternion.RotateTowards 함수 통해 회전 적용
                refTransform.rotation = Quaternion.RotateTowards(
                    refTransform.rotation,
                    targetRotation,
                    data.chaseRotateSpeed * Time.deltaTime
                );
            }
        }
    }
}