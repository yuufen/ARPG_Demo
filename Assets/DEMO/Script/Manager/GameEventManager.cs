using System;
using System.Collections;
using System.Collections.Generic;
using DEMO.Tool;
using DEMO.Tool.Singleton;
using UnityEngine;

// 事件总线单例，不需要继承 MonoBehaviour
public class GameEventManager : SingletonNonMono<GameEventManager> {
    // 用来作为下面几种事件的通用的类型
    private interface IEventHelper {
    }

    // 无参数的事件
    private class EventHelper : IEventHelper {
        // 声明一个 _action 事件，用来收集委托
        // Action 为事件的委托的类型（c# 使用委托来实现事件回调）
        private event Action _action;

        // 构造器
        public EventHelper(Action action) {
            _action = action;
        }

        // 注册回调（委托）
        public void Add(Action action) {
            _action += action;
        }

        // 触发事件
        public void Call() {
            _action?.Invoke();
        }

        // 移除回调（委托）
        public void Remove(Action action) {
            _action -= action;
        }
    }

    // 一个参数的事件
    private class EventHelper<T> : IEventHelper {
        private event Action<T> _action;

        public EventHelper(Action<T> action) {
            _action = action;
        }

        public void Add(Action<T> action) {
            _action += action;
        }

        public void Call(T value) {
            _action?.Invoke(value);
        }

        public void Remove(Action<T> action) {
            _action -= action;
        }
    }

    // 两个参数的事件
    private class EventHelper<T1, T2> : IEventHelper {
        private event Action<T1, T2> _action;

        public EventHelper(Action<T1, T2> action) {
            _action = action;
        }

        public void Add(Action<T1, T2> action) {
            _action += action;
        }

        public void Call(T1 value1, T2 value2) {
            _action?.Invoke(value1, value2);
        }

        public void Remove(Action<T1, T2> action) {
            _action -= action;
        }
    }

    /// <summary>
    /// 用来收集事件
    /// </summary>
    private Dictionary<string, IEventHelper> _eventCenter = new Dictionary<string, IEventHelper>();

    /// <summary>
    /// 添加事件监听，封装一下 _eventCenter
    /// </summary>
    /// <param name="eventName">事件名称</param>
    /// <param name="action">回调函数</param>
    public void AddListener(string eventName, Action action) {
        if (_eventCenter.TryGetValue(eventName, out var e)) {
            (e as EventHelper)?.Add(action);
        } else {
            _eventCenter.Add(eventName, new EventHelper(action));
        }
    }

    public void AddListener<T>(string eventName, Action<T> action) {
        if (_eventCenter.TryGetValue(eventName, out var e)) {
            (e as EventHelper<T>)?.Add(action);
        } else {
            _eventCenter.Add(eventName, new EventHelper<T>(action));
        }
    }

    public void AddListener<T1, T2>(string eventName, Action<T1, T2> action) {
        if (_eventCenter.TryGetValue(eventName, out var e)) {
            (e as EventHelper<T1, T2>)?.Add(action);
        } else {
            _eventCenter.Add(eventName, new EventHelper<T1, T2>(action));
        }
    }


    /// <summary>
    /// 触发事件，封装一下 _eventCenter
    /// </summary>
    /// <param name="eventName">事件名称</param>
    public void CallEvent(string eventName) {
        if (_eventCenter.TryGetValue(eventName, out var e)) {
            (e as EventHelper)?.Call();
        } else {
            DevelopmentTools.WTF($"未找到 <{eventName}> 事件");
        }
    }

    public void CallEvent<T>(string eventName, T value) {
        if (_eventCenter.TryGetValue(eventName, out var e)) {
            (e as EventHelper<T>)?.Call(value);
        } else {
            DevelopmentTools.WTF($"未找到 <{eventName}> 事件");
        }
    }

    public void CallEvent<T1, T2>(string eventName, T1 value1, T2 value2) {
        if (_eventCenter.TryGetValue(eventName, out var e)) {
            (e as EventHelper<T1, T2>)?.Call(value1, value2);
        } else {
            DevelopmentTools.WTF($"未找到 <{eventName}> 事件");
        }
    }


    /// <summary>
    /// 移除事件监听，封装一下 _eventCenter
    /// </summary>
    /// <param name="eventName">事件名称</param>
    /// <param name="action">回调函数</param>
    public void RemoveListener(string eventName, Action action) {
        if (_eventCenter.TryGetValue(eventName, out var e)) {
            (e as EventHelper)?.Remove(action);
        } else {
            DevelopmentTools.WTF($"未找到 <{eventName}> 事件");
        }
    }

    public void RemoveListener<T>(string eventName, Action<T> action) {
        if (_eventCenter.TryGetValue(eventName, out var e)) {
            (e as EventHelper<T>)?.Remove(action);
        } else {
            DevelopmentTools.WTF($"未找到 <{eventName}> 事件");
        }
    }

    public void RemoveListener<T1, T2>(string eventName, Action<T1, T2> action) {
        if (_eventCenter.TryGetValue(eventName, out var e)) {
            (e as EventHelper<T1, T2>)?.Remove(action);
        } else {
            DevelopmentTools.WTF($"未找到 <{eventName}> 事件");
        }
    }
}