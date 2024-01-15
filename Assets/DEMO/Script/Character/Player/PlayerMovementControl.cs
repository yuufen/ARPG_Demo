using System;
using System.Collections;
using System.Collections.Generic;
using DEMO.Tool;
using UnityEngine;

namespace DEMO.Movement {
    public class PlayerMovementControl : CharacterMovementControlBase {
        private float _rotationAngle;

        private float _rotateVelocity = 0f;
        [SerializeField] private float _rotateSmoothTime;

        private Transform _mainCamera;

        protected override void Awake() {
            base.Awake();
            _mainCamera = Camera.main.transform;
        }


        private void LateUpdate() {
            UpdateAnimation();
            CharacterRotationControl();
        }


        private void UpdateAnimation() {
            if (!_characterIsOnGround) return;

            _animator.SetBool(AnimationID.HasInputId, GameInputManager.MainInstance.Movement != Vector2.zero);

            if (_animator.GetBool(AnimationID.HasInputId)) {
                if (GameInputManager.MainInstance.Run) {
                    _animator.SetBool(AnimationID.RunId, true);
                }
                
                // 手柄没按奔跑，速度就是摇杆输入值，按了奔跑就锁定 2
                _animator.SetFloat(
                    AnimationID.MovementId,
                    _animator.GetBool(AnimationID.RunId) ? 2f : GameInputManager.MainInstance.Movement.sqrMagnitude,
                    0.25f,
                    Time.deltaTime
                );
            } else {
                _animator.SetFloat(
                    AnimationID.MovementId,
                    0f,
                    0.25f,
                    Time.deltaTime
                );
                if (_animator.GetFloat(AnimationID.MovementId) < 0.2f) {
                    // 当 Movement 减小快到 0 再取消 Run 状态，因为播放“停止移动”动画的时候需要区分是不是 Run
                    // 说实话这里不取消 Run 也没啥事 
                    _animator.SetBool(AnimationID.RunId, false);
                }
            }
        }


        // 只控制旋转，位移由 RootMotion 驱动
        private void CharacterRotationControl() {
            if (!_characterIsOnGround) return;

            var hasInput = _animator.GetBool(AnimationID.HasInputId);

            if (hasInput) {
                _rotationAngle = _mainCamera.eulerAngles.y + // 当按 W 时，角色旋转角度和相机旋转角度一致
                                 Mathf.Rad2Deg * Mathf.Atan2(
                                     GameInputManager.MainInstance.Movement.x,
                                     GameInputManager.MainInstance.Movement.y
                                 );
            }

            if (hasInput && _animator.AnimationAtTag("Motion")) {
                // Motion 状态下才旋转角色（不然被打了还能旋转）
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, _rotationAngle,
                    ref _rotateVelocity, _rotateSmoothTime);
            }
        }
    }
}