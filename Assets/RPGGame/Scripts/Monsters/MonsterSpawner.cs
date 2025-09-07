using System;
using System.Collections;
using UnityEngine;

namespace RPGGame
{
    // 몬스터를 생성하는 기능을 담당하는 스크립트
    public class MonsterSpawner : MonoBehaviour
    {
        // 몬스터 웨이브 정보를 저장하는 클래스
        [Serializable]
        public class MonsterWave
        {
            // 생성할 몬스터의 수
            public int count;

            // 생성할 몬스터의 레벨
            public int monsterLevel;

            // 웨이브를 생성할 때 다이얼로그에 보여줄 메시지
            [TextArea(2, 10)]
            public string spawnMessage;
        }

        // 싱글턴 기능을 제공하기 위한 인스턴스 변수
        private static MonsterSpawner instance = null;

        // 몬스터를 생성할 때 사용할 프리팹
        [SerializeField] private GameObject chomperMonsterPrefab;

        // 몬스터를 생성할 스폰 위치 배열
        [SerializeField] private Transform[] spawnPositions;

        // 몬스터 웨이브 정보
        [SerializeField] private MonsterWave[] monsterWaves;

        // 몬스터 웨이브가 시작됐는지를 나타내는 변수
        [SerializeField] private bool isWaveStarted = false;

        // 몬스터 웨이브 ID
        [SerializeField] private int currentWaveID = 0;

        private void Awake()
        {
            // 싱글턴 인스턴스가 설정되지 않았으면, 할당
            if (instance == null)
            {
                instance = this;
            }

            // 이미 생성됐다면 중복되는 게임 오브젝트 삭제
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            // 몬스터를 생성할 위치를 검색해 배열에 저장
            var transforms = GetComponentsInChildren<Transform>();

            // 검색한 트랜스폼 중 첫 번째는 사용하지 않을 것이므로 검색한 수 중에서 1개 제외
            spawnPositions = new Transform[transforms.Length - 1];

            // 검색한 트랜스폼 중 첫 번째는 생성 위치로 사용하지 않기 때문에 두 번째 트랜스폼부터 저장
            Array.Copy(transforms, 1, spawnPositions, 0, spawnPositions.Length);
        }

        // 생성할 몬스터의 수를 전달해 원하는 만큼의 몬스터를 생성할 수 있는 함수
        // 몬스터의 위치는 미리 저장해둔 생성 지점(Spawn Position)들 중에서 무작위로 선택해 생성
        public static void SpawnMonsters(int count, int monsterLevel)
        {
            // 전달받은 수만큼 루프를 통해 몬스터 생성
            for (int ix = 0; ix < count; ++ix)
            {
                SpawnAMonster(monsterLevel);
            }
        }

        // 몬스터 하나를 생성하는 함수
        private static void SpawnAMonster(int monsterLevel)
        {
            // 몬스터를 생성할 위치를 랜덤으로 선택
            int spawnPositionIndex = UnityEngine.Random.Range(0, instance.spawnPositions.Length);

            // 몬스터 생성 위치/회전 저장
            Vector3 spawnLocation = instance.spawnPositions[spawnPositionIndex].position;
            Quaternion spawnRotation = instance.spawnPositions[spawnPositionIndex].rotation;

            // 생성
            GameObject newMonster = Instantiate(instance.chomperMonsterPrefab, spawnLocation, spawnRotation);

            // 생성한 몬스터의 레벨 설정
            MonsterStateManager stateManager = newMonster.GetComponent<MonsterStateManager>();
            stateManager.SetLevel(monsterLevel);

            // 웨이브가 시작된 경우에는 몬스터가 시야 밖에 있더라도 플레이어를 쫓아가도록 설정
            if (instance.isWaveStarted)
            {
                // 몬스터가 플레이어를 쫓아가도록 설정
                stateManager.SetForceToChase();
            }
        }

        // 간격을 두고 여러 몬스터를 하나씩 생성하는 코루틴 함수
        private static IEnumerator SpawnMonstersCoroutine(int count, int monsterLevel)
        {
            // 0.2초 간격으로 몬스터 생성
            for (int ix = 0; ix < count; ++ix)
            {
                yield return instance.StartCoroutine(SpawnOneMonster(0.2f, monsterLevel));
            }
        }

        // 전달받은 시간만큼 대기 후 몬스터 하나를 생성하는 함수
        private static IEnumerator SpawnOneMonster(float time, int monsterLevel)
        {
            // 전달받은 시간만큼 대기
            yield return new WaitForSeconds(time);

            // 몬스터 생성
            SpawnAMonster(monsterLevel);
        }

        // 전달한 시간만큼 대기했다가 SpawnMonsters 함수를 호출
        private static IEnumerator SpawnMonstersWithDelay(float time, int count, int monsterLevel)
        {
            // 전달한 시간만큼 대기
            yield return new WaitForSeconds(time);

            // 몬스터 생성 함수 호출
            instance.StartCoroutine(SpawnMonstersCoroutine(count, monsterLevel));
        }

        // 몬스터 웨이브를 시작할 때 사용하는 함수
        public static void StartMonsterWave()
        {
            // 이미 웨이브를 시작했다면 다음 웨이브 진행
            if (instance.isWaveStarted)
            {
                MoveToNextWave();
                return;
            }

            // 웨이브 시작
            instance.isWaveStarted = true;

            // 웨이스 시작 ID 설정
            instance.currentWaveID = 0;

            // 몬스터 생성
            int count = instance.monsterWaves[instance.currentWaveID].count;                    // 생성할 몬스터 수
            int monsterLevel = instance.monsterWaves[instance.currentWaveID].monsterLevel;      // 생성할 몬스터 레벨

            // 2초 후 몬스터 생성
            instance.StartCoroutine(SpawnMonstersWithDelay(2f, count, monsterLevel));

            // 다이얼로그에 메시지 출력
            Dialogue.ShowDialogueTextTemporarily(instance.monsterWaves[instance.currentWaveID].spawnMessage);
        }

        // 다음 웨이브로 넘어갈 때 사용하는 함수
        private static void MoveToNextWave()
        {
            // 이미 모든 웨이브를 완료했는지 확인
            if (instance.currentWaveID == instance.monsterWaves.Length - 1)
            {
                return;
            }

            // 웨이브 ID 업데이트
            ++instance.currentWaveID;

            // 몬스터 생성.
            int count = instance.monsterWaves[instance.currentWaveID].count;                    // 생성할 몬스터 수
            int monsterLevel = instance.monsterWaves[instance.currentWaveID].monsterLevel;      // 생성할 몬스터 레벨

            // 3초 후 몬스터 생성
            instance.StartCoroutine(SpawnMonstersWithDelay(3f, count, monsterLevel));

            // 다이얼로그에 메시지 출력
            Dialogue.ShowDialogueTextTemporarily(instance.monsterWaves[instance.currentWaveID].spawnMessage);
        }
    }
}