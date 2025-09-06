using UnityEngine;

namespace RPGGame
{
    // 방어구 아이템 스크립트
    [CreateAssetMenu(fileName = "New Shield Item", menuName = "Inventory/Item/ShieldItem")]
    public class ShieldItem : Item
    {
        // 방어구 아이템이 갖는 방어력
        public float defense;

        public void Awake()
        {
            itemName = "방어구";
        }

        // 방어력을 제공하는 아이템을 사용할 때 호출되는 함수
        public override void Use()
        {
            base.Use();

            // "Player" 태그로 게임 오브젝트 검색
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // Player 게임 오브젝트를 찾았으면,
            if (player != null)
            {
                // Player의 HPController 컴포넌트를 검색해 
                Damageable damageble = player.transform.root.GetComponentInChildren<Damageable>();
                if (damageble != null)
                {
                    // 방어력을 전달
                    damageble.SetDefense(defense);
                }
            }
        }
    }
}