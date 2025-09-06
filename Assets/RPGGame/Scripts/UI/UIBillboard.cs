using UnityEngine;

namespace RPGGame
{
    // 항상 카메라를 바라보도록 회전을 설정하는 빌보드 스크립트.
    public class UIBillborad : MonoBehaviour
    {
        // 회전을 제어하기 위한 트랜스폼 컴포넌트 참조 변수
        private Transform refTransform;

        // 카메라의 회전 방향을 설정하기 위한 카메라 참조 변수
        private Camera mainCamera;

        private void Awake()
        {
            // 트랜스폼 참조 변수 설정
            if (refTransform == null)
            {
                refTransform = transform;
            }

            // 카메라 참조 변수 설정
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        private void LateUpdate()
        {
            // 카메라의 앞방향을 트랜스폼의 앞방향으로 설정
            // 앞방향 벡터인 forward를 직접 설정하면 회전을 설정하는 것과 같은 효과를 얻을 수 있다.
            refTransform.forward = mainCamera.transform.forward;
        }
    }
}