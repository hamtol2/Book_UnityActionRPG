using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame
{
    // 다이얼로그 텍스트를 화면을 보여주는 기능을 담당하는 스크립트
    // 싱글턴으로 제작하기 위해 다른 스크립트보다 빠르게 실행되도록 실행 순서 설정
    [DefaultExecutionOrder(-1)]
    public class Dialogue : MonoBehaviour
    {
        // 다이얼로그 창 게임 오브젝트(창을 열고 닫을 때 사용)
        [SerializeField] private GameObject dialogueWindow;

        // 다이얼로그 메시지를 보여줄 때 사용할 텍스트 UI 참조 변수
        [SerializeField] private TMPro.TextMeshProUGUI dialogueText;

        // 다이얼로그를 일정 시간만 보여줄 때 사용할 시간 값(단위: 초)
        [SerializeField] private float dialogueTempShowTime = 5f;

        // 텍스트 애니메이션 시간 간격(값이 작을수록 텍스트가 빠르게 보임)
        [SerializeField] private float textAnimationInterval = 0.04f;

        // 스태틱 인스턴스
        private static Dialogue instance = null;

        private void Awake()
        {
            // 스태틱 인스턴스 설정
            if (instance == null)
            {
                instance = this;
            }

            // 이미 설정된 인스턴스가 있다면 중복되는 게임 오브젝트 삭제
            else
            {
                Destroy(gameObject);
            }
        }

        // 다이얼로그를 열 때 사용하는 함수
        public static void ShowDialogue()
        {
            // 모든 코루틴 중지(기존에 다이얼로그를 닫는 코루틴이 실행 중일 수 있기 때문에)
            instance.StopAllCoroutines();

            // 다이얼로그 창 활성화
            instance.dialogueWindow.SetActive(true);
        }

        // 다이얼로그를 닫을 때 사용하는 함수
        public static void CloseDialogue()
        {
            // 다이얼로그 창 닫기
            instance.dialogueWindow.SetActive(false);
        }

        // 전달한 시간 이후에 다이얼로그 창을 닫는 함수
        public static void CloseDialogueAfterTime(float time)
        {
            // 일정 시간을 대기한 후 게임 오브젝트를 비활성화하는 코루틴 함수를 호출한다.
            instance.StartCoroutine(instance.CloseDialogueWithDelay(time));
        }

        // 다이얼로그에 텍스트를 보여줄 때 사용하는 함수
        // 타이핑 효과 적용
        public static void ShowDialogueText(string text)
        {
            // 다이얼로그 창 활성화
            ShowDialogue();

            // 타이핑 효과를 적용해 다이얼로그 텍스트를 설정하는 코루틴 함수 실행
            instance.StartCoroutine(instance.SetTextWithAnimation(text));
        }

        // 일정 시간 동안만 텍스트를 보여줄 때 사용하는 함수
        public static void ShowDialogueTextTemporarily(string text, float time = 0f)
        {
            // 다이얼로그 창을 연다.
            ShowDialogue();

            // 모든 코루틴 중단
            instance.StopAllCoroutines();

            // 창을 닫기까지 대기할 시간 계산
            float dialogueShowTime = time == 0f ? instance.dialogueTempShowTime : time;

            // 타이핑 효과를 적용해 다이얼로그 텍스트를 보여주고, 일정 시간을 대기한 후에 창을 닫는 함수 호출
            instance.StartCoroutine(instance.SetTempDialogueTextWithAnimation(text, dialogueShowTime));
        }

        // 일정 시간 동안 텍스트를 보여주되 텍스트 애니메이션을 적용해서 보여주는 함수
        private IEnumerator SetTempDialogueTextWithAnimation(string text, float time)
        {
            // 타이핑 효과를 적용해 메시지를 보여주는 함수 실행
            yield return instance.StartCoroutine(instance.SetTextWithAnimation(text));

            // 일정 시간 대기 후 창을 닫는 함수 호출
            yield return instance.StartCoroutine(instance.CloseDialogueWithDelay(time));
        }

        // 다이얼로그 텍스트를 설정할 때 텍스트에 애니메이션을 적용해 보여주는 함수
        private IEnumerator SetTextWithAnimation(string text)
        {
            // 화면에 보여줄 글자 수
            // 시작은 글자 1개로 설정
            int count = 1;

            // 코루틴 대기를 위한 객체 생성(재활용을 위해 생성)
            WaitForSeconds interval = new WaitForSeconds(textAnimationInterval);

            // 루프를 순회하면서 타이핑 효과 애니메이션 실행
            while (count <= text.Length)
            {
                // 대기
                yield return interval;

                // 다이얼로그 창의 텍스트에 문자열 설정
                // 전체 다이얼로그 대사 중에서 화면에 보여줄 텍스트만 잘라서 보여주기
                instance.dialogueText.text = text.Substring(0, count);

                // 화면에 보여줄 글자 수 증가 처리
                ++count;
            }
        }

        // 지연 시간을 두고 창을 닫을 때 내부에서 사용하는 코루틴 함수
        private IEnumerator CloseDialogueWithDelay(float delay)
        {
            // 함수에 전달받은 시간만큼 대기
            yield return new WaitForSeconds(delay);

            // 창 닫기.
            CloseDialogue();
        }
    }
}