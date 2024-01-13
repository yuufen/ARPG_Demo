using System;
using System.Collections;
using System.Collections.Generic;
using DEMO.Tool;
using UnityEngine;

public class CameraCollider : MonoBehaviour {
    [SerializeField, Header("最小/最大距离")] private Vector2 _cameraDistanceMinMax;
    [SerializeField, Header("检测 Layer")] private LayerMask _detectionLayer;
    [SerializeField, Header("检测射线长度")] private float _detectionDistance;
    [SerializeField, Header("碰撞移动平滑时间")] private float _colliderSmoothTime;

    // 当前距离
    private float _currentOffsetDistance;

    private Transform _mainCamera;

    private void Awake() {
        _mainCamera = Camera.main.transform;
    }

    private void Start() {
        _currentOffsetDistance = _cameraDistanceMinMax.y;
    }

    private void LateUpdate() {
        UpdateCameraLocalPositionWithCollider();
    }

    private void UpdateCameraLocalPositionWithCollider() {
        var detectionEnd = transform.TransformPoint(new Vector3(0, 0, -1) * _detectionDistance);

        if (Physics.Linecast(transform.position, detectionEnd, out var hit, _detectionLayer,
                QueryTriggerInteraction.Ignore)) {
            // *0.8 防止近平面穿模
            _currentOffsetDistance = Mathf.Clamp(hit.distance * 0.8f, _cameraDistanceMinMax.x, _cameraDistanceMinMax.y);
        } else {
            _currentOffsetDistance = _cameraDistanceMinMax.y;
        }

        _mainCamera.localPosition = Vector3.Lerp(_mainCamera.localPosition,
            new Vector3(0, 0, -1) * (_currentOffsetDistance - 0.1f), // -0.1 防止近平面穿模
            DevelopmentToos.UnTetheredLerp(_colliderSmoothTime));
    }

    void OnDrawGizmos() {
        var detectionEnd = transform.TransformPoint(new Vector3(0, 0, -1) * _detectionDistance);
        Gizmos.DrawLine(transform.position, detectionEnd);
    }
}