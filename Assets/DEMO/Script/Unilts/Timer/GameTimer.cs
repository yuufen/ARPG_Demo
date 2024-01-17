using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimerState {
    NOTWORKED, // 没有工作
    WORKING, // 进行中
    DONE // 完成
}

// 计时器
public class GameTimer {
    private float _time; // 计时时长
    private Action _task; // 计时结束后执行的任务
    private bool _isTimerStopped; //是否停止当前计时器
    private TimerState _timerState; // 当前状态

    public GameTimer() {
        Reset();
    }

    public TimerState GetTimerState() => _timerState;

    public void Start(float time, Action task) {
        _time = time;
        _task = task;
        _isTimerStopped = false;
        _timerState = TimerState.WORKING;
    }

    public void Update() {
        if (_isTimerStopped) return;

        _time -= Time.deltaTime;

        if (_time < 0f) {
            _task?.Invoke();
            _timerState = TimerState.DONE;
            _isTimerStopped = true;
        }
    }

    public void Reset() {
        _time = 0f;
        _task = null;
        _isTimerStopped = true;
        _timerState = TimerState.NOTWORKED;
    }
}