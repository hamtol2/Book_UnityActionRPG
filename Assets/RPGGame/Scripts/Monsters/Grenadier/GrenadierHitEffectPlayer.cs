using UnityEngine;

namespace RPGGame
{
    // 보스 몬스터의 피격(Hit) 효과 관련 기능을 관리하는 스크립트
    public class GrenadierHitEffectPlayer : MonoBehaviour
    {
        // Hit 파티클 시스템 컴포넌트 참조 변수
        [SerializeField] private ParticleSystem hitParticle;

        // 보스 몬스터의 메인 렌더러 컴포넌트 참조 변수
        [SerializeField] private SkinnedMeshRenderer mainRenderer;

        // 피격 효과 지속 시간(단위: 초).
        [SerializeField] private float hitEffectTime = 1f;

        // 피격 효과 시 적용할 머티리얼 색상
        [SerializeField] private Color hitEffectColor = Color.red;

        // 색상 효과 파라미터 전달을 위한 머티리얼
        private Material colorMaterial;

        // 원래 색상 저장을 위한 변수
        private Color originalColor;

        private void Awake()
        {
            // 색상 파라미터 적용을 위한 머티리얼 설정
            if (colorMaterial == null)
            {
                // 두 번째 머티리얼 설정
                colorMaterial = mainRenderer.materials[1];

                // 원래 색상 저장
                originalColor = colorMaterial.GetColor("_Color2");
            }
        }

        // 보스 몬스터가 맞을 때 실행할 함수
        public void Hit()
        {
            // 파티클 효과 재생
            hitParticle.Play();

            // 피격 색상 적용
            colorMaterial.SetColor("_Color2", hitEffectColor);

            // 원래 색상으로 되돌리기 위해 함수 실행 예약
            Invoke("ReturnOriginalColor", hitEffectTime);
        }

        // 원래 색상으로 되돌릴 때 사용하는 함수
        private void ReturnOriginalColor()
        {
            colorMaterial.SetColor("_Color2", originalColor);
        }
    }
}