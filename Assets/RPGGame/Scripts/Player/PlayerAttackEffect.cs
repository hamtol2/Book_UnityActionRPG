using System.Collections;
using UnityEngine;

namespace RPGGame
{
    // 플레이어가 공격 모션을 할 때 함께 재생할 애니메이션 효과이펙트의 기능을 담당하는 스크립트.
    public class PlayerAttackEffect : MonoBehaviour
    {
        // 재생할 애니메이션 컴포넌트 참조 변수.
        private Animation refAnimation;

        private void Awake()
        {
            // 컴포넌트 초기화.
            if (refAnimation == null)
            {
                refAnimation = GetComponent<Animation>();
            }

            // 시작 시 게임 오브젝트 비활성화 (효과이펙트 자동 재생을 막기 위해).
            gameObject.SetActive(false);
        }

        // 파티클 재생 함수.
        public void Activate()
        {
            // 게임 오브젝트 활성화 (애니메이션 재생을 위해서 활성화).
            gameObject.SetActive(true);

            // 애니메이션 컴포넌트 null 확인 후 문제 없으면 애니메이션 재생.
            refAnimation.Play();

            // 애니메이션 재생 후 종료를 위한 코루틴 함수 실행.
            StartCoroutine(DisableAtEndOfAnimation());
        }

        // 애니메이션 클립의 재생 시간만큼 대기한 후에 컴포넌트를 비활성화하는 함수.
        private IEnumerator DisableAtEndOfAnimation()
        {
            // 애니메이션 클립의 재생 시간 만큼 대기.
            yield return new WaitForSeconds(refAnimation.clip.length);

            // 게임 오브젝트 및 라이트 컴포넌트 비활성화.
            gameObject.SetActive(false);
        }
    }
}