using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RPGGame
{
    // 플레이어의 레벨 관리를 담당하는 스크립트
    public class PlayerLevelController : MonoBehaviour
    {
        // 레벨
        [SerializeField] private int level = 1;

        // 현재까지 획득한 경험치
        [SerializeField] private float currentExp = 0f;

        // 레벨업 이벤트로 플레이어가 레벨업을 할 때 발행된다.
        [SerializeField] private UnityEvent<int> OnLevelUP;

        // 레벨 정보를 보여주기 위한 레벨 UI
        [SerializeField] private TMPro.TextMeshProUGUI levelGaugeText;

        // 경험치 획득 양을 보여주기 위한 경험치 바 UI
        [SerializeField] private Image expBar;

        // 경험치 획득 양을 텍스트로 보여주기 위한 UI
        [SerializeField] private TMPro.TextMeshProUGUI expGaugeText;

        // 최대 레벨 값
        private int maxLevel;

        // 현재 레벨이 최대 레벨인지 확인할 때 사용하는 프로퍼티
        private bool IsMaxLevel { get { return level == maxLevel; } }

        private void Awake()
        {
            // 최대 레벨 값 저장
            maxLevel = DataManager.Instance.playerData.levels.Count;

            // 레벨 관련 초기화
            UpdateExpBar();
            UpdateLevelText();
        }

        private void UpdateExpBar()
        {
            // 이미 최대 레벨인 경우에는 업데이트 처리하지 않는다.
            if (IsMaxLevel)
            {
                // 경험치 바 UI 설정(최대치로 설정)
                if (expBar != null)
                {
                    // UI 채우기 값 최대로 설정
                    expBar.fillAmount = 1f;
                }

                // 경험치 텍스트 설정
                if (expGaugeText != null)
                {
                    expGaugeText.text = "MAX";
                }

                return;
            }

            // 다음 레벨 인덱스 저장
            int nextLevelIndex = level;

            // 다음 레벨에 도달하기까지 필요한 경험치 양 확인
            float requiredExpForNextLevel = DataManager.Instance.playerData.levels[nextLevelIndex].requiredExp;

            // 현재 레벨에 도달하기까지 필요한 경험치 양 확인
            float requiredExpForCurrentLevel = DataManager.Instance.playerData.levels[level - 1].requiredExp;

            // 경험치 바 UI 설정
            if (expBar != null)
            {
                // 경험치 양 계산
                float expAmount = (currentExp - requiredExpForCurrentLevel) / (requiredExpForNextLevel - requiredExpForCurrentLevel);

                // 설정
                expBar.fillAmount = expAmount;
            }

            // 경험치 텍스트 설정
            if (expGaugeText != null)
            {
                expGaugeText.text = $"{currentExp - requiredExpForCurrentLevel}/{requiredExpForNextLevel - requiredExpForCurrentLevel}";
            }
        }

        private void UpdateLevelText()
        {
            // 레벨 정보 UI 업데이트
            if (levelGaugeText != null)
            {
                // 이미 최고 레벨이면 최고 레벨로 표시
                if (IsMaxLevel)
                {
                    levelGaugeText.text = $"{maxLevel}/{maxLevel}";
                    return;
                }

                // 레벨 텍스트 업데이트
                levelGaugeText.text = $"{level}/{maxLevel}";
            }
        }

        // 경험치를 획득할 때 실행되는 함수
        public void GainExp(float exp)
        {
            // 경험치 획득
            currentExp += exp;

            // 경험치 바 업데이트
            UpdateExpBar();

            // 이미 최고 레벨에 도달했다면 처리하지 않고 함수 종료
            if (IsMaxLevel)
            {
                return;
            }

            // 레벨 업 확인 처리
            int oldLevelIndex = level - 1;
            int targetLevelIndex = 0;
            for (int ix = targetLevelIndex; ix < maxLevel; ++ix)
            {
                // 현재 경험치가 어느 레벨에 해당하는지 확인
                if (currentExp < DataManager.Instance.playerData.levels[ix].requiredExp)
                {
                    targetLevelIndex = ix - 1;
                    break;
                }
            }

            // 예외처리: 이미 획득한 경험치가 최대 레벨에서 요구하는 경험치를 넘은 경우
            // 최고 레벨로 인덱스 값을 조정
            if (level > 1 && currentExp > 0f && targetLevelIndex == 0)
            {
                targetLevelIndex = maxLevel - 1;
            }

            // 레벨업된 경우 이벤트 발행
            if (oldLevelIndex != targetLevelIndex)
            {
                // 레벨 정보 업데이트
                level = targetLevelIndex + 1;

                // 변경된 레벨을 데이터로 전달하면서 이벤트 발행
                OnLevelUP?.Invoke(level);

                // 경험치 바 업데이트
                UpdateExpBar();

                // 레벨 텍스트 업데이트
                UpdateLevelText();
            }
        }

        // OnLevelUP 이벤트에 구독할 때 사용하는 함수
        public void SubscribeOnLevelUP(UnityAction<int> listener)
        {
            OnLevelUP?.AddListener(listener);
        }
    }
}