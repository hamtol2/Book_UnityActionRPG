using UnityEngine;

namespace RPGGame
{
    // 게임 시작 씬의 UI 버튼 이벤트 및 씬 로드 기능을 제공하는 스크립트
    public class UIGameStart : MonoBehaviour
    {
        // 게임 로드 UI 참조 변수
        [SerializeField] private GameObject loadUI;

        // 게임 시작 버튼을 눌렀을 때 실행할 함수
        public void StartGame()
        {
            // 씬 로드 게임 오브젝트 활성화
            loadUI.SetActive(true);
        }

        // 게임 종료 버튼을 눌렀을 때 실행할 함수
        public void ExitGame()
        {
#if UNITY_EDITOR
            // 에디터 모드에서 실행할 때는 에디터의 플레이 모드 정지
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // 에디터 모드를 제외한 나머지에서는 엔진(게임) 종료.
            Application.Quit();
#endif
        }
    }
}