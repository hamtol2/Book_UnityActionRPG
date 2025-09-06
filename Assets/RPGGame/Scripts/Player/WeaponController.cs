using UnityEngine;

namespace RPGGame
{
    // 무기 관련 기능을 담당하는 스크립트
    // 무기 장착, 공격 애니메이션 재생 시 콤보에 맞는 애니메이션 효과 재생 등
    public class WeaponController : MonoBehaviour
    {
        // 무기가 장착돼야 하는 위치(모델링의 특정 뼈대 위치를 저장)
        [SerializeField] private Transform weaponHolder;

        // 장착된 무기 컴포넌트
        [SerializeField] private Weapon weapon;

        // 공격 모션 재생 시 함께 재생할 효과이펙트를 배열로 저장 (인덱스로 접근해서 공격 콤보에 맞는 효과이펙트를 선택해 재생함).
        [SerializeField] private PlayerAttackEffect[] attackComboEffects;

        // 공격 모션을 재생할 때 함께 재생할 사운드 플레이어.
        [SerializeField] private AudioPlayer swingSound;

        // 무기가 장착됐는지 확인하는 변수
        public bool IsWeaponAttached { get; private set; }

        // 플레이어가 무기를 획득할 때 무기를 장착하는 함수
        public void AttachWeapon(Weapon weapon)
        {
            // 무기가 이미 장착됐으면 함수 종료
            if (IsWeaponAttached)
            {
                return;
            }

            // 무기 장착
            weapon.Attach(weaponHolder);

            // 사운드 플레이어 설정.
            swingSound = GetComponentInChildren<AudioPlayer>();

            // 장착된 무기의 참조 설정
            this.weapon = weapon;

            // 무기 장착 설정
            IsWeaponAttached = true;

            // 무기에서 전달받아야 하는 공격 관련 이벤트 등록
            PlayerAttackState playerAttackState = transform.root.GetComponentInChildren<PlayerAttackState>();
            if (playerAttackState != null)
            {
                // 공격 판정 시작 이벤트에 함수 등록
                playerAttackState.SubscribeOnAttackBegin(weapon.OnAttackBegin);

                // 공격 판정 종료 이벤트에 함수 등록
                playerAttackState.SubscribeOnAttackCheckEnd(weapon.OnAttackEnd);
            }
        }

        // 콤보 공격 시 콤보 순서에 맞는 트레일 애니메이션 효과를 재생하는 함수.
        public void PlayAttackComboEffect(int comboIndex)
        {
            // 배열 인덱스 범위를 벗어날 때나는 경우에는 문제가 발생하지 않도록 함수 종료. (예외 처리).
            if (comboIndex < 0 || comboIndex >= attackComboEffects.Length)
            {
                return;
            }

            // 전달 받은 인덱스로 애니메이션 효과이펙트 재생.
            attackComboEffects[comboIndex].Activate();

            // 공격 사운드 재생.
            swingSound.Play();
        }
    }
}