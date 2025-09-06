using UnityEngine;

namespace RPGGame
{
    // 무기 아이템 스크립트.
    [CreateAssetMenu(fileName = "New Weapon Item", menuName = "Inventory/Item/WeaponItem")]
    public class WeaponItem : Item
    {
        // 이 무기 아이템이 가지는 공격력.
        public float attack;

        public void Awake()
        {
            itemName = "무기";
        }
    }
}