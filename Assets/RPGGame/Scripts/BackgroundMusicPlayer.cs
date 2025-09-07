using System.Collections;
using UnityEngine;

namespace RPGGame
{
    // 배경 음악을 재생하는 스크립트
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundMusicPlayer : MonoBehaviour
    {
        // 오디오를 재생할 때 사용할 정보를 저장할 클래스
        [System.Serializable]
        public class AudioInfo
        {
            // 재생할 오디오 클립
            public AudioClip clip;

            // 이 오디오를 재생할 때 설정할 볼륨 값
            // 오디오 파일마다 원하는 볼륨이 다를 수 있다.
            public float defaultVolume = 1;
        }

        // AudioSource 컴포넌트 참조 변수
        [SerializeField] private AudioSource audioPlayer;

        // 재생할 오디오 정보
        [SerializeField] private AudioInfo[] musics;

        // 음악을 전환할 때 사용할 전환 시간(단위: 초)
        [SerializeField] private float transitionTime = 1f;

        private void OnEnable()
        {
            // AudioSource 참조 변수 설정
            if (audioPlayer == null)
            {
                audioPlayer = GetComponent<AudioSource>();
            }

            // 일반 배경 음악 재생
            PlayNormalMusic();
        }

        // 일반 배경 음악 재생 함수
        public void PlayNormalMusic()
        {
            // 음원 전환
            StartCoroutine(TransitionMusic(musics[0]));
        }

        // 보스 몬스터와 전투를 진행할 때 음악 재생 함수
        public void PlayBattleMusic()
        {
            // 음원 전환
            StartCoroutine(TransitionMusic(musics[1]));
        }

        // 음원을 부드럽게 전환할 때 사용하는 코루틴 함수
        private IEnumerator TransitionMusic(AudioInfo targetAudio)
        {
            // 타이머 변수 선언
            float time = 0f;

            // 현재 볼륨 값 저장
            float volume = audioPlayer.volume;

            // 현재 재생되고 있는 음원의 볼륨을 서서히 줄인다.
            while (time < transitionTime)
            {
                audioPlayer.volume = Mathf.Lerp(volume, 0f, time / transitionTime);
                time += Time.deltaTime;
                yield return null;
            }

            // 여기까지 함수가 실행됐으면, 오디오 소스의 볼륨이 0인 상태

            // 새로 재생할 오디오 클립 설정
            audioPlayer.clip = targetAudio.clip;

            // 전환할 때마다 느낌을 약간 다르게 주기 위해 피치 값을 랜덤으로 설정
            audioPlayer.pitch = Random.Range(0.98f, 1.02f);

            // 볼륨이 0인 상태라 아직 소리가 들리지는 않지만, 재생 시작
            audioPlayer.Play();

            // 타이머 변수 초기화
            time = 0f;

            // 재생할 음원의 볼륨을 서서히 올린다.
            while (time < transitionTime)
            {
                audioPlayer.volume = Mathf.Lerp(0f, targetAudio.defaultVolume, time / transitionTime);
                time += Time.deltaTime;
                yield return null;
            }

            // 코루틴 로직이 완료되면, 최종적으로 새로 재생하는 음원에서 재생을 원하는 볼륨 설정
            audioPlayer.volume = targetAudio.defaultVolume;
        }
    }
}