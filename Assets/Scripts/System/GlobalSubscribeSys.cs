using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 订阅者模式
/// </summary>
public static class GlobalSubscribeSys{
    public delegate void EventDelegate(params object[] objects);
    public delegate object EventDelegateWithReturn(params object[] objects);
    private static Dictionary<string, List<EventDelegate>> _events = new Dictionary<string, List<EventDelegate>>();
    public static Dictionary<string, List<EventDelegate>> Events {
        get
        {
            return _events;
        }    
    }
    
    private static Dictionary<string, List<EventDelegateWithReturn>> _returnEvents = new Dictionary<string, List<EventDelegateWithReturn>>();
    public static Dictionary<string, List<EventDelegateWithReturn>> ReturnEvents {
        get
        {
            return _returnEvents;
        }    
    }


    /// <summary>
    /// 注册事件，不带返回参数
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public static void Subscribe(string eventName, EventDelegate callback)
    {
        List<EventDelegate> actions = null;
        //eventName已存在
        if (_events.TryGetValue(eventName, out actions))
        {
            actions.Add(callback);
        }
        //eventName不存在
        else
        {
            actions = new List<EventDelegate>();  
            actions.Add(callback);
            _events.Add(eventName, actions);
        }
    }

    /// <summary>
    /// 注册事件，带返回参数
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public static void Subscribe(string eventName, EventDelegateWithReturn callback)
    {
        List<EventDelegateWithReturn> actions = null;
        //eventName已存在
        if (_returnEvents.TryGetValue(eventName, out actions))
        {
            actions.Add(callback);
        }
        //eventName不存在
        else
        {
            actions = new List<EventDelegateWithReturn>();  
            actions.Add(callback);
            _returnEvents.Add(eventName, actions);
        }
    }
    
    public static void UnSubscribe(string eventName,EventDelegate callback)
    {
        List<EventDelegate> actions = null;
        if (_events.TryGetValue(eventName, out actions))
        {
            actions.Remove(callback);
            if (actions.Count == 0)
            {
                _events.Remove(eventName);
            }
        }
    }
    
    public static void UnSubscribe(string eventName,EventDelegateWithReturn callback)
    {
        List<EventDelegateWithReturn> actions = null;
        if (_returnEvents.TryGetValue(eventName, out actions))
        {
            actions.Remove(callback);
            if (actions.Count == 0)
            {
                _returnEvents.Remove(eventName);
            }
        }
    }
    
    /// <summary>
    /// 移除全部事件
    /// </summary>
    public static void UnSubscribeAllEvents ()
    {
        _events.Clear();
        _returnEvents.Clear();
    }


    public static void Invoke(string eventName,params object[] args)
    {
        List<EventDelegate> actions = null;
        if (_events.ContainsKey(eventName))
        {
            _events.TryGetValue(eventName, out actions);
            for(int i = 0 ;i<actions.Count;i++){
                actions[i](args);
            }
        }
    }
    
    public static void Invoke(string eventName,out List<object> returnObjects,params object[] args)
    {
        List<EventDelegateWithReturn> actions = null;
        returnObjects = new List<object>();
        if (_returnEvents.ContainsKey(eventName))
        {
            _returnEvents.TryGetValue(eventName, out actions);
            for(int i = 0 ;i<actions.Count;i++)
            {
                object o = actions[i](args);
                returnObjects.Add(o);
            }
        }
    }
    
}