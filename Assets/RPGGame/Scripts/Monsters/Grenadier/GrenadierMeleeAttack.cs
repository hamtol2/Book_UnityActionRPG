using UnityEngine;

namespace RPGGame
{
    // 보스 몬스터의 근접 공격 처리를 담당하는 스크립트
    public class GrenadierMeleeAttack : MonoBehaviour
    {
        // 공격력
        [SerializeField] private float attackAmount = 0f;

        // 공격 판정에 사용할 충돌 영역의 반지름
        [SerializeField] private float radius = 0.2f;

        // 공격 판정에 사용할 위치
        [SerializeField] private Transform[] attackPoints;

        // 공격 판정 대상 레이어
        [SerializeField] private LayerMask attackTargetLayer;

        // 근접 공격을 판정할 때 부모로 지정할 트랜스폼(모델링의 뼈대에서 설정)
        [SerializeField] private Transform parent;

        // 공격 판정을 진행하고 있는지 여부를 나타내는 변수
        private bool isInAttack = false;

        private void Awake()
        {
            // 보스 몬스터의 공격 스테이트 컴포넌트를 검색해서 공격 시점에 맞도록 이벤트에 등록
            GrenadierAttackState attackState = GetComponentInParent<GrenadierAttackState>();
            if (attackState != null)
            {
                // 근접 공격 판정 시작 이벤트에 등록
                attackState.SubscribeOnMeleeAttackCheckStart(OnAttackBegin);

                // 공격 종료 이벤트에 등록
                attackState.SubscribeOnAttackEnd(OnAttackEnd);
            }

            // 에디터에서 지정한 부모 트랜스폼의 자식 게임 오브젝트로 계층 설정
            Transform refTransform = transform;
            refTransform.SetParent(parent);
            refTransform.localPosition = Vector3.zero;
            refTransform.localRotation = Quaternion.identity;
        }

        // 공격력을 설정하는 함수
        public void SetAttack(float attack)
        {
            attackAmount = attack;
        }

        private void FixedUpdate()
        {
            // 공격 판정을 진행하지 않을 때는 충돌 확인을 진행하지 않고 종료
            if (!isInAttack)
            {
                return;
            }

            // 공격 판정을 진행하는 중이라면, 물리 엔진의 기능을 통해 충돌 확인
            // 충돌 지점에서 캡슐(Capsule) 형태로 충돌 확인 진행
            Collider[] colliders = Physics.OverlapCapsule(attackPoints[0].position, attackPoints[1].position, radius, attackTargetLayer);

            // 무기와 충돌한 다른 물체가 없으면 함수 종료
            if (colliders.Length == 0)
            {
                return;
            }

            // 충돌한 다른 충돌체에 대미지 전달 시도
            foreach (Collider collider in colliders)
            {
                // 충돌한 충돌체에서 대미지 전달을 위해 Damageable 컴포넌트를 검색한 후 대미지 전달
                Damageable damageable = collider.GetComponent<Damageable>();
                if (damageable != null)
                {
                    damageable.ReceiveDamage(attackAmount);
                }
            }
        }

        // 공격 판정을 시작할 때 호출할 함수
        public void OnAttackBegin()
        {
            isInAttack = true;
        }

        // 공격 판정을 종료할 때 호출할 함수
        public void OnAttackEnd()
        {
            isInAttack = false;
        }

#if UNITY_EDITOR
        // 이 컴포넌트가 추가된 게임 오브젝트를 선택했을 때만 기즈모를 그리는 함수
        private void OnDrawGizmos()
        {
            // 기즈모의 색상 설정
            Gizmos.color = Color.red;

            // attackPoints 배열을 돌면서 기즈모를 그린다.
            foreach (Transform point in attackPoints)
            {
                // 선 형태의 구를 기즈모로 그린다.
                Gizmos.DrawWireSphere(point.position, radius);
            }
        }
#endif
    }
}