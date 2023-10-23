using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 订阅者模式
/// </summary>
public static class GlobalSubscribeSys{
    public delegate void EventDelegate(params object[] objects);
    public static Dictionary<string, List<EventDelegate>> events = new Dictionary<string, List<EventDelegate>>();

    /// <summary>
    /// 注册事件，不带返回参数
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public static void Subscribe(string eventName, EventDelegate callback)
    {
        List<EventDelegate> actions = null;
        //eventName已存在
        if (events.TryGetValue(eventName, out actions))
        {
            actions.Add(callback);
        }
        //eventName不存在
        else
        {
            actions = new List<EventDelegate>();  
            actions.Add(callback);
            events.Add(eventName, actions);
        }
    }

    public static void UnSubscribe(string eventName,EventDelegate callback)
    {
        List<EventDelegate> actions = null;
        if (events.TryGetValue(eventName, out actions))
        {
            actions.Remove(callback);
            if (actions.Count == 0)
            {
                events.Remove(eventName);
            }
        }
    }
    /// <summary>
    /// 移除全部事件
    /// </summary>
    public static void UnSubscribeAllEvents ()
    {
        events.Clear();
    }


    public static void Invoke(string eventName,params object[] args)
    {
        List<EventDelegate> actions = null;
        if (events.ContainsKey(eventName))
        {
            events.TryGetValue(eventName, out actions);
            for(int i = 0 ;i<actions.Count;i++){
                actions[i](args);
            }
        }
    }
}