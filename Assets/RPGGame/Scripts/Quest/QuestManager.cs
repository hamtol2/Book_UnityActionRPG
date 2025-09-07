using UnityEngine;
using UnityEngine.Events;

namespace RPGGame
{
    // 퀘스트 관리를 담당하는 스크립트
    [DefaultExecutionOrder(-50)]
    public class QuestManager : MonoBehaviour
    {
        // 퀘스트 상태
        public enum QuestState
        {
            None = -1,
            Start,              // 시작
            Processing,         // 진행 중
            Complete,           // 완료
            Length
        }

        // 싱글턴을 제작하기 위한 인스턴스 프로퍼티
        public static QuestManager Instance { get; private set; } = null;

        // 현재 진행 중인 퀘스트 아이디
        [SerializeField] private int currentQuestID = 0;

        // 퀘스트 진행 상태
        [SerializeField] private QuestState state = QuestState.None;

        // 현재 진행 중인 퀘스트를 달성한 횟수
        [SerializeField] private int completeCount = 0;

        // 퀘스트 달성 여부를 나타내는 변수
        [SerializeField] private bool isQuestCompleted = false;

        // 퀘스트를 시작했을 때 발행되는 이벤트(int 값으로 퀘스트 ID 전달)
        [SerializeField] private UnityEvent<int> OnQuestStarted;

        // 퀘스트를 달성했을 때 발행되는 이벤트(int 값으로 퀘스트 ID 전달)
        [SerializeField] private UnityEvent<int> OnQuestCompleted;

        // 모든 퀘스트를 완료했을 때 발행되는 이벤트
        [SerializeField] private UnityEvent OnAllQuestsCompleted;

        // 퀘스트 완료 횟수가 변경될 때 발행되는 이벤트(int 값으로 현재 퀘스트 ID와 현재까지 완료한 횟수를 전달)
        [SerializeField] private UnityEvent<int, int> OnQuestCompleteCountChanged;

        // 현재 진행 중인 퀘스트
        public QuestData.Quest CurrentQuest
        {
            get
            {
                // currentQuestID 값을 배열의 인덱스로 사용해 현재 진행 중인 퀘스트 데이터를 반환
                return DataManager.Instance.questData.quests[currentQuestID];
            }
        }

        // 다음 퀘스트
        public QuestData.Quest NextQuest
        {
            get
            {
                // 다음에 진행할 퀘스트 ID 계산
                // 모든 퀘스트를 완료했으면 인덱스를 고정하고, 진행할 퀘스트가 남았으면 +1
                int nextQuestID = currentQuestID >= DataManager.Instance.questData.quests.Count - 1 ? currentQuestID : currentQuestID + 1;

                // nextQuestID를 배열 인덱스로 사용해 퀘스트 데이터 반환
                return DataManager.Instance.questData.quests[nextQuestID];
            }
        }

        private void Awake()
        {
            // 싱글턴 인스턴스가 null인 경우, 인스턴스 설정
            if (Instance == null)
            {
                Instance = this;
            }

            // 이미 다른 객체가 설정돼 있다면, 싱글턴 유지를 위해 중복되는 게임 오브젝트 제거
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            // 퀘스트 진행을 시작할 수 있도록 게임 시작 시 퀘스트 상태 확인
            CheckState();

            // 진행하는 퀘스트 타입이 몬스터 킬(Kill)일 때 발생하는 이벤트에 함수 등록
            OnQuestStarted?.AddListener(SpawnMonsters);
        }

        // 퀘스트 상태 설정 함수
        public void SetState(QuestState state)
        {
            this.state = state;
        }

        // 퀘스트 상태 확인 함수
        public QuestState CheckState()
        {
            // 현재 완료한 횟수가 퀘스트 데이터에서 요구하는 완료 횟수와 같은지 확인
            if (completeCount >= CurrentQuest.countToComplete)
            {
                // 퀘스트에서 요구하는 완료 횟수를 채웠으면, 퀘스트 완료 처리
                state = QuestState.Complete;
            }

            // 완료 횟수는 못채웠다면,
            else if (currentQuestID > 0)
            {
                // 퀘스트 진행 처리
                state = QuestState.Processing;
            }

            // 위에서 설정한 퀘스트 상태 반환
            return state;
        }

        // 퀘스트를 달성했을 때 실행하는 함수
        public void ProcessQuest(QuestData.Type type, QuestData.TargetType targetType)
        {
            // 퀘스트를 모두 완료했으면 퀘스트 진행 안 하고 종료
            if (isQuestCompleted)
            {
                return;
            }

            // 완료한 퀘스트는 진행하지 않음(함수 종료)
            if (state == QuestState.Complete)
            {
                return;
            }

            // 현재 진행 중인 퀘스트와 다르면 함수 종료
            if (CurrentQuest.type != type || CurrentQuest.targetType != targetType)
            {
                string message = $"현재 진행 중인 퀘스트가 아닙니다. {CurrentQuest.type} != {type} && {CurrentQuest.targetType} != {targetType}";
                Dialogue.ShowDialogueTextTemporarily(message);

                return;
            }

            // 퀘스트 달성 횟수 업데이트
            ++completeCount;

            // 퀘스트 달성 횟수 업데이트 이벤트 발행
            OnQuestCompleteCountChanged?.Invoke(currentQuestID, completeCount);

            // 현재 퀘스트를 완료했는지 확인
            if (CheckState() == QuestState.Complete)
            {
                // 다음 퀘스트가 남은 경우
                if (currentQuestID < DataManager.Instance.questData.quests.Count - 1)
                {
                    Dialogue.ShowDialogueTextTemporarily($"{CurrentQuest.questTitle} 퀘스트를 완료 했습니다.\n다음 퀘스트를 진행할 수 있습니다. 담당 NPC와 대화하세요.");
                }

                // 모든 퀘스트를 완료한 경우
                else
                {
                    // 모든 퀘스트를 완료 처리
                    isQuestCompleted = true;

                    // 모든 퀘스트를 완료했다는 이벤트 발행
                    OnAllQuestsCompleted?.Invoke();
                }

                // 퀘스트 달성으로 획득한 경험치 전달

                // 퀘스트 달성 이벤트 발행
                OnQuestCompleted?.Invoke(currentQuestID);
            }
        }

        // 대화를 진행하는 NPC가 현재 퀘스트 담당 NPC인지 확인
        public QuestState CheckNPCState(int npcID)
        {
            // 현재 퀘스트 ID가 다음 퀘스트의 시작 조건이며,
            // 현재 퀘스트 스테이트가 완료이고,
            // 다음 퀘스트 진행 NPC 아이디와 전달된 NPC의 아이디가 같으면,
            // 퀘스트 시작 상태로 반환
            if (currentQuestID == NextQuest.startCondition
                && state == QuestState.Complete
                && NextQuest.npcID == npcID)
            {
                return QuestState.Start;
            }

            // 현재 퀘스트 아이디가 현재 퀘스트 아이디이고
            // 현재 퀘스트의 스테이트가 진행 중이며
            // 현재 퀘스트 진행 NPC 아이디와 전달된 NPC의 아이디가 같으면,
            // 퀘스트 진행 상태로 반환
            if (currentQuestID == CurrentQuest.questID
                && state == QuestState.Processing
                && CurrentQuest.npcID == npcID)
            {
                return QuestState.Processing;
            }

            // 위의 두 조건에 맞지 않으면 None 상태 반환
            return QuestState.None;
        }

        // 다음 퀘스트로 넘어갈 때 실행하는 함수
        public void MoveToNextQuest()
        {
            ++currentQuestID;
            completeCount = 0;

            // 퀘스트 시작 이벤트 발행
            OnQuestStarted?.Invoke(currentQuestID);
        }

        // 퀘스트에 따라서 몬스터를 생성하는 함수
        public void SpawnMonsters(int questID)
        {
            // 이번에 진행할 퀘스트가 몬스터를 제거하는 퀘스트일 때 그 수에 맞게 몬스터 생성
            if (CurrentQuest.type == QuestData.Type.Kill && CurrentQuest.targetType == QuestData.TargetType.Chomper)
            {
                // 퀘스트 완료에 필요한 수만큼 몬스터 생성
                MonsterSpawner.SpawnMonsters(CurrentQuest.countToComplete, CurrentQuest.monsterLevel);
            }

            // 이번에 진행할 퀘스트가 웨이브이면, 웨이브 시작
            else if (CurrentQuest.type == QuestData.Type.Wave && CurrentQuest.targetType == QuestData.TargetType.Chomper)
            {
                // 몬스터 웨이브 시작
                MonsterSpawner.StartMonsterWave();
            }

            // 이번에 진행할 퀘스트가 보스 몬스터를 제거하는 퀘스트라면, 보스 몬스터 생성

        }

        // 퀘스트 시작 이벤트에 등록하는 함수
        public void SubscribeOnQuestStarted(UnityAction<int> listener)
        {
            OnQuestStarted?.AddListener(listener);
        }

        // 퀘스트 달성 이벤트에 등록하는 함수
        public void SubscribeOnQuestCompleted(UnityAction<int> listener)
        {
            OnQuestCompleted?.AddListener(listener);
        }

        // 퀘스트 완료 횟수가 변경될 때 발행되는 이벤트에 등록하는 함수
        public void SubscribeOnQuestCompleteCountChanged(UnityAction<int, int> listener)
        {
            OnQuestCompleteCountChanged?.AddListener(listener);
        }
    }
}