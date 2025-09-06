using UnityEngine;

namespace RPGGame
{
    // 플레이어가 수집할 수 있는 아이템의 데이터를 제공하는 클래스
    // 모든 아이템 데이터 클래스의 기반(부모) 클래스
    public abstract class Item : ScriptableObject
    {
        // 인벤토리에 보여줄 아이템의 이름(문자열)
        public string itemName;

        // 인벤토리 UI에 보여줄 2D 스프라이트
        public Sprite sprite;

        // 아이템을 수집했을 때 다이얼로그 화면에 보여줄 메시지(문자열)
        // TextArea 애트리뷰트를 사용하면, 인스펙터에서 문자열을 작성할 때 더 큰 공간을 지정할 수 있다.
        [TextArea(2, 15)]
        public string messageWhenCollected;

        // 아이템을 사용했을 때 다이얼로그 화면에 보여줄 메시지(문자열)
        [TextArea(2, 15)]
        public string messageWhenUsed;

        // 아이템을 사용할 때 필요한 기능을 제공하는 함수
        // 자식 클래스에서 확장이 가능하도록 virtual로 선언
        public virtual void Use()
        {
        }
    }
}