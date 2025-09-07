using UnityEngine;

// 프로젝트에서 사용하는 네임 스페이스
namespace RPGGame
{
    // 플레이어의 이동(Move) 상태를 담당하는 스크립트
    // 스테이트에 필요한 함수를 사용할 수 있도록 PlayerStateBase 클래스를 상속한다.
    public class PlayerMoveState : PlayerStateBase
    {
        // 회전 속력 (단위: 도/초).
        //[SerializeField] private float rotationSpeed = 540f;

        // 카메라 트랜스폼 참조 변수
        private Transform cameraTransform;

        protected override void OnEnable()
        {
            base.OnEnable();

            // 카메라 트랜스폼 참조 변수 설정
            if (cameraTransform == null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        protected override void Update()
        {
            base.Update();

            // 입력 받은 방향 값을 사용해 이동할 방향 벡터 만들기
            //Vector3 direction = new Vector3(InputManager.Movement.x, 0f, InputManager.Movement.y);
            // 카메라의 앞방향 계산. 카메라의 앞방향은 아래나 위를 향할 수 있으므로 y 성분을 0으로 조정
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0f;

            // 방향 벡터의 길이를 1로 설정하기 위해 정규화 처리
            cameraForward.Normalize();

            // 이때 카메라가 바라보는 방향을 고려해 계산
            Vector3 direction = InputManager.Movement.x * cameraTransform.right + InputManager.Movement.y * cameraForward;
            direction.y = 0f;

            // 대각선 이동 방향의 경우에는 상하/좌우 이동 방향보다 방향 벡터의 크기 값이 더 크기 때문에, 
            // 이를 정규화해서 모든 방향에서의 벡터 크기 값이 같도록 처리
            if (direction.sqrMagnitude > 1.0f)
            {
                direction.Normalize();
            }

            // 회전.
            if (direction != Vector3.zero)
            {
                // 이동 방향을 바라보는 회전 값 생성.
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // 현재 회전에서 목표 회전으로 부드럽게 회전 적용.
                refTransform.rotation = Quaternion.RotateTowards(refTransform.rotation, targetRotation, data.rotationSpeed * Time.deltaTime);
            }
        }

        // EllenRunForward 애니메이션에 추가된 PlayStep 이벤트를 받는 함수.
        private void PlayStep()
        {

        }
    }
}