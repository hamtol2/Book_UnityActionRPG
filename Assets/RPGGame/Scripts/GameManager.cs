using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGGame
{
    // 게임 관리자 스크립트
    public class GameManager : MonoBehaviour
    {
        // 게임 재시작 / 게임 종료 기능을 제공하는 메뉴 게임 오브젝트
        [SerializeField] private GameObject gameMenu;

        // 게임 메뉴를 보여주기까지 대기할 시간(단위: 초)
        [SerializeField] private float gameMenuOpenDelay = 5f;

        // 게임 시작 코멘트 - 게임이 시작될 때 다이얼로그를 통해 보여줄 메시지
        [SerializeField, TextArea(5, 5)] private string gameStartComment;

        // 게임 클리어 코멘트 - 모든 퀘스트를 완료하고 게임을 클리어했을 때 다이얼로그를 통해 보여줄 메시지
        [SerializeField, TextArea(5, 5)] private string gameClearComment;

        public void GameStart()
        {
            // 게임 시작이 되면 약 1초 후에 게임 안내 코멘트 표시
            Invoke("ShowGameStartDialogue", 1f);
        }

        // 게임을 시작할 때 안내 코멘트를 보여주는 함수
        private void ShowGameStartDialogue()
        {
            // 게임이 처음 시작될 때 안내 문구를 다이얼로그를 통해 표시
            //Dialogue.SetTempDialogueText("반가워요 엘렌!\n몬스터가 마을을 침략했어요!\n무기를 획득해 우리 마을을 구해주세요!!", 10f);
            Dialogue.ShowDialogueTextTemporarily(gameStartComment, 10f);
        }

        // 게임을 클리어했을 때 사용하는 함수
        public void GameClear(float time = 0f)
        {
            // 게임 클리어 다이얼로그를 보여주기 전까지 대기할 시간 값 계산
            // time 파라미터에 전달된 값이 있으면 그 값을 사용하고, 전달된 값이 없는 경우 고정값 사용
            float delay = time == 0f ? 2f : time + 2f;

            // 게임 클리어 시 필요한 일을 처리하는 함수 호출
            StartCoroutine(ProcessGameClear(delay));
        }

        // 게임을 클리어했을 때 대기 시간을 적용해 여러 작업을 순차적으로 처리하는 코루틴 함수
        private IEnumerator ProcessGameClear(float delay)
        {
            // 전달된 delay 값만큼 대기
            yield return new WaitForSeconds(delay);

            // 게임을 클리어 메시지를 다이얼로그를 통해 표시
            Dialogue.ShowDialogueTextTemporarily(gameClearComment, 10f);

            // 전투 배경 음악에서 일반 배경 음악으로 변경
            BackgroundMusicPlayer musicPlayer = FindFirstObjectByType<BackgroundMusicPlayer>();
            if (musicPlayer != null)
            {
                musicPlayer.PlayNormalMusic();
            }

            // 약 5초를 더 대기한 뒤
            yield return new WaitForSeconds(5f);

            // 게임 메뉴 보여주기(재시작/종료 메뉴)
            GameOver(delay + 5f);
        }

        // 게임 메뉴를 보여주는 함수
        public void GameOver(float time = 0f)
        {
            // ActivateGameMenu 함수를 호출하기 전까지 대기할 시간 값 설정
            // 파라미터로 time 값이 전달됐을 때는 해당 값을 사용하고, 전달되지 않았을 때는 gameMenuOpenDelay 값을 사용
            float delay = time == 0f ? gameMenuOpenDelay : time;

            // 일정 시간을 대기한 후에 게임 메뉴 열기
            Invoke("ActivateGameMenu", delay);
        }

        // 게임 메뉴 게임 오브젝트를 활성화하는 함수
        private void ActivateGameMenu()
        {
            // 게임 오브젝트 활성화
            gameMenu.SetActive(true);
        }

        // 게임을 재시작할 때 사용하는 함수
        public void RestartGame()
        {
            // 배경 음악 설정, 게임을 다시 시작할 때는 일반 배경 음악을 재생하도록 설정
            BackgroundMusicPlayer musicPlayer = FindFirstObjectByType<BackgroundMusicPlayer>();
            if (musicPlayer != null)
            {
                musicPlayer.PlayNormalMusic();
            }

            // 현재 열려있는 씬 정보를 로드
            Scene scene = SceneManager.GetActiveScene();

            // 현재 씬을 다시 로드
            SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
        }

        // 게임을 종료하는 함수
        public void ExitGame()
        {
#if UNITY_EDITOR
            // 에디터 모드에서 실행할 때는 에디터의 플레이 모드 정지
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // 에디터 모드를 제외할 때는 엔진 종료
            Application.Quit();
#endif
        }
    }
}