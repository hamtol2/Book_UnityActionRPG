using UnityEngine;

namespace RPGGame
{
    // NPC의 기본(idle) 스테이트 스크립트
    public class NPCIdleState : NPCStateBase
    {
        protected override void Update()
        {
            base.Update();

            // 대화가 가능한 경우(플레이어와의 거리가 대화 가능 거리일 때)
            if (CanTalk())
            {
                // 대화 스테이트로 전환
                manager.SetState(NPCStateManager.State.Talk);
            }
        }
    }
}