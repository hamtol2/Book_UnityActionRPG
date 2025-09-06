using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPGGame
{
    // 수집한 아이템 정보를 나타내는 클래스.
    // 한 아이템을 여러 개 수집할 수 있기 때문에
    // 수집된 아이템 수를 파악하기 위해 별도의 클래스를 추가한다.
    [System.Serializable]
    public class ItemSlot
    {
        // 수집한 아이템 참조 변수
        public Item item = null;

        // 수집한 아이템 수
        public int count = 0;

        // 생성자
        public ItemSlot(Item item, int count)
        {
            this.item = item;
            this.count = count;
        }

        // 아이템 수를 추가할 때 사용하는 함수
        public void AddCount(int count)
        {
            this.count += count;
        }

        // 획득한 아이템을 사용할 때 호출하는 함수
        public void UseItem()
        {
            --count;
        }
    }

    // 플레이어가 수집한 아이템을 관리하는 인벤토리 관리자 스크립트
    [DefaultExecutionOrder(-50)]
    public class InventoryManager : MonoBehaviour
    {
        // 싱글턴을 위한 스태틱 인스턴스
        private static InventoryManager instance = null;

        // 싱글턴 접근 프로퍼티
        public static InventoryManager Instance { get { return instance; } }

        // 플레이어가 수집한 아이템 목록
        [SerializeField] private Dictionary<Item, ItemSlot> items = new Dictionary<Item, ItemSlot>();

        // 아이템 목록이 바뀌면 발행되는 이벤트
        [SerializeField] private UnityEvent OnItemListChanged;

        // 현재 인벤토리에 수집된 아이템 개수를 반환하는 프로퍼티
        public int ItemCount { get { return items.Count; } }

        private void Awake()
        {
            // 싱글턴 객체 설정되지 않았으면, 이 객체를 싱글턴 객체로 설정
            if (instance == null)
            {
                instance = this;
            }

            // 이미 다른 객체가 싱글턴 객체로 할당된 경우에는, 싱글턴 유지를 위해 이 게임 오브젝트를 삭제
            else
            {
                Destroy(gameObject);
            }
        }

        // Item을 키로 사용해서 ItemSlot을 반환해주는 함수
        public ItemSlot GetItemSlot(Item item)
        {
            // 아이템을 저장하는 딕셔너리에서 item을 키로 사용해 ItemSlot 검색 시도
            if (items.TryGetValue(item, out ItemSlot itemSlot))
            {
                // 검색에 성공했으면, 검색된 객체 반환
                return itemSlot;
            }

            // 검색 실패 시 null 반환
            return null;
        }

        // List<ItemSlot>를 반환해주는 함수
        // UI 등 다른 곳에서 필요한 Item 리스트 전체의 정보를 반환
        public List<ItemSlot> GetItems()
        {
            // 인벤토리 정보를 저장할 리스트 생성
            List<ItemSlot> itemSlots = new List<ItemSlot>();

            // 딕셔너리를 순회하면서 아이템을 리스트에 추가
            foreach (var item in items)
            {
                // 리스트에 아이템 추가
                itemSlots.Add(item.Value);
            }

            // 리스트 반환
            return itemSlots;
        }

        // Item를 items 딕셔너리에 추가할 때 사용하는 함수
        public void AddItem(Item item)
        {
            // 인벤토리에 아이템이 있는지 확인
            if (items.ContainsKey(item))
            {
                // 아이템을 이미 수집한 상태라면, 아이템의 수를 늘린다.
                if (items.TryGetValue(item, out ItemSlot itemSlot))
                {
                    itemSlot.AddCount(1);
                    return;
                }
            }

            // 인벤토리에 없는 새로운 아이템을 수집한 경우에는 새로 추가
            else
            {
                items.Add(item, new ItemSlot(item, 1));
            }
        }

        // 수집한 아이템이 이미 인벤토리에 있는지 알려주는 함수
        public bool HasItem(Item item)
        {
            // 딕셔너리에 키가 있는지 확인
            return items.ContainsKey(item);
        }

        // 아이템을 획득할 때 인벤토리에 추가하는 함수
        public void OnItemCollected(CollectableItem item)
        {
            // 인벤토리에 아이템 추가
            AddItem(item.Item);

            // 아이템 변경 이벤트 발행
            OnItemListChanged?.Invoke();
        }

        // 아이템을 사용했을 때 호출하는 함수
        public void OnItemUsed(Item item)
        {
            // 사용된 아이템이 인벤토리에 있는지 확인
            if (items.ContainsKey(item))
            {
                // 사용한 아이템 검색
                if (items.TryGetValue(item, out ItemSlot itemSlot))
                {
                    // 획득한 아이템의 수를 감소 처리
                    --itemSlot.count;

                    // 아이템을 모두 사용했으면 인벤토리에서 제거
                    if (itemSlot.count == 0)
                    {
                        items.Remove(item);
                    }
                }
            }

            // 아이템 목록이 변경됐음을 이벤트를 통해서 공지
            OnItemListChanged?.Invoke();
        }

        // 아이템 목록이 바뀔 때 발행되는 이벤트에 등록하는 함수
        public void SubscribeOnItemListChanged(UnityAction listener)
        {
            OnItemListChanged?.AddListener(listener);
        }
    }
}