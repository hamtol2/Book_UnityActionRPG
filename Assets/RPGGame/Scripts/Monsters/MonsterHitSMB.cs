using UnityEngine;

namespace RPGGame
{
    // 몬스터 애니메이터 컨트롤러에서 Hit 스테이트에 추가해 사용하는 스크립트
    public class MonsterHitSMB : StateMachineBehaviour
    {
        // 스테이트에 진입했을 때 실행되는 유니티 이벤트 함수
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            // Hit 트리거 리셋
            animator.ResetTrigger("Hit");
        }
    }
}