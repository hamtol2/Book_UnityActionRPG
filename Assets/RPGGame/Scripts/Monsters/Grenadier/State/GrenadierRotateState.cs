using UnityEngine;
using UnityEngine.Events;

namespace RPGGame
{
    // 보스 몬스터가 플레이어를 향해 회전하는 기능을 담당하는 스크립트
    public class GrenadierRotateState : GrenadierStateBase
    {
        // 애니메이션 컨트롤러 참조 변수
        [SerializeField] private GrenadierAnimationController animationController;

        // 회전 애니메이션이 재생되면서 보스 몬스터가 땅에 발을 디딜 때 발생하는 이벤트
        [SerializeField] private UnityEvent OnPlayStep;

        // Animator에 전달할 회전 값, 개발 중 디버깅이 가능하도록 변수로 추가
        [SerializeField] private float angle = 0f;

        protected override void OnEnable()
        {
            base.OnEnable();

            // 애니메이션 컨트롤러 참조 변수 설정
            if (animationController == null)
            {
                animationController = GetComponentInChildren<GrenadierAnimationController>();
            }
        }

        protected override void Update()
        {
            base.Update();

            // 플레이어를 향하도록 회전 적용
            RotateToPlayer();

            // 공격 가능 거리에 플레이어가 접근하면 공격 스테이트로 전환
            if (Util.IsInSight(refTransform, manager.PlayerTransform, manager.data.sightAngle, manager.data.rangeAttackRange))
            {
                // 공격으로 전환
                manager.ChangeToAttack();
            }

            // 시야에서 벗어났으면 Idle 스테이트로 전환
            if (Vector3.Distance(refTransform.position, manager.PlayerTransform.position) > manager.data.sightRange)
            {
                // 스테이트 전환
                manager.SetState(GrenadierStateManager.State.Idle);
            }
        }

        // 보스 몬스터의 회전 애니메이션에서 발행되는 애니메이션 이벤트
        private void PlayStep()
        {
            OnPlayStep?.Invoke();
        }

        // 플레이어를 향하도록 회전하는 함수
        private void RotateToPlayer()
        {
            // 각도 계산을 위해 플레이어를 향하는 벡터를 구한다. 
            Vector3 direction = manager.PlayerTransform.position - refTransform.position;
            direction.y = 0f;

            // 이미 플레이어를 향하도록 회전 설정이 완료됐으면 회전 적용을 안 한다.
            if (direction == Vector3.zero)
            {
                return;
            }

            // 플레이어를 향하는 벡터와 보스 몬스터의 시선 벡터와의 각도 계산
            // 왼쪽으로 회전해야 하는지 오른쪽으로 회전해야 하는지를 판단하기 위해 각도의 차이 및 부호(-/+)까지 계산해주는 SignedAngle 함수 활용
            angle = Vector3.SignedAngle(refTransform.forward, direction, Vector3.up);

            // 플레이어와의 각도 차이가 20도 미만일 때는 회전 애니메이션은 재생하지 않고, 플레이어를 바라보도록 회전 설정
            if (Mathf.Abs(angle) < 20f)
            {
                // 플레이어를 향하는 방향으로 회전 설정
                refTransform.rotation = Quaternion.LookRotation(direction);
                return;
            }

            // 애니메이션에 회전 값 설정
            // -180도와 180도 사이의 값을 -1과 1 사이의 값으로 변환해서 전달
            angle /= 180f;
            animationController.SetAngle(angle);
        }
    }
}