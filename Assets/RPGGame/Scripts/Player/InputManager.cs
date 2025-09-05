using UnityEngine;
using UnityEngine.InputSystem;

namespace RPGGame
{
    // 게임의 입력을 관리하는 입력 관리자 스크립트
    // DefaultExecutionOrder는 스크립트의 실행 순서를 지정할 때 사용한다.
    // 순서를 지정하지 않으면 유니티에서 자유롭게 그 순서를 지정하는데,
    // 이 스크립트는 다른 스크립트보다 빨리 처리돼야 하므로 0보다 작은 숫자를 지정해 순서를 정해준다.
    [DefaultExecutionOrder(-1)]
    public class InputManager : MonoBehaviour
    {
        // WASD/방향키 입력을 저장할 변수
        // static으로 지정을 하면 InputManager.Movement의 형태로 다른 스크립트에서 쉽게 접근이 가능하다.
        // 외부에서 접근은 가능하나, 이 변수에 값을 설정하는 것은 외부에서 불가능하도록 설정한다.
        public static Vector2 Movement { get; private set; } = Vector2.zero;

        // 이동 관련 입력 액션
        private InputAction moveAction;

        private void Awake()
        {
            // 이동 입력 액션 설정
            if (moveAction == null)
            {
                moveAction = InputSystem.actions.FindAction("Move");
            }
        }

        private void Update()
        {
            // WASD/방향키 입력 값 읽기
            Movement = moveAction.ReadValue<Vector2>();
        }
    }
}