using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPGGame
{
    // 몬스터의 스테이트를 제어하는 스크립트
    public class MonsterStateManager : MonoBehaviour
    {
        // 몬스터의 상태(스테이트)를 나타내는 열거형
        public enum State
        {
            None = -1,
            Idle,
            Patrol,
            Chase,
            Attack,
            Dead,
            Length
        }

        // 몬스터의 현재 스테이트 변수
        [SerializeField] private State state = State.None;

        // 몬스터의 각 스테이트에 실행될 스테이트 스크립트 배열
        [SerializeField] private MonsterStateBase[] states;

        // 스테이트가 변경될 때 발행되는 이벤트
        [SerializeField] private UnityEvent<State> OnStateChanged;

        // 몬스터 데이터
        [SerializeField] private MonsterData data;

        // 트랜스폼 컴포넌트 참조 변수
        private Transform refTransform;

        // 몬스터의 레벨
        [SerializeField] private int level = 1;

        // PlayerStateManager 참조 변수. 플레이어의 스테이트를 확인하는 데 사용
        private PlayerStateManager targetPlayerStateManager;

        // 몬스터의 레벨 데이터
        public MonsterData.LevelData CurrentLevelData { get; private set; }

        // 플레이어 트랜스폼 컴포넌트 참조 변수
        // 플레이어가 시야에 들어왔는지 확인할 때 주로 사용
        public Transform PlayerTransform { get; private set; }

        // 플레이어가 죽었는지를 알려주는 프로퍼티
        public bool IsPlayerDead { get { return targetPlayerStateManager.IsPlayerDead; } }

        // 몬스터 웨이브 시 몬스터 시야 밖에서도 플레이어를 쫓아가도록 설정하는 변수
        public bool IsForcedToChase { get; private set; }

        // 초기화
        private void Awake()
        {
            // 몬스터 데이터 로드
            data = DataManager.Instance.monsterData;

            // 몬스터 레벨 데이터 설정
            CurrentLevelData = data.levels[level - 1];

            // 몬스터 스테이트 배열 크기 초기화
            states = new MonsterStateBase[(int)State.Length];

            // GetComponent를 통해 각 스테이트 스크립트 설정
            for (int ix = 0; ix < states.Length; ++ix)
            {
                // 컴포넌트의 Type을 가져오기 위해 컴포넌트의 이름 설정
                // 컴포넌트의 이름을 사용해 Type을 구할 때는 Namespace도 같이 적용해야 한다.
                string componentName = $"RPGGame.Monster{(State)ix}State";

                // GetComponent로 컴포넌트를 가져온 뒤 states 배열에 저장
                states[ix] = GetComponent(Type.GetType(componentName)) as MonsterStateBase;

                // 각 스테이트는 시작할 때 비활성화
                states[ix].enabled = false;

                // 스테이트에 데이터 전달(의존성 주입)
                // data 변수는 동적으로 검색해서 설정하기 때문에 null 체크를 해주는 것이 좋다.
                if (data != null)
                {
                    states[ix].SetData(data);
                }
            }

            // 트랜스폼 컴포넌트 설정
            if (refTransform == null)
            {
                refTransform = transform;
            }

            // 플레이어 트랜스폼 컴포넌트 검색 후 설정
            if (PlayerTransform == null)
            {
                PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform.root;

                // PlayerStateManager 참조 변수 설정
                targetPlayerStateManager = PlayerTransform.GetComponent<PlayerStateManager>();
            }

            // HPController에 필요한 값 전달 및 이벤트 등록 처리
            HPController hpController = GetComponentInChildren<HPController>();
            if (hpController != null)
            {
                // 최대 체력 값 설정
                hpController.SetMaxHP(CurrentLevelData.maxHP);

                // 방어력 설정
                hpController.SetDefense(CurrentLevelData.defense);

                // 몬스터가 죽을 때 발행되는 이벤트에 함수 등록
                hpController.SubscribeOnDead(OnMonsterDead);
            }

            // MonsterAttackController 컴포넌트를 검색한 후, 공격력 설정
            MonsterAttackController attackController = GetComponentInChildren<MonsterAttackController>();
            if (attackController != null)
            {
                // 현재 레벨 데이터의 공격력 설정
                attackController.SetAttack(CurrentLevelData.attack);
            }
        }

        private void OnEnable()
        {
            // Idle 스테이트로 시작
            SetState(State.Idle);
        }

        private void Update()
        {
            // 현재 스테이트가 죽음(Dead)이라면 함수 종료
            // 플레이어가 죽음 스테이트일 때도 정찰을 하지 않도록 함수 종료
            if (state == State.Dead || IsPlayerDead)
            {
                return;
            }

            // 현재 스테이트가 Idle/Patrol이면, 플레이어가 시야에 들어왔는지 확인(정찰)
            if (state == State.Idle || state == State.Patrol)
            {
                // 시야에 들어왔는지 확인 후 시야에 들어왔으면 Chase 스테이트로 전환
                if (Util.IsInSight(refTransform, PlayerTransform, data.sightAngle, data.sightRange))
                {
                    // Chase 스테이트로 전환
                    SetState(State.Chase);
                    return;
                }
            }

            // 현재 스테이트가 Chase/Attack 스테이트라면, 플레이어가 시야에서 벗어났는지 확인
            if (state == State.Chase && !IsForcedToChase)
            {
                // 시야에서 벗어났는지 확인하고, 벗어났으면 Idle 스테이트로 전환
                if (!Util.IsInSight(refTransform, PlayerTransform, data.sightAngle, data.sightRange))
                {
                    SetState(State.Idle);
                    return;
                }
            }
        }

        // 스테이트를 전환할 때 사용하는 함수
        public void SetState(State newState)
        {
            // 전환하려는 새로운 스테이트가 현재 스테이트와 같으면 함수 종료
            // 현재 스테이트가 죽음일 때도 함수 종료
            if (state == newState || state == State.Dead)
            {
                return;
            }

            // 현재 스테이트가 None이 아니면 스테이트 비활성화
            if (state != State.None)
            {
                states[(int)state].enabled = false;
            }

            // 새로운 스테이트 컴포넌트 활성화
            states[(int)newState].enabled = true;

            // 현재 스테이트 업데이트
            state = newState;

            // 스테이트 변경 이벤트 발행
            OnStateChanged?.Invoke(state);
        }

        // 스테이트 변경 이벤트에 구독할 때 사용할 함수
        public void SubscribeOnStateChanged(UnityAction<State> listener)
        {
            OnStateChanged?.AddListener(listener);
        }

        // 몬스터의 레벨을 설정하는 함수
        // 레벨을 변경하고, 레벨에 해당하는 데이터를 설정함
        public void SetLevel(int level)
        {
            // 레벨 없데이트
            this.level = level;

            // 레벨 데이터 업데이트
            CurrentLevelData = data.levels[level - 1];
        }

        // 몬스터가 죽을 때 실행되는 함수
        public void OnMonsterDead()
        {
            // 테스트
            //Util.LogRed("몬스터 죽음.");

            // 죽음 스테이트로 전환
            SetState(State.Dead);

            // 플레이어 경험치 획득
            PlayerLevelController playerLevelController = FindFirstObjectByType<PlayerLevelController>();

            // 플레이어 레벨 관리자 검색 후 경험치 전달
            if (playerLevelController != null)
            {
                // 몬스터를 처치했을 때 얻는 경험치 양을 전달
                playerLevelController.GainExp(CurrentLevelData.gainExp);
            }
        }

        // 몬스터 웨이브 시 플레이어를 쫓아가도록 설정하는 함수
        public void SetForceToChase()
        {
            // 강제로 따라가도록 하는 옵션 설정
            IsForcedToChase = true;

            // 퀘스트 아이템의 타입을 웨이브로 변경
            // 퀘스트 아이템의 타입을 웨이브로 변경
            QuestItem questItem = GetComponentInChildren<QuestItem>();
            if (questItem != null)
            {
                questItem.SetType(QuestData.Type.Wave);
            }

            // 쫓아가기 스테이트로 전환
            SetState(State.Chase);
        }
    }
}