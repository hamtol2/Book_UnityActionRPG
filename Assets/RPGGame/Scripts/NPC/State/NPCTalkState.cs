using UnityEngine;

namespace RPGGame
{
    // NPC가 대화 상태일 때를 나타내는 스크립트
    public class NPCTalkState : NPCStateBase
    {
        // 퀘스트 관리자 참조 변수
        private QuestManager questManager;

        protected override void OnEnable()
        {
            base.OnEnable();

            // 퀘스트 관리자 참조 변수 설정
            if (questManager == null)
            {
                questManager = QuestManager.Instance;
            }

            // NPC ID에 따른 NPC 상태 확인
            switch (questManager.CheckNPCState(manager.NPCID))
            {
                // 퀘스트를 시작하는 경우의 처리
                case QuestManager.QuestState.Start:
                    {
                        // 다음 퀘스트로 업데이트
                        questManager.MoveToNextQuest();

                        // 퀘스트 내용을 다이얼로그에 표시
                        Dialogue.ShowDialogueText(questManager.CurrentQuest.questBeginText);

                        // 퀘스트 상태를 진행으로 변경
                        questManager.SetState(QuestManager.QuestState.Processing);

                    }
                    break;

                // 퀘스트를 진행 중인 경우의 처리
                case QuestManager.QuestState.Processing:
                    {
                        // 퀘스트 데이터에서 진행 중일 때의 NPC 대사를 가져와 다이얼로그에 표시
                        Dialogue.ShowDialogueText(questManager.CurrentQuest.questProgressText);

                    }
                    break;

                // 위의 두 상태가 아닌 경우(NPC 담당 퀘스트가 아니거나, 모든 퀘스트를 완료했을 때)
                default:
                    {
                        // 일반 대화에 사용되는 대사를 가져와 다이얼로그에 표시
                        Dialogue.ShowDialogueText(questManager.CurrentQuest.smallTalk);
                    }
                    break;
            }
        }

        protected override void Update()
        {
            base.Update();

            // 대화가 가능하지 않은 경우에는 기본 스테이트로 전환
            if (!CanTalk())
            {
                // 3초 후에 다이얼로그 닫기
                Dialogue.CloseDialogueAfterTime(3f);

                // 대기 스테이트로 전환
                manager.SetState(NPCStateManager.State.Idle);
            }

            // 플레이어를 바라보도록 회전 설정
            // 바라볼 방향 벡터 계산(플레이어 캐릭터를 향하는 방향 계산)
            Vector3 direction = manager.PlayerTransform.position - refTransform.position;
            direction.y = 0f;

            // 계산한 방향 값을 사용해 회전 계산 및 설정
            refTransform.rotation = Quaternion.LookRotation(direction);
        }
    }
}