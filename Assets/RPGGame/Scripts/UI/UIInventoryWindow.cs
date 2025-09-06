using System.Collections.Generic;
using UnityEngine;

namespace RPGGame
{
    // UI로 인벤토리 목록을 보여주는 창 스크립트
    public class UIInventoryWindow : MonoBehaviour
    {
        // 싱글턴 형태로 기능을 제공하기 위한 인스턴스 변수
        private static UIInventoryWindow instance;

        // 인벤토리 창 게임 오브젝트
        [SerializeField] private GameObject window;

        // UI 아이템 목록을 생성할 때 부모로 지정할 트랜스폼
        [SerializeField] private RectTransform contentTransform;

        // UI 아이템을 생성할 때 사용할 프리팹
        [SerializeField] private UIInventoryListItem itemPrefab;

        // 아이템 생성 및 정렬 시 사용할 아이템 너비
        [SerializeField] private float itemWidth;

        // 아이템 생성 및 정렬 시 사용할 아이템 높이
        [SerializeField] private float itemHeight;

        // 아이템을 저장할 인벤토리 컨테이너(검색을 쉽게 하기 위해 딕셔너리를 사용)
        [SerializeField] private List<UIInventoryListItem> items = new List<UIInventoryListItem>();

        // UI 인벤토리 창이 열려있는 지를 알려주는 프로퍼티
        public static bool IsOn { get { return instance.window.activeSelf; } }

        private void Awake()
        {
            // 싱글턴 객체 설정
            if (instance == null)
            {
                instance = this;
            }

            // instance가 이미 설정됐으면, 이미 객체가 존재하는 것이기 때문에 중복되는 객체는 삭제
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            // 인벤토리 관리자의 아이템 목록이 변경될 때 발생하는 이벤트에 함수 등록
            InventoryManager.Instance.SubscribeOnItemListChanged(OnItemListChanged);
        }

        // 아이템 목록에 변화가 생겼을 때 실행되는 함수
        public static void OnItemListChanged()
        {
            // 인벤토리 관리자가 가진 아이템의 수와 UI 인벤토리 관리자가 가진 아이템의 수가 같으면,
            // 각 아이템의 수만 확인
            if (instance.items.Count == InventoryManager.Instance.ItemCount)
            {
                // 아이템을 루프로 순회하면서,
                foreach (var item in instance.items)
                {
                    // 아이템 수를 확인해 설정
                    ItemSlot itemSlot = InventoryManager.Instance.GetItemSlot(item.item);
                    item.SetCount(itemSlot.count);
                }

                // 함수 종료
                return;
            }

            // 인벤토리 내의 아이템 수가 다르면, 일단 모두 제거(리셋을 위해)
            foreach (var item in instance.items)
            {
                Destroy(item.gameObject);
            }

            // 아이템 목록도 리셋
            instance.items.Clear();

            // 프리팹을 통해 UI 아이템을 재생성

            // 가로에 배치할 수 있는 최대 아이템 개수(4개)
            const int maxXCount = 4;

            // 전체 아이템 정보를 가져온다.
            List<ItemSlot> itemList = InventoryManager.Instance.GetItems();

            // 루프를 순회하면서 인벤토리 아이템 게임 오브젝트를 생성하고 필요한 정보 설정
            foreach (ItemSlot itemSlot in itemList)
            {
                // UI 아이템 생성하며, 생성 시 contentTransform을 부모 트랜스폼으로 지정
                UIInventoryListItem newItem = Instantiate(instance.itemPrefab, instance.contentTransform);

                // 아이템에 필요한 정보 설정
                newItem.SetItem(itemSlot.item);
                newItem.SetSprite(itemSlot.item.sprite);
                newItem.SetName(itemSlot.item.itemName);
                newItem.SetCount(itemSlot.count);

                // UI 인벤토리에 새로 생성한 아이템 추가
                instance.items.Add(newItem);
            }

            // Content 트랜스폼의 높이 설정을 위해 현재 가로/세로 크기를 가져온다.
            Vector2 contentSize = instance.contentTransform.sizeDelta;

            // 아이템 배치 라인(줄) 수 계산
            float lineCount = itemList.Count / maxXCount + 1;

            // 라인 수에 따른 Content 트랜스폼의 높이 계산
            contentSize.y = lineCount * (instance.itemHeight) + (lineCount - 1) * 10f + 20f;

            // Content 트랜스폼에 설정
            instance.contentTransform.sizeDelta = contentSize;
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
    }
}