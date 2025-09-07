using UnityEngine;
using UnityEngine.Events;

namespace RPGGame
{
    // 데이터를 관리하는 관리자 스크립트
    // 데이터 관련 스크립트는 다른 스크립트보다 먼저 처리돼야 하기 때문에 실행 순서를 앞으로 설정
    [DefaultExecutionOrder(-100)]
    public class DataManager : MonoBehaviour
    {
        // 싱글턴을 제작하기 위한 인스턴스 프로퍼티
        public static DataManager Instance { get; private set; } = null;

        // 플레이어 데이터 Scriptable Object
        public PlayerData playerData { get; private set; }

        // 몬스터 데이터 Scriptable Object
        public MonsterData monsterData { get; private set; }

        // 퀘스트 데이터 Scriptable Object
        public QuestData questData { get; private set; }

        // NPC 데이터 Scriptable Object
        public NPCData npcData { get; private set; }

        // 보스 몬스터 데이터 Scriptable Object
        public MonsterData grenadierData { get; private set; }

        private void Awake()
        {
            // 싱글턴 인스턴스가 null이면, 인스턴스 설정 및 초기화
            if (Instance == null)
            {
                // 싱글턴 인스턴스 설정
                Instance = this;

                // 초기화 함수 호출
                Initialize();
            }
            // 이미 다른 객체가 설정돼 있다면, 싱글턴 유지를 위해 중복되는 게임 오브젝트 제거
            else
            {
                Destroy(gameObject);
            }
        }

        private void Initialize()
        {
            // 플레이어 데이터 초기화
            if (playerData == null)
            {
                // Resources.Load 함수를 사용해 플레이어 데이터 로드
                playerData = Resources.Load<PlayerData>("Data/Player Data");

                // 로드한 데이터를 확인 후 초기화되지 않았다면 오류 출력
                if (playerData.levels.Count == 0)
                {
                    Debug.LogError("플레이어의 레벨 데이터가 초기화되지 않았습니다.");
                }
            }

            // 몬스터 데이터 초기화
            if (monsterData == null)
            {
                // Resources.Load 함수를 사용해 몬스터 데이터 로드
                monsterData = Resources.Load<MonsterData>("Data/Monster Data");

                // 로드한 데이터를 확인 후 초기화되지 않았다면 오류 출력
                if (monsterData.levels.Count == 0)
                {
                    Debug.LogError("몬스터의 레벨 데이터가 초기화되지 않았습니다.");
                }
            }

            // 퀘스트 데이터 초기화
            if (questData == null)
            {
                // Resources.Load 함수를 사용해 퀘스트 데이터 로드
                questData = Resources.Load<QuestData>("Data/Quest Data");

                // 로드한 데이터를 확인 후 초기화되지 않았다면 오류 출력
                if (questData.quests.Count == 0)
                {
                    Debug.LogError("퀘스트 데이터가 초기화되지 않았습니다.");
                }
            }
            
            // NPC 데이터 초기화
            if (npcData == null)
            {
                // NPC 데이터 파일 검색 후 설정
                npcData = Resources.Load<NPCData>("Data/NPC Data");

                // 데이터 초기화가 안 된 경우에는 오류 출력
                if (npcData.attributes.Count == 0)
                {
                    Debug.LogError("NPC 데이터가 초기화되지 않았습니다.");
                }
            }

            // 보스 몬스터 데이터 초기화
            if (grenadierData == null)
            {
                grenadierData = Resources.Load<MonsterData>("Data/Grenadier Data");
                if (grenadierData.levels.Count == 0)
                {
                    Debug.LogError("보스 몬스터의 레벨 데이터가 초기화되지 않았습니다.");
                }
            }
        }
    }
}