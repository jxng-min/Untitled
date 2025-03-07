using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Junyoung
{ // 스태틱으로 선언 해야함
    public class EventBus : MonoBehaviour
    {
        private static IDictionary<GameEventType, UnityEvent> m_events = new Dictionary<GameEventType, UnityEvent>();
       
        //이벤트 딕셔너리에 이벤트와 엑션을 연결시킨 후 넣는 메소드
        public static void Subscribe(GameEventType event_type, UnityAction listener)
        {
            UnityEvent this_event;

            if (m_events.TryGetValue(event_type, out this_event)) // 이벤트 딕셔너리에 이미 등록된 딕셔너리면 리스너(연결된 메소드)만 추가
            {
                this_event.AddListener(listener);
            }
            else
            {
                this_event = new UnityEvent();
                this_event.AddListener(listener);
                m_events.Add(event_type, this_event);
            }
        }

        // 이벤트 딕셔너리에서 이벤트를 찾아서 해당 리스너를 제거하는 메소드
        public static void Unsubscribe(GameEventType event_type, UnityAction listener)
        {
            UnityEvent this_event;

            if (m_events.TryGetValue(event_type, out this_event))
            {
                this_event.RemoveListener(listener);
            }
        }

        // 해당 이벤트를 실행
        public static void Publish(GameEventType event_type)
        {
            UnityEvent this_event;

            if (m_events.TryGetValue(event_type, out this_event))
            {
                this_event.Invoke();
                Debug.Log($"{event_type.ToString()} 이벤트 실행.");
            }
        }
    }
}

