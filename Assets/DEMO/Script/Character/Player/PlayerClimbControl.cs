using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player 攀爬能力组件
public class PlayerClimbControl : MonoBehaviour {
    private Animator _animator;

    [SerializeField, Header("检测")] private float _detectionDistance;
    [SerializeField] private LayerMask _detectionLayer;

    private RaycastHit _hit;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        CharacterClimbInput();
    }

    private bool CanClimb() {
        return Physics.Raycast(transform.position + transform.up * 0.5f, transform.forward,
            out _hit, _detectionDistance, _detectionLayer, QueryTriggerInteraction.Ignore);
    }


    private void CharacterClimbInput() {
        if (!CanClimb()) return;

        // 按下攀爬键
        if (GameInputManager.MainInstance.Climb) {
            var rotation = Quaternion.LookRotation(-_hit.normal); // 让角色正朝墙壁
            var position = new Vector3(
                _hit.point.x, // xz 坐标即射线 hit 坐标
                _hit.collider.bounds.max.y , // 墙最高处世界y坐标
                _hit.point.z
            );// Match Target Position

            switch (_hit.collider.tag) {
                case "中高墙":
                    //触发事件
                    CallClimbEvent(position, rotation);
                    // 切换状态
                    _animator.CrossFade("爬中高墙", 0f, 0, 0f);
                    break;
                case "高墙":
                    CallClimbEvent(position, rotation);
                    _animator.CrossFade("爬高墙", 0f, 0, 0f);
                    break;
            }
        }
    }

    private void CallClimbEvent(Vector3 position, Quaternion rotation) {
        // 设置 Match Target
        GameEventManager.MainInstance.CallEvent("SetAnimationMatchInfo", position, rotation);
        // 禁用角色重力
        GameEventManager.MainInstance.CallEvent("EnableCharacterGravity", false);
    }

    void OnDrawGizmos() {
        // 绘制墙壁检测射线
        var from = transform.position + transform.up * 0.5f;
        var to = from + transform.forward * _detectionDistance;
        Gizmos.DrawLine(from, to);
    }
}