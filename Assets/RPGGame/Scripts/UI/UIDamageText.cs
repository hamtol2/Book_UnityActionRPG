using System.Collections;
using UnityEngine;

namespace RPGGame
{
    // 대미지가 전달될 때 대미지 값을 텍스트 UI로 보여주는 스크립트
    public class UIDamageText : MonoBehaviour
    {
        // 대미지 텍스트
        [SerializeField] private TMPro.TextMeshProUGUI damageText;

        // 대미지 스케일 애니메이션을 위한 스케일 값
        [SerializeField] private float scaleMax = 1.5f;
        [SerializeField] private float scaleMin = 0.8f;

        // 스케일 애니메이션 재생 시간(단위: 초)
        [SerializeField] private float scaleAnimationTime = 0.5f;

        // 원래 폰트 크기
        private float originalFontSize;

        // 스케일 애니메이션을 시작한 시간으로부터 경과한 시간(단위: 초)
        private float elapsedTime = 0f;

        private void Awake()
        {
            // 원래 폰트 크기 저장
            originalFontSize = damageText.fontSize;
        }

        // 대미지를 받았을 때 대미지의 양을 전달받는 함수
        public void OnDamageRecived(float damage)
        {
            // 대미지 텍스트 UI에 대미지 값을 보여준다.
            damageText.text = $"-{damage}";

            // 텍스트 스케일 애니메이션 재생
            StartCoroutine(PlayScaleAnimation());
        }

        private IEnumerator PlayScaleAnimation()
        {
            // RectTransform에 초기 스케일 설정
            damageText.fontSize = originalFontSize * scaleMax;

            // 경과 시간 초기화
            elapsedTime = 0f;
            while (elapsedTime <= scaleAnimationTime)
            {
                // 한 프레임 대기
                yield return null;

                // 경과한 시간 계산
                elapsedTime += Time.deltaTime;

                // 적용할 스케일 값 계산
                float scale = Mathf.Lerp(scaleMax, scaleMin, elapsedTime / scaleAnimationTime);

                // 스케일 설정
                damageText.fontSize = originalFontSize * scale;
            }

            // 텍스트를 비운다.
            SetEmpty();
        }

        // 일정 시간 이후에 대미지 텍스트를 비활성화하기 위한 함수
        private void SetEmpty()
        {
            // 텍스트에 빈 문자열 설정
            damageText.text = string.Empty;

            // 원래 폰트 크기 값 설정
            damageText.fontSize = originalFontSize;
        }
    }
}