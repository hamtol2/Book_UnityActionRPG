using UnityEngine;
using UnityEngine.UI;

namespace RPGGame
{
    // 퀘스트 UI 창에서 퀘스트의 목록를 제어하는 스크립트
    public class UIQuestListItem : MonoBehaviour
    {
        // 진행/완료를 표시하는 이미지 컴포넌트
        [SerializeField] private Image questStatusImage;

        // 닫힘을 표시할 때 이미지 컴포넌트에 지정할 색상 값
        [SerializeField] private Color closeColor;

        // 진행을 표시할 때 이미지 컴포넌트에 지정할 색상 값
        [SerializeField] private Color progressColor;

        // 완료를 표시할 때 이미지 컴포넌트에 지정할 색상 값
        [SerializeField] private Color completeColor;

        // 진행/완료를 표시하는 텍스트 컴포넌트
        [SerializeField] private TMPro.TextMeshProUGUI questStatusText;

        // 닫힘/진행/완료를 표시하기 위한 텍스트 상수
        private readonly string closeText = "닫힘";
        private readonly string progressText = "진행";
        private readonly string completeText = "완료";

        // 퀘스트 타이틀을 보여주는 텍스트 컴포넌트
        [SerializeField] private TMPro.TextMeshProUGUI questTitleText;

        // 퀘스트 회수를 보여주는 텍스트 컴포넌트
        [SerializeField] private TMPro.TextMeshProUGUI questCountText;

        // 퀘스트를 완료하기 위해 필요한 달성 횟수
        private int completeCount = 0;

        // 퀘스트를 아직 열지 않은 상태로 설정하는 함수
        public void SetClosed()
        {
            questStatusImage.color = closeColor;
            questStatusText.text = closeText;
        }

        // 퀘스트 진행을 설정하는 함수
        public void SetProgress()
        {
            questStatusImage.color = progressColor;
            questStatusText.text = progressText;
        }

        // 퀘스트 완료를 설정하는 함수
        public void SetCompleted()
        {
            questStatusImage.color = completeColor;
            questStatusText.text = completeText;

            SetQuestCount(completeCount);
        }

        // 퀘스트 제목 설정 함수
        public void SetQuestTitle(string questTitle)
        {
            questTitleText.text = questTitle;
        }

        // 현재까지 완료한 횟수만 전달해 퀘스트 카운트 텍스트를 업데이트하는 함수
        // 처음 생성할 때 저장한 완료 횟수와 현재까지 완료한 횟수를 사용해 텍스트 구성
        public void SetQuestCount(int currentCompleteCount)
        {
            questCountText.text = $"{currentCompleteCount}/{completeCount}";
        }

        // 퀘스트 완료 텍스트 업데이트 함수
        public void SetQuestCount(int currentCompleteCount, int countsToCompleteQuest)
        {
            completeCount = countsToCompleteQuest;
            questCountText.text = $"{currentCompleteCount}/{countsToCompleteQuest}";
        }
    }
}