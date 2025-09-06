using System.Collections;
using UnityEngine;

namespace RPGGame
{
    // 카메라 흔들기 효과를 제공하는 스크립트
    public class CameraShake : MonoBehaviour
    {
        // 카메라의 트랜스폼 컴포넌트
        [SerializeField] private Transform cameraTransform;

        // 카메라를 흔들 때 사용할 값
        [SerializeField] private float shakeTime = 0.4f;          // 흔드는 시간(단위: 초)
        [SerializeField] private float shakeAmount = 0.05f;       // 흔들기 떨림 정도

        // 카메라의 원래 위치
        private Vector3 originPosition;

        // 현재 카메라를 흔들고 있는지 확인하는 변수
        private bool isShaking = false;

        private void Awake()
        {
            // 카메라 트랜스폼 설정
            if (cameraTransform == null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        void OnEnable()
        {
            // 카메라의 원래 위치 저장
            originPosition = cameraTransform.localPosition;
        }

        // 카메라 흔들기 효과를 재생하는 함수
        public void Play()
        {
            // 이미 카메라를 흔들고 있다면 재실행 방지를 위해 함수 종료
            if (isShaking)
            {
                return;
            }

            // 카메라 흔들기를 시작할 때 원래 위치 저장
            originPosition = cameraTransform.localPosition;

            // ShakeCamera 코루틴 함수 실행
            StartCoroutine("ShakeCamera");
        }

        // 코루틴(Coroutine)으로 카메라 흔드는 효과를 구현한 함수
        private IEnumerator ShakeCamera()
        {
            // 카메라 흔들기 상태로 설정
            isShaking = true;

            // 경과 시간을 계산할 변수 선언
            float elapsedTime = 0f;

            // shakeTime 변수에 설정한 시간이 지날 때까지 효과 재생
            while (elapsedTime < shakeTime)
            {
                // 카메라를 흔들 위치를 랜덤 값으로 구한다.
                // 위치가 원점(0,0,0)이고, shakeAmount를 반지름으로 하는 구체에서 랜덤으로 위치 선택
                Vector3 shakePosition = Random.insideUnitSphere * shakeAmount;

                // 카메라의 위치를 변경해 카메라를 흔듦
                // 공식 = 원래 위치 + 랜덤 위치;
                cameraTransform.localPosition = originPosition + shakePosition;

                // 경과 시간 업데이트.
                elapsedTime += Time.deltaTime;

                // 한 프레임 대기
                yield return null;
            }

            // 흔드는 효과를 모두 재생한 다음에는 카메라 위치를 원래 위치로 설정
            cameraTransform.localPosition = originPosition;

            // 카메라 흔들기 완료
            isShaking = false;
        }
    }
}