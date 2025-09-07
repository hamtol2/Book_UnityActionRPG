using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPGGame
{
    // NPC의 스테이트를 제어하는 스크립트
    public class NPCStateManager : MonoBehaviour
    {
        // NPC의 스테이트를 나타내는 열거형
        public enum State
        {
            None = -1,
            Idle,           // 대기.
            Talk,           // 대화.
            Length
        }

        // NPC의 현재 스테이트 값
        [SerializeField] private State state = State.None;

        // NPC 스테이트 컴포넌트 배열
        [SerializeField] private NPCStateBase[] states;

        // NPC의 스테이트가 변경될 때 발행되는 이벤트
        [SerializeField] private UnityEvent<State> OnStateChanged;

        // NPC 아이디
        [SerializeField] private int npcID = 0;

        // NPC의 아이디를 반환하는 프로퍼티
        public int NPCID { get { return npcID; } }

        // NPC의 데이터
        public NPCData.Attribute data { get; private set; } = null;

        // 플레이어 트랜스폼 참조 변수
        public Transform PlayerTransform { get; private set; } = null;

        private void Awake()
        {
            // 플레이어 트랜스폼 검색 후 설정
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            // NPC ID를 사용해 NPC 데이터 가져오기
            data = DataManager.Instance.npcData.attributes[npcID - 1];

            // 스테이트 컴포넌트 배열 초기화
            states = new NPCStateBase[(int)State.Length];
            for (int ix = 0; ix < (int)states.Length; ++ix)
            {
                // 컴포넌트 이름 설정
                string componentName = $"RPGGame.NPC{(State)ix}State";

                // 컴포넌트의 이름을 사용해 컴포넌트를 검색한 뒤 states 배열에 추가
                states[ix] = GetComponent(Type.GetType(componentName)) as NPCStateBase;

                // 스테이트 컴포넌트의 초기 상태는 비활성화 처리
                states[ix].enabled = false;
            }
        }

        private void OnEnable()
        {
            // 초기 스테이트 설정
            SetState(State.Idle);
        }

        public void SetState(State newState)
        {
            // 전환하려는 새로운 스테이트와 현재 스테이트가 같으면 처리하지 않고 함수 종료
            if (state == newState)
            {
                return;
            }

            // 스테이트가 None이 아니라면 현재 스테이트를 비활성화
            if (state != State.None)
            {
                states[(int)state].enabled = false;
            }

            // 새로운 스테이트 활성화
            states[(int)newState].enabled = true;

            // 현재 스테이트를 새로운 스테이트로 업데이트
            state = newState;

            // 스테이트 변경 이벤트 발행
            OnStateChanged?.Invoke(state);
        }

        // NPC의 스테이트(상태)가 변경될 때 발행되는 이벤트에 등록하는 함수
        public void SubscribeOnStateChanged(UnityAction<State> listener)
        {
            OnStateChanged?.AddListener(listener);
        }
    }
}