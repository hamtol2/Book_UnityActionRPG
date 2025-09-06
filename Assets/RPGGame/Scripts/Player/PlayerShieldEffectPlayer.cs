using System.Collections;
using UnityEngine;

namespace RPGGame
{
    // 플레이어의 피격 효과를 재생하는 스크립트
    public class PlayerShieldEffectPlayer : MonoBehaviour
    {
        // 피격 효과 재생 시간(단위: 초)
        [SerializeField] private float playTime = 0.5f;

        // 피격 효과 재생 함수
        public void Play()
        {
            // 게임 오브젝트 활성화
            gameObject.SetActive(true);

            // 일정 시간 후에 게임 오브젝트를 비활성화 처리
            StartCoroutine(WaitAndTurnOff(playTime));
        }

        // 일정 시간을 대기한 후에 게임 오브젝트를 비활성화하는 코루틴 함수
        private IEnumerator WaitAndTurnOff(float delay)
        {
            // 함수에 전달된 시간만큼 대기
            yield return new WaitForSeconds(delay);

            // 게임 오브젝트 비활성화
            gameObject.SetActive(false);
        }
    }
}