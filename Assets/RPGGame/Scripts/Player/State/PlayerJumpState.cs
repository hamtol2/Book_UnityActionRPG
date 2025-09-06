using UnityEngine;
using UnityEngine.Events;

namespace RPGGame
{
    // 플레이어의 점프 스테이트를 담당하는 스크립트
    public class PlayerJumpState : PlayerStateBase
    {
        // 점프를 할 때 위로 향하는 속도(미터/초)
        [SerializeField] private float jumpPower = 8f;

        // 점프 상태일 때 플레이어의 Y축 속력
        [SerializeField] private float verticalSpeed = 0f;

        // 점프할 때 적용할 중력
        [SerializeField] private float gravityInJump = 10f;

        // 플레이어가 공중에 떠 있을 때 앞으로 이동하는 속력
        [SerializeField] private float moveSpeed = 5f;

        // 플레이어가 점프를 했다가 지면에 발을 딛을 때 발생하는 이벤트
        [SerializeField] private UnityEvent OnJumpEnd;

        protected override void OnEnable()
        {
            base.OnEnable();

            // 점프가 시작되면 플레이어가 위로 올라갈 수 있도록 힘 적용.
            verticalSpeed = jumpPower;
        }

        protected override void Update()
        {
            // 이동 방향을 계산할 때 사용할 변수 선언.
            Vector3 movement = Vector3.zero;
            
            // 캐릭터 컨트롤러가 지면에 닿으면 기본 스테이트로 전환.
            if (verticalSpeed < 0f && manager.IsGrounded)
            {
                // 점프 종료 알림
                OnJumpEnd?.Invoke();

                // 기본 스테이트로 전환
                manager.SetState(PlayerStateManager.State.Idle);
            }
            else
            {
                // 위로 향하는 힘이 0보다 크면 위쪽으로 이동하고 있는 상태
                if (verticalSpeed > 0f)
                {
                    // 이때는 위로 적용되는 힘을 줄여야 함(그래야 다시 떨어질 수 있기 때문에)
                    verticalSpeed -= gravityInJump * Time.deltaTime;
                }

                // 점프 높이의 정점에 도달했는지 확인
                // (verticalSpeed 값이 0 값과 매우 비슷한지 확인)
                if (Mathf.Approximately(verticalSpeed, 0f))
                {
                    verticalSpeed = 0f;
                }

                // 떠 있는 동안에는 중력 적용
                verticalSpeed += Physics.gravity.y * Time.deltaTime;

                // 플레이어가 공중에 떠 있을 때는 앞방향으로 이동할 수 있도록 속력 설정
                movement = moveSpeed * refTransform.forward * Time.deltaTime;

                // 계산된 위쪽 방향으로의 힘을 이동 방향에 더해주기.
                movement += Vector3.up * verticalSpeed * Time.deltaTime;
            }

            // 캐릭터 컨트롤러를 통해 이동 적용.
            characterController.Move(movement);
        }
    }
}