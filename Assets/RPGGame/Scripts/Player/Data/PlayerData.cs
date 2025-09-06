using System.Collections.Generic;
using UnityEngine;

namespace RPGGame
{
    // 플레이어가 사용할 데이터 스크립트.
    public class PlayerData : ScriptableObject
    {
        // 플레이어의 레벨 데이터.
        [System.Serializable]
        public class LevelData
        {
            // 레벨 값.
            public int level = 1;

            // 이 레벨의 최대 체력.
            public float maxHP = 0f;

            // 이 레벨에 도달하기까지 필요한 경험치 (누적 경험치).
            public float requiredExp = 0f;
        }

        // 플레이어의 모든 레벨 데이터를 간가지는 리스트(동적 배열).
        public List<LevelData> levels;

        // 플레이어의 최소/최대 회전 속도(단위: 각도/초).
        public float rotationSpeed = 1000f;

        // 플레이어가 점프를 할 때 사용할 값 (단위: 미터/초).
        public float jumpPower = 8f;

        // 플레이어가 점프 상태에서 받는 중력 값 (단위: 미터/초).
        public float gravityInJump = 10f;
    }
}