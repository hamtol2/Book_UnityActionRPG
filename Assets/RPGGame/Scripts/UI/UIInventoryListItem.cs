using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPGGame
{
    // 인벤토리 UI 창에 생성되는 아이템 UI 스크립트
    // IPointerClickHandler 인터페이스는 UI의 클릭 이벤트를 받을 때 사용할 수 있고,
    // OnPointerClick 함수를 구현해야 한다.
    public class UIInventoryListItem : MonoBehaviour, IPointerClickHandler
    {
        // 아이템 정보
        public Item item;

        // 아이템 이미지
        public Image image;

        // 아이템 이름 텍스트
        public TMPro.TextMeshProUGUI nameText;

        // 아이템 수 텍스트
        public TMPro.TextMeshProUGUI countText;

        // 아이템 정보를 설정하는 함수
        public void SetItem(Item item)
        {
            this.item = item;
        }

        // 아이템 이미지를 설정하는 함수
        public void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }

        // 아이템 이름을 설정하는 함수
        public void SetName(string name)
        {
            nameText.text = name;
        }

        // 아이템 수를 설정하는 함수
        public void SetCount(int count)
        {
            countText.text = $"{count}";
        }

        // 아이템 UI가 클릭됐을 때 실행될 함수
        public void OnPointerClick(PointerEventData eventData)
        {
            item.Use();
        }
    }
}