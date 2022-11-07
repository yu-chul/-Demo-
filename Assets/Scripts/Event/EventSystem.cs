using System.Collections;
using System.Collections.Generic;
using System;

public delegate void EventListenerDelegate(BaseEvent evt);

public class EventSystem 
{
    static EventSystem instance;
    public static EventSystem GetInstance()
    {
        if (instance == null)
            instance = new EventSystem();
        return instance;
    }

    private Hashtable listeners = new Hashtable();

    public void AddEventListener(EventType eventType,EventListenerDelegate listener)
    {
        EventListenerDelegate eventListenerDelegate = this.listeners[eventType] as EventListenerDelegate;
        eventListenerDelegate = (EventListenerDelegate)Delegate.Combine(eventListenerDelegate, listener);
        this.listeners[eventType] = eventListenerDelegate;
    }
    public void RemoveEventListener(EventType eventType, EventListenerDelegate listener)
    {
        EventListenerDelegate eventListenerDelegate = this.listeners[eventType] as EventListenerDelegate;
        if(eventListenerDelegate != null)
        {
            eventListenerDelegate = (EventListenerDelegate)Delegate.Combine(eventListenerDelegate, listener);
        }
        this.listeners[eventType] = eventListenerDelegate;
    }
    public void SendEvent(BaseEvent evt)
    {
        EventListenerDelegate eventListenerDelegate = this.listeners[evt.Type] as EventListenerDelegate;
        if (eventListenerDelegate != null)
        {
            try
            {
                eventListenerDelegate(evt);
            }
            catch(Exception ex)
            {
                throw new Exception(string.Concat(new string[]
                {
                    "Error dispatch event ",
                    evt.Type.ToString(),
                    ":",
                    ex.Message,
                    " ",
                    ex.StackTrace
                }), ex);
            }
        }
    }

    public void a(Action fun)
    {
        fun.Invoke();
        System.Threading.Timer timer = new System.Threading.Timer(
        new System.Threading.TimerCallback((object a) => { fun?.Invoke(); }), null,
        0, 1000);//1S定时器
    }
    private static System.Timers.Timer aTimer;
    public void RemoveAll()
    {
        this.listeners.Clear();
    }
}
