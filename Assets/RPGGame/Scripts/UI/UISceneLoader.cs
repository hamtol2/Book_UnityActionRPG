using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPGGame
{
    // 메인 게임 씬을 로드하는 스크립트
    public class UISceneLoader : MonoBehaviour
    {
        // 메인 씬 이름
        [SerializeField] private string mainSceneName;

        // 로드 진행 상태를 보여줄 프로그레스 바 이미지 참조 변수
        [SerializeField] private Image progressBar;

        // 씬을 로드할 때 대기할 시간
        [SerializeField] private float loadingTime = 4f;

        private void OnEnable()
        {
            // 씬 로드 코루틴 함수 실행
            StartCoroutine(LoadScene());
        }

        // 씬을 로드하는 함수
        private IEnumerator LoadScene()
        {
            // 비동기 방식으로 씬 로드
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(mainSceneName);

            // 씬이 모두 로드되더라도 바로 이동하지 않도록 설정
            asyncOperation.allowSceneActivation = false;

            // 씬 로드가 완료될 때까지 대기
            yield return asyncOperation.isDone;

            // 경과 시간 계산을 위한 변수 선언
            float elapsedTime = 0f;

            // 가짜 진행률이지만 씬이 로드된 것처럼 표시
            while (true)
            {
                // 1프레임 대기
                yield return null;

                // 경과 시간 업데이트
                elapsedTime += Time.deltaTime;

                // 진행률 계산
                float progress = elapsedTime / loadingTime;

                // 진행률 표시
                progressBar.fillAmount = progress;

                // 99퍼센트 이상 진행되면, 완료로 표시하고 씬 이동
                if (progress >= 0.99f)
                {
                    // 프로프레스 바 진행률 최대로 표시
                    progressBar.fillAmount = 1f;

                    // 루프 해제
                    break;
                }
            }

            // 0.5초 정도 더 대기
            yield return new WaitForSeconds(0.5f);

            // 씬 이동 허용
            asyncOperation.allowSceneActivation = true;
        }
    }
}