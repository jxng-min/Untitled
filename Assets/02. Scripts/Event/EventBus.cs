using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Junyoung
{ // ����ƽ���� ���� �ؾ���
    public class EventBus : MonoBehaviour
    {
        private static IDictionary<GameEventType, UnityEvent> m_events = new Dictionary<GameEventType, UnityEvent>();
       
        //�̺�Ʈ ��ųʸ��� �̺�Ʈ�� ������ �����Ų �� �ִ� �޼ҵ�
        public static void Subscribe(GameEventType event_type, UnityAction listener)
        {
            UnityEvent this_event;

            if (m_events.TryGetValue(event_type, out this_event)) // �̺�Ʈ ��ųʸ��� �̹� ��ϵ� ��ųʸ��� ������(����� �޼ҵ�)�� �߰�
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

        // �̺�Ʈ ��ųʸ����� �̺�Ʈ�� ã�Ƽ� �ش� �����ʸ� �����ϴ� �޼ҵ�
        public static void Unsubscribe(GameEventType event_type, UnityAction listener)
        {
            UnityEvent this_event;

            if (m_events.TryGetValue(event_type, out this_event))
            {
                this_event.RemoveListener(listener);
            }
        }

        // �ش� �̺�Ʈ�� ����
        public static void Publish(GameEventType event_type)
        {
            UnityEvent this_event;

            if (m_events.TryGetValue(event_type, out this_event))
            {
                this_event.Invoke();
                Debug.Log($"{event_type.ToString()} �̺�Ʈ ����.");
            }
        }
    }
}

