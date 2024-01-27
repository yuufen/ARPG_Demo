using System;
using System.Collections;
using System.Collections.Generic;
using DEMO.Tool;
using UnityEngine;

public class TP_CameraControl : MonoBehaviour {
    [Header("相机参数配置")]
    [SerializeField] private Vector2 _cameraVerticalAngleMinMax; // 相机垂直角度限制

    [SerializeField] private float _controlSpeed; // 相机旋转速度
    [SerializeField] private float _rotationSmoothTime; // 相机旋转 SmoothDamp 的 smoothTime

    private Transform _lookTarget; // 相机 Target
    [SerializeField] private float _positionOffset; // 相机中心点偏移 // todo 整成 3 维的，三个方向偏移
    [SerializeField] private float _positionSmoothTime; // 相机距离

    private Vector2 _input;
    private Vector3 _cameraRotation;
    private Vector3 _smoothDampVelocity;

    private void Awake() {
        _lookTarget = GameObject.FindWithTag("CameraTarget").transform;
    }

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        CameraInput();
    }

    private void LateUpdate() {
        UpdateCameraRotation();
        UpdateCameraPosition();
    }

    private void CameraInput() {
        _input.y += GameInputManager.MainInstance.CameraLook.x * _controlSpeed; // 水平
        _input.x -= GameInputManager.MainInstance.CameraLook.y * _controlSpeed; // 垂直
        _input.x = Mathf.Clamp(_input.x, _cameraVerticalAngleMinMax.x, _cameraVerticalAngleMinMax.y);
    }

    private void UpdateCameraRotation() {
        _cameraRotation = Vector3.SmoothDamp(_cameraRotation, new Vector3(_input.x, _input.y, 0f),
            ref _smoothDampVelocity, _rotationSmoothTime);
        transform.eulerAngles = _cameraRotation;
    }

    private void UpdateCameraPosition() {
        var newPosition = _lookTarget.position + -transform.forward * _positionOffset;
        transform.position = Vector3.Lerp(transform.position, newPosition,
            DevelopmentTools.UnTetheredLerp(_positionSmoothTime));
    }
}