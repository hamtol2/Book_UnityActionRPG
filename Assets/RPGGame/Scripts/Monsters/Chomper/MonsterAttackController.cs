using UnityEngine;

namespace RPGGame
{
    // 몬스터의 공격 판정 및 대미지 전달 기능을 담당하는 스크립트
    public class MonsterAttackController : MonoBehaviour
    {
        // 공격력
        [SerializeField] private float attackAmount = 0f;

        // 공격 중인지를 나타내는 불리언 변수
        [SerializeField] private bool isInAttack = false;

        // 공격 판정에 사용할 구체 충돌체의 반지름
        [SerializeField] private float radius = 0.5f;

        // 공격 판정에 사용할 레이어
        [SerializeField] private LayerMask attackTargetLayer;

        // 공격 판정에 사용하는 공격 위치 트랜스폼
        private Transform refTransform;

        private void Awake()
        {
            // 트랜스폼 참조 변수 설정
            if (refTransform == null)
            {
                refTransform = transform;
            }
        }

        // 몬스터의 공격력을 설정하는 함수
        public void SetAttack(float attackAmount)
        {
            this.attackAmount = attackAmount;
        }

        private void FixedUpdate()
        {
            // 공격 판정을 하지 않을 때는 함수 종료
            if (!isInAttack)
            {
                return;
            }

            // 공격 판정 진행

            // 공격 위치, 반지름을 사용해 충돌 감지
            Collider[] colliders = Physics.OverlapSphere(refTransform.position, radius, attackTargetLayer);

            // 충돌한 충돌체가 없으면 함수 종료
            if (colliders.Length == 0)
            {
                return;
            }

            // 앞선 충돌 감지 과정에서 충돌한 충돌체가 있으면 대미지 전달
            foreach (var collider in colliders)
            {
                // 충돌체에서 대미지 전달을 위한 Damageable 컴포넌트 검색
                Damageable damageable = collider.GetComponent<Damageable>();

                // 검색에 성공했으면 대미지 전달
                if (damageable != null)
                {
                    // 대미지 전달
                    damageable.ReceiveDamage(attackAmount);

                    // 대미지를 전달했으면 공격 종료
                    isInAttack = false;
                    return;
                }
            }
        }

        // 공격이 시작될 때 실행될 이벤트 리스너 함수
        public void OnAttackBegin()
        {
            // 공격 판정을 시작하도록 변수 설정
            isInAttack = true;
        }

        // 공격이 종료되면 실행될 이벤트 리스너 함수
        public void OnAttackEnd()
        {
            // 공격 판정을 종료하도록 변수 설정
            isInAttack = false;
        }

        // 화면에 기즈모를 그리는 함수
        private void OnDrawGizmos()
        {
            // 유니티 에디터에서만 실행되도록 전처리
#if UNITY_EDITOR
            // 트랜스폼 참조 변수 설정
            if (refTransform == null)
            {
                refTransform = transform;
            }

            // 기즈모의 색상을 빨간색으로 설정
            Gizmos.color = Color.red;

            // 구체 그리기
            Gizmos.DrawWireSphere(refTransform.position, radius);
#endif
        }
    }
}