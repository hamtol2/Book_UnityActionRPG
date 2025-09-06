using UnityEngine;
using UnityEngine.Events;

namespace RPGGame
{
    // 플레이어가 수집할 수 있는 아이템 클래스
    [RequireComponent(typeof(Collider))]
    public class CollectableItem : MonoBehaviour
    {
        // 아이템 정보.
        [SerializeField] protected Item item;

        // 아이템 수집 후 게임 오브젝트를 삭제할 지 여부를 결정하는 옵션
        [SerializeField] private bool shouldDeleteAfterCollected = true;

        // 아이템 획득 이벤트
        [SerializeField] protected UnityEvent OnItemCollected;

        // 트랜스폼 컴포넌트 참조 변수
        protected Transform refTransform;

        // 아이템이 수집됐는지를 알려주는 프로퍼티
        public bool HasCollected { get; protected set; } = false;

        // 아이템 정보를 반환하는 공개 프로퍼티
        public Item Item { get { return item; } }

        protected virtual void Awake()
        {
            // 트랜스폼 설정
            if (refTransform == null)
            {
                refTransform = transform;
            }
        }

        // 트리거(Trigger) 타입의 충돌을 시작할 때 실행되는 이벤트 함수
        private void OnTriggerEnter(Collider other)
        {
            OnCollect(other);
        }

        // 이 아이템을 획득할 때 실행되는 함수
        // 아이템마다 획득 시 제공하는 기능이 달라질 수 있기 때문에
        // 확장이 가능하도록 virtual 키워드로 선언
        protected virtual void OnCollect(Collider other)
        {
            // 아이템이 설정되지 않았으면 함수 종료
            // item 변수는 꼭 설정돼야 함
            if (item == null)
            {
                Debug.Log("item 변수가 설정되지 않았습니다.");
                return;
            }

            // 이미 획득한 아이템이거나
            // 충돌한 다른 게임 오브젝트의 태그가 Player가 아니면 함수 종료
            if (HasCollected || !other.CompareTag("Player"))
            {
                return;
            }

            // 인벤토리 관리자에 아이템 추가
            InventoryManager.Instance.OnItemCollected(this);

            // 아이템 수집 후 제거해야 할 때 게임 오브젝트 삭제
            if (shouldDeleteAfterCollected)
            {
                Destroy(gameObject);
            }

            // 아이템이 수집됐다는 이벤트 발행
            OnItemCollected?.Invoke();

            // 다이얼로그에 아이템 수집 메시지 전달
            Dialogue.ShowDialogueTextTemporarily(item.messageWhenCollected);
        }
    }
}