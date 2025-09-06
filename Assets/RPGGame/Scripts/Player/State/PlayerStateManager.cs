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
            Jump,
            Attack,
            Length
        }

        // 공격 콤보를 나타내는 열거형
        public enum AttackCombo
        {
            None = -1,
            Combo1,
            Combo2,
            Combo3,
            Combo4,
            Length
        }

        // 플레이어의 현재 상태(스테이트)를 나타내는 열거형 변수
        [SerializeField] private State state = State.None;

        // 플레이어의 각 스테이트 컴포넌트를 제어하기 위한 배열
        [SerializeField] private PlayerStateBase[] states;

        // 플레이어 스테이트가 변경될 때 발행될 이벤트
        [SerializeField] private UnityEvent<State> OnStateChanged;

        // 캐릭터가 지면에 있는지 확인하기 위해 캐릭터 컨트롤러를 사용
        private CharacterController characterController;

        // 플레이어가 지면에 있는지 확인하는 변수
        public bool IsGrounded { get; private set; }

        // 플레이어 애니메이션 컨트롤러 참조 변수
        private PlayerAnimationController animationController;

        // 무기 컨트롤러 참조 변수.
        private WeaponController weaponController;

        // 다음에 이어질 공격 콤보 값을 보여주는 공개 프로퍼티
        public AttackCombo NextAttackCombo { get; private set; } = AttackCombo.None;

        private void Awake()
        {
            // 캐릭터 컨트롤러 설정
            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }

            // 플레이어 애니메이션 컨트롤러 설정
            if (animationController == null)
            {
                animationController = GetComponentInChildren<PlayerAnimationController>();
            }

            // 공격 스테이트가 종료될 때 발생하는 이벤트에 함수 등록
            PlayerAttackState attackState = GetComponent<PlayerAttackState>();
            if (attackState != null)
            {
                attackState.SubscribeOnAttackEnd(OnAttackEnd);
            }

            // 무기 컨트롤러 참조 변수 설정.
            if (weaponController == null)
            {
                weaponController = GetComponentInChildren<WeaponController>();
            }
        }

        private void OnEnable()
        {
            // 게임이 시작되면 대기(Idle) 스테이트로 전환
            SetState(State.Idle);
        }

        private void Update()
        {
            // 점프 스테이트일 때는 입력을 고려하지 않도록 함수 종료
            if (state == State.Jump)
            {
                return;
            }

            // 공격 입력 확인
            // 무기를 획득한 상태인지도 함께 확인
            if (InputManager.IsAttack && weaponController.IsWeaponAttached)
            {
                // 현재 스테이트가 공격이 아니라면, 공격 스테이트로 전환
                if (state != State.Attack)
                {
                    // 첫 번째 콤보 공격 설정
                    NextAttackCombo = AttackCombo.Combo1;

                    // 공격 스테이트로 전환
                    SetState(State.Attack);

                    // 애니메이터의 AtackCombo 값을 Combo1로 설정.
                    animationController.SetAttackComboState((int)NextAttackCombo);
                    return;
                }

                // 이미 공격 스테이트일 때는 다음에 이어질 콤보 설정

                // 현재 재생 중인 애니메이션의 스테이트 저장
                AnimatorStateInfo currentAnimationState = animationController.GetCurrentStateInfo();

                // 현재 재생 중인 공격 애니메이션의 스테이트 이름이 AttackCombo1이면 다음 콤보는 Combo2
                if (currentAnimationState.IsName("AttackCombo1"))
                {
                    NextAttackCombo = AttackCombo.Combo2;
                }

                // 현재 재생 중인 공격 애니메이션의 스테이트 이름이 AttackCombo2면 다음 콤보는 Combo3
                else if (currentAnimationState.IsName("AttackCombo2"))
                {
                    NextAttackCombo = AttackCombo.Combo3;
                }

                // 현재 재생 중인 공격 애니메이션의 스테이트 이름이 AttackCombo3이면 다음 콤보는 Combo4
                else if (currentAnimationState.IsName("AttackCombo3"))
                {
                    NextAttackCombo = AttackCombo.Combo4;
                }

                // 현재 재생 중인 공격 애니메이션의 스테이트 이름이 AttackComb4(또는 그 이외)면 다음 콤보는 없음(콤보 종료)
                else
                {
                    NextAttackCombo = AttackCombo.None;
                }

                return;
            }

            // 현재 스테이트가 공격일 때는 대기-이동-점프 전환을 처리하지 않고 함수 종료
            if (state == State.Attack)
            {
                return;
            }

            // 점프 입력 확인
            if (IsGrounded && state == State.Move && InputManager.IsJump)
            {
                // 플레이어가 공중에 있다고 설정
                IsGrounded = false;

                // 점프 스테이트로 전환
                SetState(State.Jump);
                return;
            }

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

        // 애니메이터를 통해 위치가 이동할 때 실행되는 함수
        private void OnAnimatorMove()
        {
            // 캐릭터 컨트롤러를 통해 플레이어가 지면에 있는지 확인 후 저장
            IsGrounded = characterController.isGrounded;
        }

        // 플레이어의 공격 스테이트가 종료될 때 호출되는 함수
        private void OnAttackEnd()
        {
            // 다음 콤보 값 초기화
            NextAttackCombo = AttackCombo.None;

            // 스테이트 관리자를 통해 플레이어의 스테이트를 대기(Idle)로 전환
            SetState(State.Idle);

            // 공격 스테이트가 종료되면, 다음 콤보를 나타내는 변수 값을 초깃값으로 설정
            animationController.SetAttackComboState((int)NextAttackCombo);
        }
    }
}