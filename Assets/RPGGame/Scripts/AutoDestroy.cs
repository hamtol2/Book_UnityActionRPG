using UnityEngine;

namespace RPGGame
{
    // Destroy 함수를 호출한 후에 지정한 초가 지나면 게임 오보젝트를 삭제하는 기능을 제공
    public class AutoDestroy : MonoBehaviour
    {
        // 삭제할 때까지 대기할 시간
        [SerializeField] private float destroyTime = 2f;

        // 삭제 요청 함수
        public void Destroy()
        {
            // 유니티의 GameObject.Destroy 함수를 destroyTime 시간이 지나면 호출해 달라고 예약
            GameObject.Destroy(gameObject, destroyTime);
        }
    }
}