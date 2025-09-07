using UnityEngine;

namespace RPGGame
{
    // 보스 몬스터(Grenadier) 스테이트의 기반 스크립트
    public class GrenadierStateBase : MonoBehaviour
    {
        // 스테이트 관리자 참조 변수
        protected GrenadierStateManager manager;

        // 트랜스폼 컴포넌트 참조 변수
        protected Transform refTransform;

        protected virtual void OnEnable()
        {
            // 스테이트 관리자 설정
            if (manager == null)
            {
                manager = GetComponent<GrenadierStateManager>();
            }

            // 트랜스폼 참조 변수 설정
            if (refTransform == null)
            {
                refTransform = transform;
            }
        }

        protected virtual void Update()
        {
        }

        protected virtual void OnDisable()
        {
        }
    }
}