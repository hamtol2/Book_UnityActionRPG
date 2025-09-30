using UnityEngine;
using UnityEngine.Events;

namespace RPGGame
{
    // 대미지 처리를 담당하는 스크립트
    public class Damageable : MonoBehaviour
    {
        // 대미지를 입었을 때 발생하는 이벤트
        [SerializeField] private UnityEvent<float> OnDamageReceived;

        // 대미지를 받았을 때 중복으로 대미지를 계속해서 받지 않도록 하는 변수(무적 모드)
        [SerializeField] private bool isInVulnerable = false;

        // 대미지를 중복으로 입지 않도록 유지하는 시간(단위: 초).
        [SerializeField] private float invulnerableTime = 0.2f;

        // 무적 모드의 지속 시간 계산을 위한 변수
        [SerializeField] private float time = 0f;

        // 아이템으로 인한 방어력(플레이어에서 주로 사용)
        [SerializeField] private float defense = 0f;

        private void Update()
        {
            // 대미지를 입지 않는 무적 모드일 때 무적 모드 해제까지의 시간 계산
            if (isInVulnerable && Time.time > time + invulnerableTime)
            {
                // 무적 모드 해제.
                isInVulnerable = false;
            }
        }

        // 방어력을 설정하는 함수
        public void SetDefense(float defense)
        {
            this.defense = defense;
        }

        // 대미지를 입었을 때 호출하는 함수
        // damageAmount: 전달한 대미지의 양
        public void ReceiveDamage(float damageAmount)
        {
            // 무적 상태라면 대미지를 처리하지 않고 함수 종료
            if (isInVulnerable)
            {
                return;
            }

            // 무적 상태 On
            isInVulnerable = true;

            // 무적 상태 시작 시간 저장
            time = Time.time;

            // 최종 대미지 계산: 대미지 - 방어력
            float finalDamage = damageAmount - defense;

            // 전달한 대미지의 양에서 방어력 값을 뺀 후, 대미지 값이 음수가 되지 않도록 처리
            finalDamage = Mathf.Max(0f, finalDamage);

            // 대미지를 입었다는 이벤트 발행
            OnDamageReceived?.Invoke(finalDamage);

            // 대미지를 받았다는 메시지 출력(테스트 용도)
            Util.Log($"ReceiveDamage:{transform.root.name}, damage:{damageAmount}");
        }
    }
}