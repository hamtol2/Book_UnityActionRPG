using System.Collections.Generic;
using UnityEngine;

namespace RPGGame
{
    // 몬스터가 사용할 데이터 스크립트.
    public class MonsterData : ScriptableObject
    {
        // 몬스터의 레벨 데이터.
        [System.Serializable]
        public class LevelData
        {
            // 몬스터 레벨.
            public int level = 1;

            // 체력.
            public float maxHP = 0f;

            // 공격력.
            public float attack = 0f;

            // 범위 공격력(보스 몬스터가 사용).
            public float rangeAttack = 0f;

            // 방어력.
            public float defense = 0f;

            // 플레이어가 이 레벨의 몬스터를 잡았을 때 획득할 경험치.
            public float gainExp = 0f;
        }

        // 몬스터의 모든 레벨 데이터를 갖가지는 리스트(동적 배열).
        public List<LevelData> levels;

        // 정찰 전까지 기다리는 시간 (단위: 초).
        public float patrolWaitTime = 3f;

        // 정찰할 때 회전하는 속도 (단위: 각도/초).
        public float patrolRotateSpeed = 360f;

        // 시야 각도의 절반 값 (단위: 각도).
        public float sightAngle = 60f;

        // 시야 거리 (단위: 미터).
        public float sightRange = 10f;

        // 쫒아갈 때 회전하는 속도 (단위: 각도/초).
        public float chaseRotateSpeed = 540f;

        // 공격 가능 거리 (단위: 미터).
        public float attackRange = 2f;

        // 원거리 공격 가능 거리(보스 몬스터가 사용. 단위: 미터).
        public float rangeAttackRange = 6f;
    }
}