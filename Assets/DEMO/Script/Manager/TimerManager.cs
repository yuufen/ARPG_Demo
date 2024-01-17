using System;
using System.Collections;
using System.Collections.Generic;
using DEMO.Tool.Singleton;
using UnityEngine;

// 用来管理所有计时器 
// 1. 创建计时器
// 2. 维护空闲和工作中的计时器集合
// 3. 更新工作中的计时器
// 4. 计时器工作完成后，回收到空闲计时器集合
public class TimerManager : Singleton<TimerManager> {
    [SerializeField] private int _initTimerCount;

    private Queue<GameTimer> _notWorkedTimer = new Queue<GameTimer>(); // 空闲计时器
    private List<GameTimer> _workingTimer = new List<GameTimer>(); // 工作中的计时器


    private void Start() {
        InitTimer();
    }

    private void Update() {
        UpdateWorkingTimer();
    }

    private void InitTimer() {
        for (int i = 0; i < _initTimerCount; i++) {
            CreateTimer();
        }
    }

    private void CreateTimer() {
        var timer = new GameTimer();
        _notWorkedTimer.Enqueue(timer);
    }

    public void TryStartOneTimer(float time, Action task) {
        if (_notWorkedTimer.Count == 0) {
            CreateTimer(); // 如果没有空闲的计时器，就再创建一个
        }

        var timer = _notWorkedTimer.Dequeue();
        timer.Start(time, task);
        _workingTimer.Add(timer);
    }

    private void UpdateWorkingTimer() {
        if (_workingTimer.Count == 0) return;

        for (int i = 0; i < _workingTimer.Count; i++) {
            var timer = _workingTimer[i];
            if (timer.GetTimerState() == TimerState.WORKING) {
                timer.Update();
            } else {
                // 状态为 Done 了 
                _workingTimer.Remove(timer);
                _notWorkedTimer.Enqueue(timer);
                timer.Reset();
            }
            
        }
    }
}