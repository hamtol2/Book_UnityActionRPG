using UnityEngine;
using UnityEngine.AI;

namespace RPGGame
{
    // 여러 편의 기능을 제공하는 유틸리티 클래스
    public class Util
    {
        // Debug.Log 대체 함수
        public static void Log(object message)
        {
#if UNITY_EDITOR
            Debug.Log($"{message}");
#endif
        }

        public static void LogRed(object message)
        {
#if UNITY_EDITOR
            Debug.Log($"<color=red>{message}</color>");
#endif
        }

        public static void LogGreen(object message)
        {
#if UNITY_EDITOR
            Debug.Log($"<color=green>{message}</color>");
#endif
        }

        public static void LogBlue(object message)
        {
#if UNITY_EDITOR
            Debug.Log($"<color=blue>{message}</color>");
#endif
        }

        // 목적지에 도착했는지를 확인할 때 사용하는 함수
        public static bool IsArrived(Transform selfTransform, Vector3 destination, float offset = 0.1f)
        {
            // 두 지점 사이의 거리를 계산한 값이 허용 오차 범위(offset)보다 작으면 도착했다고 판단
            return Vector3.Distance(selfTransform.position, destination) < offset;
        }

        // 시야 판정에 사용하는 함수(시야각과 시야 거리를 모두 검사)
        public static bool IsInSight(Transform selfTransform, Transform target, float sightAngle, float sightRange)
        {
            // 시야각 계산을 위해 플레이어로 향하는 방향 구하기
            Vector3 direction = target.position - selfTransform.position;
            direction.Normalize();

            // 트랜스폼의 앞방향(forward) 벡터와 몬스터에서 플레이어로 향하는 벡터 사이의 각도를 계산
            // 이 값이 시야각 내에 포함되는지 확인
            if (Vector3.Angle(selfTransform.forward, direction) <= sightAngle)
            {
                // 시야각 안에 들어왔으면 시야 거리 안에도 들어왔는지 한번 더 확인
                if (Vector3.Distance(selfTransform.position, target.position) <= sightRange)
                {
                    // 이 경우에는 시야에 들어왔다고 판단
                    return true;
                }
            }

            // 위에서 확인한 경우가 아니라면, 시야 범위 밖에 있다고 판단
            return false;
        }

        // center를 기준으로 range의 범위에서 이동 가능한 위치를 랜덤으로 구한 뒤 반환하는 함수
        // 랜덤 위치 선택을 30회 시도해보고 그 횟수 안에서 이동 가능한 위치를 찾으면 해당 위치를 반환
        // 30회를 모두 시도했는데 이동 가능한 위치를 찾지 못했으면 실패
        public static bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {
            // 랜덤 위치 선택을 30회 시도
            for (int ix = 0; ix < 30; ++ix)
            {
                // 단위 구(반지름이 1인 구)에서 임의로 한 위치를 구한 뒤, 정찰 거리 값(range)을 곱해서
                // 반지름이 정찰 거리인 구체에서 임의로 한 위치를 결정하는 로직
                Vector3 randomPoint = center + Random.insideUnitSphere * range;

                // 내비게이션 시스템을 통해 앞서 구한 위치가 이동 가능한 위치인지 판단
                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    // 이동 가능한 위치인 경우, 이 위치를 결과인 result에 설정
                    result = hit.position;

                    // 이동 가능하다는 의미로 true를 반환
                    return true;
                }
            }

            // 랜덤으로 선택한 30회 모두 이동 가능한 위치 선택이 안된 경우에는 일단 결과 위치를 (0, 0, 0)으로 설정
            result = Vector3.zero;

            // 랜덤 위치 선택 실패
            return false;
        }
    }
}