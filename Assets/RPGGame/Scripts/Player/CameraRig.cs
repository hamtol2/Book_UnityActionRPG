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
        }

        // LateUpdate 함수는 Update 함수와 비슷하게 프레임마다 실행
        // 하지만 LateUpdate 함수는 Update가 실행된 이후 시점에 실행
        private void LateUpdate()
        {
            // 카메라가 캐릭터를 따라다닐 때 약간의 지연(딜레이) 효과를 적용해 부드럽게 따라가도록 Vector3.Lerp를 활용
            refTransform.position = Vector3.Lerp(refTransform.position, followTarget.position, movementDelay * Time.deltaTime);
        }
    }
}