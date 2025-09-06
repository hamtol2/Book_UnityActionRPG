using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace RPGGame
{
    // CSV 데이터 파일을 읽어와서 PlayerData Scriptable Object 애셋을 지정한 경로에 생성하는 스크립트.
    public class GameDataCreator
    {
        // 모든 데이터 애셋을 저장할 폴더.
        private static readonly string dataFolderPath = "Assets/RPGGame/Resources/Data";

        // PlayerLevelData.csv 데이터 파일 경로.
        private static readonly string playerLevelDataFilePath = "Assets/RPGGame/Data/Editor/PlayerLevelData.csv";

        // PlayerData 애셋의 저장 경로.
        private static readonly string playerDataSOFilePath = "Assets/RPGGame/Resources/Data/Player Data.asset";

        // MonsterLevelData.csv 데이터 파일 경로.
        private static readonly string monsterLevelDataFilePath = "Assets/RPGGame/Data/Editor/MonsterLevelData.csv";

        // MonsterData 애셋의 저장 경로.
        private static readonly string monsterDataSOFilePath = "Assets/RPGGame/Resources/Data/Monster Data.asset";

        // 데이터 애셋을 저장할 폴더가 있는지 확인하고, 없으면 생성하는 함수.
        private static void CheckAndCreateDataFolder()
        {
            // 데이터 애셋을 생성할 폴더 생성.
            if (!Directory.Exists(dataFolderPath))
            {
                Directory.CreateDirectory(dataFolderPath);
            }
        }

        // 유니티 에디터 상단에 메뉴를 생성해주는 구문.
        [MenuItem("RPGGame/Create Player Data")]
        private static void CreatePlayerData()
        {
            // 데이터 애셋 저장 폴더 확인 및 생성.
            CheckAndCreateDataFolder();

            // 먼저 PlayerData Scriptable Object 파일이 있는지 검색.
            PlayerData playerDataSO = AssetDatabase.LoadAssetAtPath(playerDataSOFilePath, typeof(PlayerData)) as PlayerData;

            // PlayerData 파일이 없으면, 지정한 경로에 애셋 새로 생성.
            if (playerDataSO == null)
            {
                // PlayerData 스크립터블 오브젝트 인스턴스 생성.
                playerDataSO = ScriptableObject.CreateInstance<PlayerData>();

                // 생성한 인스턴스를 파일로 저장.
                AssetDatabase.CreateAsset(playerDataSO, playerDataSOFilePath);
            }

            // CSV 파일을 줄 별로 읽어서 문자열 리스트에 저장.
            string[] lines = File.ReadAllLines(playerLevelDataFilePath);

            // 레벨 데이터 초기화.
            playerDataSO.levels = new List<PlayerData.LevelData>();

            // CSV 파일의 첫 번째 줄은 데이터로는 필요하지 않으므로기 때문에 무시하고 두 번째 줄 부터 루프 처리.
            for (int ix = 1; ix < lines.Length; ++ix)
            {
                // CSV 는 콤마(,)로 데이터를 구분하므로기 때문에 문자열을 콤마를 기준으로 분리해 저장.
                string[] data = lines[ix].Split(',', System.StringSplitOptions.RemoveEmptyEntries);

                // 레벨 데이터 생성.
                PlayerData.LevelData levelData = new PlayerData.LevelData();
                levelData.level = int.Parse(data[0]);           // 첫 번째 데이터는 레벨.
                levelData.maxHP = float.Parse(data[1]);         // 두 번째 데이터는 체력.
                levelData.requiredExp = float.Parse(data[2]);   // 세 번째 데이터는 필요한 경험치.

                // 생성한 데이터를 PlayerData Scriptable Object에 추가.
                playerDataSO.levels.Add(levelData);
            }

            // 애셋이 변경됐다고 표시.
            EditorUtility.SetDirty(playerDataSO);

            // 애셋 저장.
            AssetDatabase.SaveAssets();
        }

        // 유니티 에디터 상단에 메뉴를 생성해주는 구문.
        [MenuItem("RPGGame/Create Monster Data")]
        private static void CreateMonsterData()
        {
            // 데이터 애셋 저장 폴더 확인 및 생성.
            CheckAndCreateDataFolder();

            // 먼저 MonsterData Scriptable Object 파일이 있는지 검색.
            MonsterData monsterDataSO = AssetDatabase.LoadAssetAtPath(monsterDataSOFilePath, typeof(MonsterData)) as MonsterData;

            // MonsterData 파일이 없으면, 지정한 경로에 애셋 새로 생성.
            if (monsterDataSO == null)
            {
                // MonsterData 스크립터블 오브젝트 인스턴스 생성.
                monsterDataSO = ScriptableObject.CreateInstance<MonsterData>();

                // 생성한 인스턴스를 파일로 저장.
                AssetDatabase.CreateAsset(monsterDataSO, monsterDataSOFilePath);
            }

            // CSV 파일을 줄 별로 읽어서 문자열 리스트에 저장.
            string[] lines = File.ReadAllLines(monsterLevelDataFilePath);

            // 레벨 데이터 초기화.
            monsterDataSO.levels = new List<MonsterData.LevelData>();

            // CSV 파일의 첫 번째 줄은 데이터로는 필요하지 않기 때문에 무시하고 두 번째 줄 부터 루프 처리.
            for (int ix = 1; ix < lines.Length; ++ix)
            {
                // CSV 는 콤마(,)로 데이터를 구분하기 때문에 문자열을 콤마를 기준으로 분리해 저장.
                string[] data = lines[ix].Split(',', System.StringSplitOptions.RemoveEmptyEntries);

                // 레벨 데이터 생성.
                MonsterData.LevelData levelData = new MonsterData.LevelData();
                levelData.level = int.Parse(data[0]);           // 첫 번째 데이터는 레벨.
                levelData.maxHP = float.Parse(data[1]);         // 두 번째 데이터는 공격력.
                levelData.attack = float.Parse(data[2]);        // 세 번째 데이터는 공격력.
                levelData.defense = float.Parse(data[3]);       // 네 번째 데이터는 방어력.
                levelData.gainExp = float.Parse(data[4]);       // 다섯 번째 데이터는 획득 경험치.

                // 생성한 데이터를 MonsterData Scriptable Object에 추가.
                monsterDataSO.levels.Add(levelData);
            }

            // 애셋이 변경됐다고 표시.
            EditorUtility.SetDirty(monsterDataSO);

            // 애셋 저장.
            AssetDatabase.SaveAssets();
        }
    }
}