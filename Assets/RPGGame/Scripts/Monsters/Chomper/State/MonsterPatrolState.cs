using UnityEngine;

namespace RPGGame
{
    // 몬스터의 정찰(Patrol) 스테이트 스크립트
    // 정찰 지점을 랜덤으로 선택한 뒤 선택한 위치로 이동
    public class MonsterPatrolState : MonsterStateBase
    {
        // 정찰할 때 이동 가능한 거리(단위: 미터)
        [SerializeField] private float patrolDistance = 6f;

        // 임의로 설정한 정찰 위치
        [SerializeField] private Vector3 patrolDestination;

        // 이 시간 이후까지 정찰 지점에 도달하지 못하면 다시 대기 상태로 전환
        // 정찰 지점을 잘못 선택하는 경우, 막힌 길 앞에서 우왕좌왕할 수 있기 때문에 사용
        [SerializeField] private float validPartolTime = 3f;

        // 정찰 지점을 시각적으로 표기하기 위해 사용(테스트 목적)
        [SerializeField] private Transform pointer;

        // 테스트 설정 옵션
        [SerializeField] private bool test = false;

        // 정찰 시간 계산을 위한 변수
        private float patrolStartTime = 0f;

        protected override void OnEnable()
        {
            base.OnEnable();

            // 테스트가 설정됐으면, 정찰 위치 표시를 위한 포인터 초기화
            if (test)
            {
                // 트랜스폼 부모 계층 해제
                pointer.SetParent(null);

                // 위치 초기화
                pointer.position = Vector3.zero;
            }

            // 랜덤으로 이동할 위치 선택이 성공했으면, 정찰 진행
            if (Util.RandomPoint(refTransform.position, patrolDistance, out patrolDestination))
            {
                // 정찰 시작 시간 저장
                patrolStartTime = Time.time;

                // 테스트: 목적지를 설정한 뒤 이동하려는 곳이 어디인지 표시
                if (test)
                {
                    // 이동할 목적지 설정
                    pointer.position = patrolDestination;

                    // 포인터 켜기
                    pointer.gameObject.SetActive(true);
                }
                else
                {
                    pointer.gameObject.SetActive(false);
                }
            }

            // 랜덤으로 정찰할 위치 선택에 실패했으면 다시 Idle 상태로 전환
            else
            {
                // 포인터 끄기
                ResetPointer();

                // 대기 스테이트로 전환
                manager.SetState(MonsterStateManager.State.Idle);
            }
        }

        protected override void Update()
        {
            base.Update();

            // 이동하는 데 걸리는 시간이 오래 걸리는 경우에는, 
            // 이동에 문제가 있다고 판단해 Patrol 스테이트 종료
            if (Time.time > patrolStartTime + validPartolTime)
            {
                manager.SetState(MonsterStateManager.State.Idle);
            }

            // 목적지에 도착했는지 확인 후 도착했으면 다시 Idle 스테이트로 전환
            if (Util.IsArrived(refTransform, patrolDestination, 0.5f))
            {
                // 포인터 끄기
                ResetPointer();

                // 대기(Idle) 스테이트로 전환
                manager.SetState(MonsterStateManager.State.Idle);
            }

            // 회전 처리

            // 회전 방향 계산
            Vector3 direction = patrolDestination - refTransform.position;
            direction.y = 0f;

            // 회전해야 하는 방향과 몬스터의 방향이 일치하지 않으면 회전 진행
            if (direction != Vector3.zero)
            {
                // 바라봐야 하는 방향을 쿼터니언으로 계산
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // 회전 속력을 적용해 부드럽게 회전 처리
                refTransform.rotation = Quaternion.RotateTowards(
                    refTransform.rotation,
                    targetRotation,
                    data.patrolRotateSpeed * Time.deltaTime
                );
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // 스테이트를 종료할 때는 정찰 목적 지점을 Vector3.zero(0, 0, 0)으로 초기화
            patrolDestination = Vector3.zero;

            // 테스트: 목적지 표시 끄기
            if (test)
            {
                ResetPointer();
            }
        }

        // 목적지 표시 기능을 끌 때 사용하는 함수
        private void ResetPointer()
        {
            // 몬스터 게임 오브젝트가 비활성화일 때는 함수 종료
            // 게임을 종료하거나, 몬스터가 죽을 때 몬스터 게임 오브젝트가 비활성화된다.
            if (!transform.root.gameObject.activeInHierarchy)
            {
                return;
            }

            // 테스트 옵션을 활성화했을 때만 처리
            if (test)
            {
                // 포인터 게임 오브젝트 비활성화
                pointer.gameObject.SetActive(false);

                // 포인터의 트랜스폼을 먹개비 몬스터로 설정
                pointer.SetParent(refTransform);

                // 포인터의 위치 초기화
                pointer.localPosition = Vector3.zero;
            }
        }
    }
}