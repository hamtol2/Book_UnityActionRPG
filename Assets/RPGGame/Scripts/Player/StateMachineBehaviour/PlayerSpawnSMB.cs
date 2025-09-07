using UnityEngine;

namespace RPGGame
{
    // 플레이어가 등장할 때 재생되는 애니메이션 스테이트에 추가되는 스크립트.
    public class PlayerSpawnSMB : StateMachineBehaviour
    {
        // 애니메이션 재생이 완료되고 Idle 스테이트로 전환할 때 플레이어의 로직도 Idle 스테이트로 전환
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            // PlayerStateManager를 통해 스테이트 전환
            PlayerStateManager stateManager = animator.GetComponent<PlayerStateManager>();
            if (stateManager != null)
            {
                stateManager.SetState(PlayerStateManager.State.Idle);
            }

            // 게임 매니저의 GameStart 함수 호출
            // 게임 시작 안내 멘트를 다이얼로그를 통해 표시
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.GameStart();
            }
        }
    }
}