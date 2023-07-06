using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void EventReceiver(params object[] parameters);
    static Dictionary<EventType, EventReceiver> _events = new Dictionary<EventType, EventReceiver>();

    public static void Subscribe(EventType eventType,EventReceiver listener)
    {
        if (!_events.ContainsKey(eventType))
        {
            _events.Add(eventType,listener);   
        }
        else
        {
            _events[eventType] += listener;
        }
    } 
    public static void Unsuscribe(EventType eventType, EventReceiver listener)
    {
        if (_events.ContainsKey(eventType))
        {
            _events[eventType] -= listener;   
        }
    }

    public static void TriggerEvent(EventType eventType,params object[] parameters)
    {
        if (_events.ContainsKey(eventType))
        {   
            _events[eventType](parameters);
        }
    }
    public static void clear()
    {
        _events.Clear();   
    }
    public enum EventType
    {
        ChangeLevel,

    }



}

