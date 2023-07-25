using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private Dictionary<Event, Action<object>> eventDic = new Dictionary<Event, Action<object>>();
    public static EventManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void AddListener(Event ev,Action<object> action)
    {
        if (!eventDic.ContainsKey(ev))
        {
            eventDic.Add(ev, action);
        }
        else
        {
            eventDic[ev] += action;
        }
    }

    public void RemoveListener(Event ev,Action<object> action)
    {
        if (eventDic.ContainsKey(ev))
        {
            eventDic[ev] -= action;
        }
    }

    public void OnTriggerEvent(Event ev,object obj)
    {
        if (eventDic.ContainsKey(ev))
        {
            eventDic[ev]?.Invoke(obj);
        }
    }

    public void CleanEvent()
    {
        eventDic.Clear();
        GC.Collect();
    }


}

public enum Event
{
    StartARTracker,//开始AR定位
    LocalizationFound,//定位到位置
    SelectedObjEvent,//选中拖拽的物体
    DeSelectedEvent,//没有选中任何物体
    SelectedToggleEvent,//UI Toggle选中要实例化的物体
    IngObjEvent,//实例化物体
    PlayAudioEvent
}