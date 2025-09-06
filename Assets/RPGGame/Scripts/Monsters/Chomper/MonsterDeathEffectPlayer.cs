using UnityEngine;

namespace RPGGame
{
    // 몬스터가 죽을 때 시각적 효과를 재생하는 기능을 제공하는 스크립트
    public class MonsterDeathEffectPlayer : MonoBehaviour
    {
        // 몬스터의 셰이더 파라미터 이름
        private readonly string cutoffParameterName = "_Cutoff";

        // 시각적 효과를 재생하기 전에 약간 기다리기 위한 지연 시간(단위: 초)
        [SerializeField] private float startTime = 1.2f;

        // 시각적 효과의 재생 시간(단위: 초)
        [SerializeField] private float playTime = 2.0f;

        // 몬스터 모델에서 사용하는 Renderer 컴포넌트들을 저장하는 배열 변수
        private Renderer[] renderers;

        // 죽음 효과 재생 중, 경과 시간을 계산하기 위한 변수(단위: 초)
        private float elapsedTime = 0f;

        // 효과가 재생 중인지, 아닌 지를 설정하는 변수
        private bool isPlaying = false;

        // 셰이더 파라미터를 사용하기 위한 변수
        private MaterialPropertyBlock propertyBlock;

        private void OnEnable()
        {
            // 모델링에서 사용하는 Renderer 컴포넌트를 모두 검색해 배열에 저장
            renderers = transform.root.GetComponentsInChildren<Renderer>();

            // 파라미터 블록 변수 생성
            propertyBlock = new MaterialPropertyBlock();
        }

        private void Update()
        {
            // 시각 효과를 재생하지 않을 때는 함수 종료
            if (!isPlaying)
            {
                return;
            }

            // Renderer 컴포넌트 배열을 순회하면서 머티리얼 파라미터에 값 설정
            foreach (Renderer renderer in renderers)
            {
                // Renderer에서 머티리얼 프로퍼티 블록의 현재 값 가져오기
                renderer.GetPropertyBlock(propertyBlock);

                // 머티리얼 프로퍼티 블록의 _Cutoff 파라미터에 값을 설정
                // _Cutoff 파라미터는 Dissolve(디졸브) 효과를 제어할 수 있는데,
                // 0일 때 모델링 전체가 보이고, 1일 때 완전히 사라진다.
                propertyBlock.SetFloat(cutoffParameterName, elapsedTime / playTime);

                // _Cutoff 파라미터를 설정한 머티리얼 프로퍼티 블록을 다시 Renderer에 적용
                renderer.SetPropertyBlock(propertyBlock);
            }

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            // 재생 시간이 모두 지났는지 확인
            if (elapsedTime > playTime)
            {
                // 시각 효과 애니메이션 중단
                isPlaying = false;

                // 몬스터 게임 오브젝트 제거
                Destroy(transform.root.gameObject);
            }
        }

        // 몬스터 죽음 시각 효과를 재생할 때 사용하는 함수
        public void Play()
        {
            // startTime에 설정된 시간만큼 기다렸다가 PlayDeathEffect 함수 실행
            Invoke("PlayDeathEffect", startTime);
        }

        // 몬스터 죽음 시각 효과를 재생하는 함수
        private void PlayDeathEffect()
        {
            // 재생 경과 시간 초기화
            elapsedTime = 0f;

            // 애니메이션 재생 설정
            isPlaying = true;
        }
    }
}