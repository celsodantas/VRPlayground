using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    [System.Serializable]
    public class VRHomeEvent : UnityEvent<object>
    {
    }

    public enum Event
    {
        INTERACTABLE_MENU_GRAB_PRESSED = 1
    }

    private Dictionary<Event, VRHomeEvent> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<Event, VRHomeEvent>();
        }
    }

    public static void StartListening(Event eventID, UnityAction<object> listener)
    {
        VRHomeEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventID, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new VRHomeEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventID, thisEvent);
        }
    }

    public static void StopListening(Event eventID, UnityAction<object> listener)
    {
        if (eventManager == null) return;
        VRHomeEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventID, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(Event eventID, object sender)
    {
        VRHomeEvent thisEvent = null;

        if (instance.eventDictionary.TryGetValue(eventID, out thisEvent))
        {
            thisEvent.Invoke(sender);
        }
    }
}