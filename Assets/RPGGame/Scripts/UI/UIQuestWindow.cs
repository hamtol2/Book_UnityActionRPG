using System.Collections.Generic;
using UnityEngine;

namespace RPGGame
{
    // 퀘스트 팝업 창의 기능을 제공하는 스크립트
    public class UIQuestWindow : MonoBehaviour
    {
        // 싱글턴 구현을 위한 인스턴스 변수
        private static UIQuestWindow instance;

        // 퀘스트 팝업 메인 창
        [SerializeField] private GameObject window;

        // 퀘스트 리스트 UI를 추가할 때 부모로 설정할 트랜스폼
        [SerializeField] private RectTransform contentTransform;

        // 퀘스트 팝업 창 스크롤 뷰에 추가할 퀘스트 리스트 UI 프리팹
        [SerializeField] private UIQuestListItem itemPrefab;

        // UI 아이템의 높이
        [SerializeField] private float itemHeight = 60f;

        // 생성된 퀘스트 리스트 UI
        [SerializeField] private List<UIQuestListItem> items = new List<UIQuestListItem>();

        // UI 퀘스트 창이 열려 있는지를 알려주는 프로퍼티
        public static bool IsOn { get { return instance.window.activeSelf; } }

        private void Awake()
        {
            // 싱글턴 인스턴스가 null인 경우, 인스턴스 설정
            if (instance == null)
            {
                // 이 컴포넌트를 싱글턴 객체로 설정
                instance = this;

                // 퀘스트 창 초기화
                Initialize();
            }

            // 이미 다른 객체가 설정돼 있다면, 싱글턴 유지를 위해 중복되는 게임 오브젝트 제거
            else
            {
                Destroy(gameObject);
            }
        }

        // 초기화
        private void Initialize()
        {
            // 퀘스트 시작 이벤트에 함수 등록
            QuestManager.Instance.SubscribeOnQuestStarted(OnQuestStarted);

            // 퀘스트 달성 이벤트에 함수 등록
            QuestManager.Instance.SubscribeOnQuestCompleted(OnQuestCompleted);

            // 퀘스트 달성 횟수 변경 이벤트에 함수 등록
            QuestManager.Instance.SubscribeOnQuestCompleteCountChanged(OnQuestCompleteCountChanged);

            // 퀘스트 목록 UI 생성
            for (int ix = 1; ix < DataManager.Instance.questData.quests.Count; ++ix)
            {
                QuestData.Quest quest = DataManager.Instance.questData.quests[ix];

                // 프리팹을 사용해 퀘스트 UI 생성
                UIQuestListItem newItem = Instantiate(itemPrefab, contentTransform);

                // 새로 추가한 아이템 초기 설정
                newItem.SetClosed();
                newItem.SetQuestTitle(quest.questTitle);
                newItem.SetQuestCount(0, quest.countToComplete);

                // 생성한 UI를 리스트에 추가
                items.Add(newItem);
            }

            // 퀘스트 목록 UI를 포함하는 Content의 크기 설정
            Vector2 size = contentTransform.sizeDelta;
            size.y = (DataManager.Instance.questData.quests.Count - 1) * itemHeight;
            contentTransform.sizeDelta = size;
        }

        // 퀘스트 팝업 창 열기 함수
        public static void Show()
        {
            instance.window.SetActive(true);
        }

        // 퀘스트 팝업 창 닫기 함수
        public static void Close()
        {
            instance.window.SetActive(false);
        }

        // 퀘스트를 시작할 때 발행되는 이벤트에 등록해 사용하는 함수
        private void OnQuestStarted(int questID)
        {
            items[questID - 1].SetProgress();
        }

        // 퀘스트를 달성했을 때 발행되는 이벤트에 등록해 사용하는 함수
        // 퀘스트 UI 목록에 완료 표시
        private void OnQuestCompleted(int questID)
        {
            items[questID - 1].SetCompleted();
        }

        // 퀘스트 달성 횟수가 업데이트될 때 발행되는 이벤트에 등록해 사용하는 함수
        private void OnQuestCompleteCountChanged(int questID, int completeCount)
        {
            items[questID - 1].SetQuestCount(completeCount);
        }
    }
}