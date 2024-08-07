using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHub
{
    private static Dictionary<EventType, Action<object>> _events = new Dictionary<EventType, Action<object>>();

    public static void SendEvent(EventType eventType, object payload) {
        Debug.Log($"{eventType} {payload}");

        if (!_events.ContainsKey(eventType)) {
            return;
        }

        _events[eventType]?.Invoke(payload);
    }

    public static void Listen(EventType eventType, Action<object> handler) {
        if (!_events.ContainsKey(eventType)) {
            _events[eventType] = handler;
        }
        else {
            _events[eventType] += handler;
        }
    }

    public static void Unlisten(EventType eventType, Action<object> handler) {
        if (!_events.ContainsKey(eventType)) {
            return;
        }

        _events[eventType] -= handler;
    }
}