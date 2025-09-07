using System.Collections.Generic;
using UnityEngine;

namespace RPGGame
{
    [CreateAssetMenu]
    public class NPCData : ScriptableObject
    {
        // NPC의 데이터 속성을 갖는 클래스
        [System.Serializable]
        public class Attribute
        {
            // NPC 아이디
            public int id = 0;

            // NPC 이름
            public string name = string.Empty;

            // NPC의 대화 가능 거리
            public float interactionSight = 0f;
        }

        // NPC 데이터를 저장하는 리스트 변수
        public List<Attribute> attributes = new List<Attribute>();
    }
}