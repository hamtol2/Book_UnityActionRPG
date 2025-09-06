using UnityEngine;

namespace RPGGame
{
    // 여러 오디오 클립 중에서 임의로 선택해 재생하는 기능을 제공하는 스크립트
    // 오디오 재생을 위해서는 AudioSource 컴포넌트가 필요하기 때문에 RequireComponent를 활용
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        // 오디오의 피치(Pitch) 값을 랜덤으로 설정할 지를 결정하는 옵션
        [SerializeField] private bool isRandomizePitch = false;

        // Pitch를 랜덤으로 설정할 때 적용할 임의의 범위 값
        [SerializeField] private float pitchRandomRange = 0.2f;

        // 재생 시점을 조금 뒤로 미룰 때 사용할 시간 값(단위: 초)
        [SerializeField] private float playDelay = 0f;

        // 재생할 오디오 클립 배열
        [SerializeField] private AudioClip[] audioClips;

        // 오디오를 재생할 컴포넌트(AudioSource)
        private AudioSource audioPlayer;

        private void OnEnable()
        {
            // 오디오 컴포넌트 설정
            if (audioPlayer == null)
            {
                audioPlayer = GetComponent<AudioSource>();
            }
        }

        // 오디오 재생 함수
        public void Play()
        {
            // 재생할 오디오 클립을 임의로 선택
            AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];

            // 피치 값 설정 - 임의로 결정할지 옵션에 따라서 1.0이나 임의로 피치 값을 설정
            audioPlayer.pitch = isRandomizePitch ? Random.Range(1.0f - pitchRandomRange, 1.0f + pitchRandomRange) : 1.0f;

            // 임의로 선택한 오디오 클립을 AudioSource 컴포넌트에 설정
            audioPlayer.clip = clip;

            // 오디오 재생으로, 재생할 때 playDelay 값을 적용
            audioPlayer.PlayDelayed(playDelay);
        }
    }
}