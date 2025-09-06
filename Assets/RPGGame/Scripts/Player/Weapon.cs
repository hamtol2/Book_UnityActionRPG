using UnityEngine;

namespace RPGGame
{
    // 캐릭터의 무기 스크립트
    public class Weapon : CollectableItem
    {
        // 공격력 데이터
        [SerializeField] private float attackAmount = 0f;

        // 공격 판정에 사용할 충돌 영역의 반지름
        [SerializeField] private float radius = 0.1f;

        // 무기와 몬스터가 충돌할 때 재생할 Hit 파티클
        [SerializeField] private ParticleSystem hitParticle;

        // 공격을 판정할 때 사용할 위치
        [SerializeField] private Transform[] attackPoints;

        // 공격 판정 대상 레이어
        [SerializeField] private LayerMask attackTargetLayer;

        // 공격 판정을 진행하고 있는지 여부를 나타내는 변수
        private bool isInAttack = false;

        protected override void Awake()
        {
            base.Awake();

            // 무기 아이템 정보로부터 공격력 값을 읽어와 attack 변수에 저장
            // 몬스터에 대미지를 전달할 때마다 공격력 값을 함께 전달해야 하는데
            // 그때마다 매번 item을 형변환해서 attack 값을 읽어오는 것이 불필요하기 때문에 미리 저장해두고 사용함
            WeaponItem weaponItem = item as WeaponItem;
            if (weaponItem != null)
            {
                attackAmount = weaponItem.attack;
            }
        }

        private void FixedUpdate()
        {
            // 공격 판정을 하지 않을 때는 충돌 확인을 진행하지 않고 종료
            if (!isInAttack)
            {
                return;
            }

            // 공격 판정을 진행하는 중이라면, 물리 엔진의 기능을 통해 충돌 확인
            // Physics.OverlapCapsule 함수를 사용해 임시로 캡슐 모양의 충돌체 형태로 충돌 확인
            Collider[] colliders = Physics.OverlapCapsule(attackPoints[0].position, attackPoints[1].position, radius, attackTargetLayer);

            // 무기와 충돌한 다른 물체가 없으면 함수 종료
            if (colliders.Length == 0)
            {
                return;
            }

            // 무기와 충돌한 다른 충돌체에 대미지 전달 시도
            foreach (Collider collider in colliders)
            {
                // 무기와 부딪힌 충돌체에서 대미지 전달을 위해 Damageable 컴포넌트를 검색한 후 대미지 전달
                // 테스트를 위해 임시로 로그 출력
                //Util.LogRed("무기와 충돌함");
                Damageable damageable = collider.GetComponent<Damageable>();
                if (damageable != null)
                {
                    // Damageable 대미지(attackAmount) 전달
                    damageable.ReceiveDamage(attackAmount);
                }

                // Hit 파티클 재생
                if (hitParticle != null)
                {
                    // 파티클의 위치를 충돌한 컴포넌트의 위치로 설정
                    hitParticle.transform.position = collider.transform.position;

                    // 옮긴 위치에서 파티클 재생
                    hitParticle.Play();
                }
            }
        }

        protected override void OnCollect(Collider other)
        {
            //base.OnCollect(other);

            // 이 아이템을 수집하지 않았고, 부딪힌 충돌체의 태그가 Player라면 무기 수집 처리
            if (!HasCollected && other.CompareTag("Player"))
            {
                // 무기 수집 처리를 위해 WeaponController 컴포넌트 검색
                WeaponController weaponController = other.GetComponentInChildren<WeaponController>();

                // WeaponController에서 무기 수집 진행
                if (weaponController != null)
                {
                    weaponController.AttachWeapon(this);
                }

                // 아이템이 수집됐다는 이벤트 발행
                OnItemCollected?.Invoke();
            }
        }

        // 무기를 수집할 때 실행하는 함수
        public void Attach(Transform parentTransform)
        {
            // 트랜스폼 설정 후 위치/회전 조정
            refTransform.SetParent(parentTransform);
            refTransform.localPosition = Vector3.zero;
            refTransform.localRotation = Quaternion.identity;

            HasCollected = true;
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
    }
}