using UnityEngine;

namespace RPGGame
{
    // NPC의 애니메이션을 제어하는 스크립트
    public class NPCAnimationController : MonoBehaviour
    {
        // Animator 컴포넌트 참조 변수
        [SerializeField] private Animator refAnimator;

        private void OnEnable()
        {
            // Animator 컴포넌트 설정
            refAnimator = transform.parent.GetComponentInChildren<Animator>();

            // NPC 스테이트가 전환될 때 발행되는 이벤트에 구독
            var stateManager = transform.parent.GetComponentInChildren<NPCStateManager>();
            if (stateManager)
            {
                stateManager.SubscribeOnStateChanged(OnStateChanged);
            }
        }

        // NPC의 스테이트가 변경될 때 실행되는 함수
        // Animator에 State 값을 int로 변환해 전달
        private void OnStateChanged(NPCStateManager.State state)
        {
            refAnimator.SetInteger("State", (int)state);
        }
    }
}