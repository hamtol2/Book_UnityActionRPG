using UnityEngine;

// 유니티의 이벤트를 사용하기 위해 추가
using UnityEngine.Events;

// 프로젝트에서 사용하는 네임 스페이스
namespace RPGGame
{
    // 플레이어의 스테이트를 제어하는 관리자 스크립트
    public class PlayerStateManager : MonoBehaviour
    {
        // 플레이어의 스테이트를 나타내는 열거형
        public enum State
        {
            None = -1,
            Idle,
            Move,
            Length
        }

        // 플레이어의 현재 상태(스테이트)를 나타내는 열거형 변수
        [SerializeField] private State state = State.None;

        // 플레이어의 각 스테이트 컴포넌트를 제어하기 위한 배열
        [SerializeField] private PlayerStateBase[] states;

        // 플레이어 스테이트가 변경될 때 발행될 이벤트
        [SerializeField] private UnityEvent<State> OnStateChanged;

        private void OnEnable()
        {
            // 게임이 시작되면 대기(Idle) 스테이트로 전환
            SetState(State.Idle);
        }

        private void Update()
        {
            // 입력 관리자의 Movement 입력이 없을 때는 대기(Idle) 스테이트로 전환
            if (InputManager.Movement == Vector2.zero)
            {
                SetState(State.Idle);
            }
            // 입력 관리자의 Movement 입력이 있을 때는 이동(Move) 스테이트로 전환
            else
            {
                SetState(State.Move);
            }
        }

        // 플레이어의 스테이트를 변경하는 함수
        public void SetState(State newState)
        {
            // 플레이어의 현재 스테이트와 변경하려는 스테이트가 같다면, 변경할 필요가 없으므로 함수 종료
            if (state == newState)
            {
                return;
            }

            // 현재 스테이트가 None이 아니라면 현재 스테이트를 비활성화
            if (state != State.None)
            {
                states[(int)state].enabled = false;
            }

            // 새로운 스테이트 활성화
            states[(int)newState].enabled = true;

            // 현재 스테이트를 나타내는 변수의 값을 새로운 스테이트로 설정
            state = newState;

            // 스테이트가 변경됐다는 이벤트를 발행
            OnStateChanged?.Invoke(state);
        }
    }
}