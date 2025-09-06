using UnityEngine;

namespace RPGGame
{
    // 몬스터의 Idle 스테이트 스크립트
    // 정찰을 위해 이동하기 전까지 대기
    public class MonsterIdleState : MonsterStateBase
    {
        // 시간 계산을 위한 변수
        private float elapsedTime = 0f;

        // 스테이트 시작 함수
        protected override void OnEnable()
        {
            base.OnEnable();

            // 타이머 계산을 위한 변수 초기화
            elapsedTime = 0f;
        }

        // 스테이트 업데이트 함수
        protected override void Update()
        {
            base.Update();

            // 타이머 변수에 프레임 시간을 누적해서 얼만큼의 시간이 지났는 지를 계산
            elapsedTime += Time.deltaTime;

            // Idle 스테이트가 실행된 후 지난 시간이 waitTime에 정한 시간보다 더 지났으면
            // 타이머 계산 변수 초기화 후 스테이트 전환
            if (elapsedTime > data.patrolWaitTime)
            {
                elapsedTime = 0f;
                manager.SetState(MonsterStateManager.State.Patrol);
            }
        }

        // 애니메이션 이벤트 리스너 함수
        public void PlayStep()
        {

        }

        public void Grunt()
        {

        }
    }
}