using UnityEngine;

namespace RPGGame
{
    // 퀘스트 정보를 담고 있는 스크립트
    // 퀘스트 대상이 되는 오브젝트에 부착
    public class QuestItem : MonoBehaviour
    {
        // 퀘스트 타입
        [SerializeField] private QuestData.Type type = QuestData.Type.None;

        // 퀘스트 타깃 타입
        [SerializeField] private QuestData.TargetType targetType = QuestData.TargetType.None;

        // 퀘스트 아이템의 타입을 설정하는 함수
        public void SetType(QuestData.Type type)
        {
            this.type = type;
        }

        // 퀘스트 아이템을 완료했을 때 실행하는 함수
        public virtual void OnCompleted()
        {
            // 예외 처리. 두 값은 None이면 안 된다. 
            if (type == QuestData.Type.None || targetType == QuestData.TargetType.None)
            {
                return;
            }

            // 퀘스트 관리자에 퀘스트 달성 정보 전달
            QuestManager.Instance.ProcessQuest(type, targetType);
        }
    }
}