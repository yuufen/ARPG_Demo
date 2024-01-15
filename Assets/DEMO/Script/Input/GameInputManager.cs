using System;
using System.Collections;
using System.Collections.Generic;
using DEMO.Tool.Singleton;
using UnityEngine;

// 封装一遍 Input Action
public class GameInputManager : Singleton<GameInputManager> {
    private GameInputAction _gameInputAction;

    public Vector2 Movement => _gameInputAction.GameInput.Movement.ReadValue<Vector2>();
    public Vector2 CameraLook => _gameInputAction.GameInput.CameraLook.ReadValue<Vector2>();
    public bool Run => _gameInputAction.GameInput.Run.triggered;
    public bool Climb => _gameInputAction.GameInput.Climb.triggered;
    public bool LAttack => _gameInputAction.GameInput.LAttack.triggered;
    public bool RAttack => _gameInputAction.GameInput.RAttack.triggered;
    public bool Grab => _gameInputAction.GameInput.Grab.triggered;
    public bool TakeOut => _gameInputAction.GameInput.TakeOut.triggered;


    private void Awake() {
        base.Awake();
        _gameInputAction ??= new GameInputAction();
    }

    private void OnEnable() {
        _gameInputAction.Enable();
    }

    private void OnDisable() {
        _gameInputAction.Disable();
    }
}