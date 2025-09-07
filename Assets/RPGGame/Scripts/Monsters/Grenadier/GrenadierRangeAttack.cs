using UnityEngine;

namespace RPGGame
{
    // 보스 몬스터의 범위 공격 처리를 담당하는 스크립트
    public class GrenadierRangeAttack : MonoBehaviour
    {
        // 공격력
        [SerializeField] private float attackAmount = 0f;

        // 공격 판정에 사용할 반지름
        [SerializeField] private float radius = 1f;

        // 공격 판정에 사용할 레이어
        [SerializeField] private LayerMask attackLayerMask;

        // 트랜스폼 컴포넌트 참조 변수
        private Transform refTransform;

        private void Awake()
        {
            // 트랜스폼 컴포넌트 참조 변수 설정
            if (refTransform == null)
            {
                refTransform = transform;
            }

            // 광역 공격 이벤트에 함수 등록을 위한 공격 스테이트 컴포넌트 검색
            GrenadierAttackState attackState = GetComponentInParent<GrenadierAttackState>();
            if (attackState != null)
            {
                // 광역 공격 판정을 시작하는 이벤트에 함수 등록
                attackState.SubscribeOnRangeAttackStart(Attack);
            }
        }

        // 범위 공격 판정 함수
        public void Attack()
        {
            // 구체 모형으로 충돌 판정
            Collider[] colliders = Physics.OverlapSphere(refTransform.position, radius, attackLayerMask);

            // 충돌한 물체가 없으면 함수 종료
            if (colliders.Length == 0)
            {
                return;
            }

            // 충돌한 물체를 루프를 통해 대미지 전달
            foreach (Collider collider in colliders)
            {
                // 충돌한 물체에서 Damageable 컴포넌트를 검색
                Damageable damageable = collider.GetComponent<Damageable>();
                if (damageable == null)
                {
                    continue;
                }

                // 컴포넌트 검색에 성공했으면, 대미지 전달
                damageable.ReceiveDamage(attackAmount);
            }
        }

        // 공격력 설정 함수
        public void SetAttack(float attack)
        {
            attackAmount = attack;
        }

        // 공격 범위 설정 함수
        public void SetAttackRange(float range)
        {
            radius = range;
        }

        // 유니티 에디터에서만 실행되도록 전처리 구문 적용
#if UNITY_EDITOR
        // 기즈모를 그릴 때 사용할 수 있는 함수(유니티 이벤트 함수)
        private void OnDrawGizmos()
        {
            // 트랜스폼 참조 변수가 설정되지 않았으면, 설정
            if (refTransform == null)
            {
                refTransform = transform;
            }

            // 기즈모 색상 설정
            Gizmos.color = Color.cyan;

            // 선 형태로 구체를 그린다.
            Gizmos.DrawWireSphere(refTransform.position, radius);
        }
#endif
    }
}