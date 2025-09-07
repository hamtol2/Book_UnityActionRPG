using UnityEngine;

namespace RPGGame
{
    // 보스 몬스터 애니메이션의 Dead 스테이트에 추가될 스크립트
    // 보스 몬스터의 Dead 애니메이션 종료 시점에 게임 클리어를 알리도록 하기 위해 사용
    public class GrenadierDeadStateSMB : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            // 게임 관리자 스크립트를 검색
            GameManager gameManager = FindFirstObjectByType<GameManager>();

            // 검색이 잘 됐으면, 게임 클리어 함수 호출
            if (gameManager != null)
            {
                // 게임 클리어를 호출할 때 보스 몬스터의 죽음(Dead) 애니메이션의 재생 시간을 전달해서
                // 애니메이션 시간만큼 대기한 후에 게임 클리어 메시지가 나타나도록 함수 호출
                gameManager.GameClear(stateInfo.length + 1f);
            }
        }
    }
}