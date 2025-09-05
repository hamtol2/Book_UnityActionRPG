// 유니티 엔진이 제공하는 기본적인 기능을 사용하기 위해 추가
using UnityEngine;
// 유니티 엔진의 입력 시스템 관련 기능을 사용하기 위해 추가
using UnityEngine.InputSystem;

// RPGGame 네임 스페이스 추가
// 책에서 작성하는 모든 스크립트는 RPGGame 네임 스페이스를 추가해 작성할 예정
namespace RPGGame
{
    // 키보드의 WASD/방향키 입력을 받아서 플레이어를 제어하는 스크립트
    public class PlayerController : MonoBehaviour
    {
        // 트랜스폼 컴포넌트 참조 변수
        private Transform refTransform;

        // 이동 관련 입력 액션
        private InputAction moveAction;

        // 이동 빠르기
        [SerializeField] private float moveSpeed = 3f;

        // 회전 빠르기
        [SerializeField] private float rotationSpeed = 720f;

        // Animator 컴포넌트 참조 변수
        [SerializeField] private Animator refAnimator;

        // Awake 함수는 컴포넌트가 생성된 이후 첫 번째 Update 실행 전에 한 번만 실행됨
        void Awake()
        {
            // 트랜스폼 컴포넌트 참조 변수 설정
            if (refTransform == null)
            {
                // 유니티는 transform 이라는 프로퍼티를 통해 같은 게임 오브젝트에 있는
                // 트랜스폼 컴포넌트에 접근할 수 있는 기능을 제공한다.
                // 하지만 이렇게 변수를 추가한 뒤, 처음에 저장하고 재사용하는 것이 최적화 측면에서 더 좋다.
                refTransform = transform;
            }

            // 이동 입력 액션 설정
            if (moveAction == null)
            {
                moveAction = InputSystem.actions.FindAction("Move");
            }

            // Animator 컴포넌트 참조 변수 설정
            if (refAnimator == null)
            {
                refAnimator = GetComponentInChildren<Animator>();
            }
        }

        // Update 함수는 매 프레임 실행됨
        void Update()
        {
            // WASD/방향키 입력 값 읽기
            Vector2 moveValue = moveAction.ReadValue<Vector2>();

            // 입력 받은 방향 값을 사용해 이동할 방향 벡터 만들기
            Vector3 direction = new Vector3(moveValue.x, 0f, moveValue.y);
            direction.Normalize();

            // 게임 오브젝트의 위치 갱신하기
            // 새로운 위치 = 현재 위치 + 이동할 방향 벡터 x 프레임 시간
            refTransform.position = refTransform.position + direction * moveSpeed * Time.deltaTime;

            // 회전
            if (direction != Vector3.zero)
            {
                // 이동 방향을 바라보는 회전 값 생성
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // 현재 회전에서 목표 회전으로 부드럽게 회전 적용.
                refTransform.rotation = Quaternion.RotateTowards(refTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Animator 컴포넌트에 State 파라미터 설정
            // 입력 값이 x와 y축 모두 0인 경우에는 0(Idle)로 설정
            if (moveValue == Vector2.zero)
            {
                refAnimator.SetInteger("State", 0);
            }
            // x또는 y축 입력이 있는 경우에는 1(Move)로 설정
            else
            {
                refAnimator.SetInteger("State", 1);
            }
        }

        // EllenRunForward 애니메이션에 추가된 PlayStep 이벤트를 받는 함수
        private void PlayStep()
        {

        }
    }
}