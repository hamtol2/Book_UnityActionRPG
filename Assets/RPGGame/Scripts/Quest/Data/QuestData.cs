using System.Collections.Generic;
using UnityEngine;

namespace RPGGame
{
    // 플레이어가 사용할 데이터 스크립트
    // CreateAssetMenu는 유니티 에디터의 Asset 메뉴를 통해 이 ScriptableObject를 생성할 수 있도록 해준다.
    [CreateAssetMenu]
    public class QuestData : ScriptableObject
    {
        // 퀘스트 타입.
        public enum Type
        {
            None = -1,
            CollectWeapon,
            Kill,
            Wave,
            Length
        }

        // 퀘스트 타깃 타입
        public enum TargetType
        {
            None = -1,
            Player,
            Chomper,
            Grenadier,
            Length
        }

        // 퀘스트 정보를 갖는 클래스
        [System.Serializable]
        public class Quest
        {
            // 퀘스트 아이디
            public int questID = 0;

            // 퀘스트 타이틀
            public string questTitle = string.Empty;

            // 퀘스트 타입
            public Type type = Type.None;

            // 퀘스트 타깃 타입
            public TargetType targetType = TargetType.None;

            // 퀘스트 달성에 필요한 횟수
            public int countToComplete = 0;

            // 퀘스트를 달성했을 때 획득할 수 있는 경험치
            public float exp = 0f;

            // 퀘스트 시작 텍스트
            public string questBeginText = string.Empty;

            // 퀘스트 진행 텍스트
            public string questProgressText = string.Empty;

            // 퀘스트 담당이 아닌 NPC를 만났을 때 NPC 대사
            public string smallTalk = string.Empty;

            // 퀘스트 시작 조건
            public int startCondition = 0;

            // 다음 퀘스트 아이디
            public int nextQuestID = 0;

            // 퀘스트 담당 NPC 아이디
            public int npcID = 0;

            // 몬스터를 잡는 퀘스트일 때 사용할 몬스터의 레벨
            public int monsterLevel = 0;
        }

        // 퀘스트 목록
        public List<Quest> quests = new List<Quest>();
    }
}