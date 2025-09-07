using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPGGame
{
    // 보스 몬스터인 Grenadier의 스테이트 관리자 스크립트
    [DefaultExecutionOrder(-1)]
    public class GrenadierStateManager : MonoBehaviour
    {
        // 스테이트 열거형
        public enum State
        {
            None = -1,
            Idle,
            Rotate,
            Attack,
            Dead,
            Length
        }

        // 공격 타입을 나타내는 열거형
        public enum AttackType
        {
            None = -1,
            Melee,
            Range,
            Length
        }

        // 현재 스테이트를 나타내는 변수
        [SerializeField] private State state = State.None;

        // 각 스테이트에 실행될 컴포넌트 배열
        [SerializeField] private GrenadierStateBase[] states;

        // 스테이트 변경 이벤트
        [SerializeField] private UnityEvent<State> OnStateChanged;

        // 공격 타입 변경 이벤트
        [SerializeField] private UnityEvent<AttackType> OnAttackTypeChanged;

        // 보스 몬스터의 레벨
        [SerializeField] private int level = 1;

        // PlayerStateManager 참조 변수, 플레이어의 스테이트를 확인하는 데 사용
        private PlayerStateManager TargetPlayerStateManager { get; set; }

        // 트랜스폼 참조 변수
        private Transform refTransform;

        // 공격과 공격 사이의 시간 간격(단위: 초)
        // 계속 공격이 가능하면 보스 몬스터가 너무 강력해서 공격한 뒤 다음 공격이 가능할 때까지 시간 간격을 둔다.
        [SerializeField] private float attackInterval = 3f;

        // 공격이 시작될 때 기록하는 시간 값
        // 공격과 공격 사이의 시간 간격을 두기 위해서 시간 기록을 한다.
        [SerializeField] private float attackTime = 0f;

        // 공격 스테이트로 전환이 가능한지를 나타내는 프로퍼티
        private bool CanAttack
        {
            get
            {
                // 현재 시간이 지난 공격 때 [기록한 시간 + 공격 간격]보다 큰지 확인
                // 공격이 가능한 시간까지 충분히 지나지 않았다면 공격 불가능
                return Time.time > attackTime + attackInterval;
            }
        }

        // 현재 공격 타입을 나타내는 변수
        public AttackType CurrentAttackType { get; private set; } = AttackType.None;

        // 플레이어가 죽었는지를 알려주는 프로퍼티
        public bool IsPlayerDead
        {
            get
            {
                return TargetPlayerStateManager.IsPlayerDead;
            }
        }

        // 보스 몬스터 데이터
        public MonsterData data;

        // 보스 몬스터의 레벨 데이터
        public MonsterData.LevelData CurrentLevelData { get; private set; }

        // 플레이어 트랜스폼 참조 변수
        public Transform PlayerTransform { get; private set; }

        private void Awake()
        {
            // 트랜스폼 참조 변수 설정
            if (refTransform == null)
            {
                refTransform = transform;
            }

            // 데이터 초기화
            data = DataManager.Instance.grenadierData;

            // 보스 몬스터의 레벨 데이터 초기화
            CurrentLevelData = data.levels[level - 1];

            // 스테이트 컴포넌트 배열 크기 초기화
            states = new GrenadierStateBase[(int)State.Length];

            // 스테이트 컴포넌트를 검색해 배열에 추가
            for (int ix = 0; ix < states.Length; ++ix)
            {
                // 컴포넌트의 Type을 가져오기 위해 컴포넌트의 이름 설정
                // 컴포넌트의 이름을 사용해 Type을 구할 때는 Namespace도 같이 적용해야 한다.
                string componentName = $"RPGGame.Grenadier{(State)ix}State";

                // GetComponent로 컴포넌트를 가져온 뒤 states 배열에 저장
                states[ix] = GetComponent(Type.GetType(componentName)) as GrenadierStateBase;

                // 각 스테이트는 시작할 때 비활성화
                states[ix].enabled = false;
            }

            // 플레이어 스테이트 관리자 및 트랜스폼 설정
            TargetPlayerStateManager = FindAnyObjectByType<PlayerStateManager>();
            PlayerTransform = TargetPlayerStateManager.transform;

            // HPController에 필요한 값 전달 및 이벤트 등록
            HPController hpController = GetComponentInChildren<HPController>();
            if (hpController != null)
            {
                hpController.SetMaxHP(CurrentLevelData.maxHP);
                hpController.SetDefense(CurrentLevelData.defense);

                // 몬스터가 죽었을 때 발행되는 이벤트에 등록
                hpController.SubscribeOnDead(OnDead);
            }

            // GrenadierMeleeAttack에 공격력 값 전달
            GrenadierMeleeAttack meleeAttack = GetComponentInChildren<GrenadierMeleeAttack>();
            if (meleeAttack != null)
            {
                meleeAttack.SetAttack(CurrentLevelData.attack);
            }

            // GrenadierRangeAttack에 공격력 및 공격 범위 값 전달
            GrenadierRangeAttack rangeAttack = GetComponentInChildren<GrenadierRangeAttack>();
            if (rangeAttack != null)
            {
                rangeAttack.SetAttack(CurrentLevelData.rangeAttack);
                rangeAttack.SetAttackRange(data.rangeAttackRange);
            }
        }

        private void OnEnable()
        {
            // 대기 스테이트로 시작
            SetState(State.Idle);
        }

        // 스테이트 변경 함수
        public void SetState(int state)
        {
            SetState((State)state);
        }

        // 스테이트 변경 함수
        public void SetState(State newState)
        {
            // 현재 스테이트와 변경할 스테이트가 같을 때는 처리하지 않고, 함수 종료
            // 현재 스테이트가 죽음인 경우에도 함수 종료
            if (state == State.Dead || state == newState)
            {
                return;
            }

            // 현재 스테이트가 None이 아닐 때는 현재 스테이트 컴포넌트 비활성화
            if (state != State.None)
            {
                states[(int)state].enabled = false;
            }

            // 변경할 스테이트 컴포넌트 활성화
            states[(int)newState].enabled = true;

            // 스테이트 값 업데이트
            state = newState;

            // 스테이트 변경 이벤트 발행
            OnStateChanged?.Invoke(state);
        }

        // 몬스터가 공격으로 전환할 때 사용하는 함수
        public void ChangeToAttack()
        {
            // 현재 스테이트가 공격이면, 함수 종료
            // 플레이어가 죽은 경우에도 함수 종료
            if (state == State.Attack || IsPlayerDead)
            {
                return;
            }

            // 공격이 가능하지 않은 경우, 함수 종료
            if (!CanAttack)
            {
                return;
            }

            // 현재 스테이트가 공격이 아닌 경우, 공격 스테이트 설정

            // 근접 또는 원거리 공격을 선택하기 위해 플레이어와의 거리 계산
            // 거리의 차이만 비교하면 되므로 제곱근(루트)를 사용하지 않는 방식으로 계산
            float distanceToPlayer = (PlayerTransform.position - refTransform.position).sqrMagnitude;

            // 플레이어와의 거리가 원거리 공격 가능 거리보다 클 때는 공격 스테이트로 전환하지 않고 종료
            if (distanceToPlayer > data.rangeAttackRange * data.attackRange)
            {
                CurrentAttackType = AttackType.None;
                return;
            }

            // 플레이어와의 거리가 근접 공격 거리보다는 크고, 원거리 공격 가능 거리 안에 있는 경우에는 원거리 공격 선택
            if ((distanceToPlayer > data.attackRange * data.attackRange) &&
                (distanceToPlayer <= data.rangeAttackRange * data.rangeAttackRange))
            {
                CurrentAttackType = AttackType.Range;
            }

            // 플레이어와의 거리가 근접 공격 거리 안에 있으면 원거리 공격 선택
            else if (distanceToPlayer <= data.attackRange * data.attackRange)
            {
                CurrentAttackType = AttackType.Melee;
            }

            // 공격으로 전환할 때 공격 시간 기록
            attackTime = Time.time;

            // 공격 타입 변경 이벤트 발행
            OnAttackTypeChanged?.Invoke(CurrentAttackType);

            // 스테이트 변경
            SetState(State.Attack);
        }

        // 몬스터가 죽을 때 실행되는 함수
        public void OnDead()
        {
            SetState(State.Dead);
        }

        // 플레이어를 향하도록 회전하는 함수
        public void RotateToPlayer()
        {
            // 각도 계산을 위해 플레이어를 향하는 벡터를 구한다.
            Vector3 direction = PlayerTransform.position - refTransform.position;
            direction.y = 0f;

            // 플레이어를 향하는 방향을 바라보도록 회전 설정
            refTransform.rotation = Quaternion.LookRotation(direction);
        }

        // 스테이트 변경 이벤트에 등록(구독)하는 함수
        public void SubscribeOnStateChanged(UnityAction<State> listener)
        {
            OnStateChanged?.AddListener(listener);
        }

        // 공격 타입 변경 이벤트에 등록(구독)하는 함수
        public void SubscribeOnAttackTypeChanged(UnityAction<AttackType> listener)
        {
            OnAttackTypeChanged?.AddListener(listener);
        }
    }
}