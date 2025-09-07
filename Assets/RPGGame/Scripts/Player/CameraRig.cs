using UnityEngine;

namespace RPGGame
{
    // 플레이어를 따라 다니는 카메라 스크립트
    public class CameraRig : MonoBehaviour
    {
        // 카메라가 따라다닐 타겟(플레이어 캐릭터)의 트랜스폼 컴포넌트 참조 변수
        [SerializeField] private Transform followTarget;

        // 플레이어를 따라다닐 때 적용할 이동 딜레이 값
        [SerializeField] private float movementDelay = 5f;

        // 카메라의 트랜스폼 컴포넌트 참조 변수
        private Transform refTransform;

        // 카메라 컴포넌트 참조 변수
        private Camera refCamera;

        // 카메라가 회전할 때 적용할 지연 값
        [SerializeField] private float rotationDelay = 5f;

        // 마우스 드래그에 따른 카메라 회전 빠르기를 제어하는 변수
        // 회전이 너무 빠르거나 너무 느릴 때가 있어서 감도를 조정하는 데 사용
        [SerializeField] private float rotationSpeed = 0.2f;

        // 카메라 X축 각도 제한
        [SerializeField] private Vector2 rotationXMinMax = new Vector2(-20f, 25f);

        // X 회전 값 계산 변수
        private float xRotation = 0f;

        // Y 회전 값 계산 변수
        private float yRotation = 0f;

        private void Awake()
        {
            // 트랜스폼 컴포넌트 참조 변수 설정
            if (refTransform == null)
            {
                refTransform = transform;
            }

            // 태그를 사용해 플레이어 캐릭터의 트랜스폼을 설정
            if (followTarget == null)
            {
                followTarget = GameObject.FindGameObjectWithTag("Player").transform;
            }

            // 카메라 참조 변수 설정
            if (refCamera == null)
            {
                refCamera = Camera.main;
            }
        }

        // LateUpdate 함수는 Update 함수와 비슷하게 프레임마다 실행
        // 하지만 LateUpdate 함수는 Update가 실행된 이후 시점에 실행
        private void LateUpdate()
        {
            // 카메라가 캐릭터를 따라다닐 때 약간의 지연(딜레이) 효과를 적용해 부드럽게 따라가도록 Vector3.Lerp를 활용
            refTransform.position = Vector3.Lerp(refTransform.position, followTarget.position, movementDelay * Time.deltaTime);

            // 카메라 회전 처리

            // UI 창이 열려있을 때는 회전 처리 무시
            if (UIInventoryWindow.IsOn || UIQuestWindow.IsOn)
            {
                return;
            }

            // 마우스의 상하 드래그 값을 적용해 카메라의 X회전에 누적
            xRotation -= InputManager.MouseMove.y * rotationSpeed;

            // X 회전이 미리 정해둔 범위를 벗어나지 않도록 회전 값 고정
            xRotation = Mathf.Clamp(xRotation, rotationXMinMax.x, rotationXMinMax.y);

            // 마우스의 좌우 드래그 값을 적용해 카메라의 Y회전에 누적 
            yRotation += InputManager.MouseMove.x * rotationSpeed;

            // 부드럽게 회전을 처리하기 위해 현재 회전 값 준비
            Quaternion startRotation = refTransform.rotation;

            // 부드럽게 회전을 처리하기 위해 목표 회전 값 준비
            // 마우스 드래그 입력을 적용한 X, Y 회전 값을 쿼터니언으로 변환
            Quaternion endRotation = Quaternion.Euler(new Vector3(xRotation, yRotation, 0f));

            // 회전 보간 함수인 Slerp를 활용해 startRotation에서 endRotation으로 부드럽게 회전 적용
            refTransform.rotation = Quaternion.Slerp(startRotation, endRotation, rotationDelay * Time.deltaTime);
        }
    }
}