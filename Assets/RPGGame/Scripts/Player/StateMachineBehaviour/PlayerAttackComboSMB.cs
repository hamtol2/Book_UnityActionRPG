using UnityEngine;

namespace RPGGame
{
    // AttackCombo 애니메이션 스테이트에 추가할 StateMachineBehaviour 스크립트.
    public class PlayerAttackComboSMB : StateMachineBehaviour
    {
        // 공격 모션 재생 시 함께 재생할 파티클 효과이펙트 배열 인덱스.
        // 이 인덱스 값은, 콤보 인덱스와 같음(ex: AttackCombo1 -> 0, AttackCombo2 -> 1, AttackCombo3 -> 2, AttackCombo4 -> 3).
        [SerializeField] private int effectIndex = -1;

        // 효과이펙트 재생을 위한 무기 관리자 참조 변수.
        private WeaponController weaponController;

        // 스테이트에 진입할 때 파티클 효과이펙트를 같이 재생하도록 처리.
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // effectIndex 값이 -1일 때는인 경우에는 잘못된 경우이므로기 때문에 문제가 발생하지 않도록 효과이펙트 재생하지 않고 함수 종료.
            if (effectIndex == -1)
            {
                return;
            }

            // 무기 관리자 컴포넌트 참조 변수 설정.
            if (weaponController == null)
            {
                weaponController = animator.GetComponentInChildren<WeaponController>();
            }

            // 효과이펙트 재생.
            weaponController.PlayAttackComboEffect(effectIndex);
        }
    }
}