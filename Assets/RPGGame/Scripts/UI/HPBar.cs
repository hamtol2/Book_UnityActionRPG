using UnityEngine;
using UnityEngine.UI;

namespace RPGGame
{
    // 체력 상태를 보여주는 UI 스크립트
    public class HPBar : MonoBehaviour
    {
        // 현재 남은 체력을 퍼센티지로 보여줄 때 사용할 이미지 컴포넌트 참조 변수
        [SerializeField] private Image hpBar;

        // 현재 남은 체력을 텍스트로 보여줄 때 사용하는 텍스트 컴포넌트 참조 변수
        [SerializeField] private TMPro.TextMeshProUGUI hpGaugeText;

        // 체력이 변경될 때 발생하는 OnHPChanged 이벤트와 연결해 실행할 함수
        public void OnDamageReceived(float currentHP, float maxHP)
        {
            // 이미지 참조 변수가 설정되면 이미지 업데이트
            if (hpBar != null)
            {
                // 전달받은 파라미터를 사용해 
                // 최대 체력 대비 현재 남은 체력을 퍼센티지로 변환해 설정.
                hpBar.fillAmount = currentHP / maxHP;
            }

            // 텍스트 참조 변수가 설정되면, 텍스트 정보 업데이트
            if (hpGaugeText != null)
            {
                // 전달받은 파라미터를 사용해 "현재 체력"/"최대 체력"
                // 형태로 문자열을 만든 후 텍스트 컴포넌트에 설정
                hpGaugeText.text = $"{currentHP}/{maxHP}";
            }
        }
    }
}