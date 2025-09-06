using UnityEngine;

namespace RPGGame
{
    // 체력 아이템 스크립트
    // CreateAssetMenu 애트리뷰트를 사용해 생성 메뉴 추가
    [CreateAssetMenu(fileName = "New Health Item", menuName = "Inventory/Item/HealthItem")]
    public class HealthItem : Item
    {
        // 회복하는 체력의 양
        public float healthAmount;

        public void Awake()
        {
            // UI에서 보여줄 아이템의 이름 설정
            itemName = "체력";
        }

        // 체력 아이템을 사용할 때 플레이어의 체력을 회복시키는 함수
        public override void Use()
        {
            base.Use();

            // "Player" 태그로 게임 오브젝트 검색
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // Player 게임 오브젝트를 찾았으면,
            if (player != null)
            {
                // Player의 HPController 컴포넌트를 검색해 체력을 회복할 수 있도록 값 전달
                HPController hpController = player.transform.root.GetComponentInChildren<HPController>();
                if (hpController != null)
                {
                    // HPController 컴포넌트의 OnHealed 함수를 호출해 체력 회복 처리
                    hpController.OnHealed(healthAmount);
                }
            }
        }
    }
}