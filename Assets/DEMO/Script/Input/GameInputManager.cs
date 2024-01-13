using System;
using System.Collections;
using System.Collections.Generic;
using DEMO.Tool.Singleton;
using UnityEngine;

public class GameInputManager : Singleton<GameInputManager> {
    private GameInputAction _gameInputAction;

    public Vector2 Movement => _gameInputAction.GameInput.Movement.ReadValue<Vector2>();
    public Vector2 CameraLook => _gameInputAction.GameInput.CameraLook.ReadValue<Vector2>();
    public bool Run => _gameInputAction.GameInput.Run.triggered;

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