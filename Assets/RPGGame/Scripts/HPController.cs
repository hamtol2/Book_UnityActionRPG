using UnityEngine;
using UnityEngine.Events;

namespace RPGGame
{
    // 체력을 관리하는 스크립트
    public class HPController : MonoBehaviour
    {
        // 현재 체력
        [SerializeField] private float currentHP = 0f;

        // 최대 체력
        [SerializeField] private float maxHP = 0f;

        // 방어력
        [SerializeField] private float defense = 0f;

        // 대미지를 받았을 때 발행되는 이벤트
        [SerializeField] private UnityEvent<float, float> OnHPChanged;

        // 죽었을 때 발행되는 이벤트
        [SerializeField] private UnityEvent OnDead;

        // 최대 체력을 설정하는 함수
        public void SetMaxHP(float maxHP)
        {
            // 체력 데이터 업데이트
            this.maxHP = maxHP;
            currentHP = maxHP;

            // 체력 변경 이벤트 발행
            OnHPChanged?.Invoke(currentHP, maxHP);
        }

        // 방어력을 설정하는 함수
        public void SetDefense(float defense)
        {
            this.defense = defense;
        }

        // Health 아이템을 획득해 HP가 회복될 때 실행될 함수
        public virtual void OnHealed(float hpAmount)
        {
            // 전달받은 양만큼 체력 회복
            currentHP += hpAmount;

            // 회복한 양이 최대 체력을 넘지 않도록 예외 처리
            currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

            // 체력 회복 후 변경된 HP 이벤트 발행
            OnHPChanged?.Invoke(currentHP, maxHP);
        }

        // 대미지를 입었을 때 실행되는 함수
        // Damageable 컴포넌트에서 발행되는 이벤트에 등록해서 사용
        public virtual void OnDamaged(float damage)
        {
            // 대미지 처리
            float finalDamage = Mathf.Max(0f, damage - defense);
            currentHP -= finalDamage;

            // 대미지 처리 후 현재 체력 값이 0이하로 내려가지 않도록 값 처리
            currentHP = Mathf.Max(0f, currentHP);

            // 대미지 처리 후 현재 체력, 최대 체력을 전달하며 이벤트 발행
            OnHPChanged?.Invoke(currentHP, maxHP);

            // 체력이 모두 소진됐으면 죽었다는 이벤트 발행
            if (currentHP == 0f)
            {
                OnDead?.Invoke();
            }
        }

        // 한 번에 죽는 경우에 실행되는 함수
        // 데드존(Deadzone)에 들어갔을 때 한 번에 죽음
        public virtual void Die()
        {
            // 현재 체력을 0으로 설정.
            currentHP = 0f;

            // 대미지 처리 후 체력 변경 이벤트 발행
            OnHPChanged?.Invoke(currentHP, maxHP);

            // 죽음 이벤트 발행
            OnDead?.Invoke();
        }

        // OnDead 이벤트에 구독할 때 사용하는 함수
        public void SubscribeOnDead(UnityAction listener)
        {
            OnDead?.AddListener(listener);
        }
    }
}