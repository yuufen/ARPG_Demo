using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 角色、敌人移动控制基类
namespace DEMO.Movement {
    [RequireComponent(typeof(CharacterController))]
    public abstract class CharacterMovementControlBase : MonoBehaviour {
        protected CharacterController _control;
        protected Animator _animator;

        protected Vector3 _moveDirection;

        // 地面检测
        protected bool _characterIsOnGround; // 角色是否在地面上
        [SerializeField, Header("地面检测")] protected float _groundDetectionPositionOffset;
        [SerializeField] protected float DetectionRange;
        [SerializeField] protected LayerMask _whatIsGround;

        // 重力
        [Header("重力")] protected float _falloutTime = 0.15f; // 掉落超过 0.15s 后再播放掉落动画，防止下小台阶时鬼畜。
        protected float _falloutDeltaTime; // 用来记录单次掉落是否超过 _falloutTime，在地面上时重置回 _falloutTime
        protected readonly float CharacterGravity = -9.8f;
        protected float _characterVerticalVelocity; // 角色当前垂直方向速度
        protected readonly float _characterVerticalMaxVelocity = 54f; // 角色最大垂直方向速度

        protected Vector3 _characterVerticalDirection; // 角色三轴移动速度

        protected bool _isGravityEnable;

        // 虚方法，就是可以被子类重写的方法
        protected virtual void Awake() {
            _control = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
        }

        protected virtual void OnEnable() {
            GameEventManager.MainInstance.AddListener<bool>("EnableCharacterGravity", SetIsGravityEnable);
        }

        protected virtual void OnDisable() {
            GameEventManager.MainInstance.RemoveListener<bool>("EnableCharacterGravity", SetIsGravityEnable);
        }

        protected void Start() {
            _falloutDeltaTime = _falloutTime; // 初始化
            _isGravityEnable = true;
        }

        private void Update() {
            SetCharacterGravity();
            UpdateCharacterGravity();
        }

        // 用于修改 Animator 根运动处理 transform 的逻辑。这里通过 RootMotion 驱动角色移动
        protected virtual void OnAnimatorMove() {
            // 声明 OnAnimatorMove 方法之后默认的 RootMotion 将不应用在 transform 上
            // 所以这里手动应用默认的 Root Motion
            _animator.ApplyBuiltinRootMotion();
            // 然后将 RootMotion 移动量经过处理后传入 CharacterController 的 Move 方法，通过控制器的 Move 修改 Transform
            // TODO 但是控制器的 Move 方法并没有生效，因为 Builtin RootMotion 会覆盖掉 transform
            // TODO 所以应该去改 RootMotion 的 Move 逻辑
            // TODO 但是下面重力的 Move 能生效，可能是因为 RootMotion 只有 xz 的移动
            // UpdateCharacterMoveDirection(_animator.deltaPosition);
        }

        // 地面检测
        private bool GroundDetection() {
            var position = transform.position;
            // 检测球的中心
            var detectionPosition = new Vector3(position.x, position.y - _groundDetectionPositionOffset, position.z);
            // 检测球和地面的碰撞
            return Physics.CheckSphere(detectionPosition, DetectionRange, _whatIsGround,
                QueryTriggerInteraction.Ignore);
        }

        // 重力
        private void SetCharacterGravity() {
            _characterIsOnGround = GroundDetection();

            if (_characterIsOnGround) {
                // 重置对应属性
                _falloutDeltaTime = _falloutTime;
                if (_characterVerticalVelocity < 0f) {
                    _characterVerticalVelocity = -2f; // 初始下落速度 -2
                }
            } else {
                // 下落动画
                if (_falloutDeltaTime > 0f) {
                    _falloutDeltaTime -= Time.deltaTime;
                } else {
                    // 播放下落动画
                }

                // 重力加速
                if (_characterVerticalVelocity < _characterVerticalMaxVelocity && _isGravityEnable) {
                    _characterVerticalVelocity += CharacterGravity * Time.deltaTime;
                }
            }
        }

        private void UpdateCharacterGravity() {
            if (!_isGravityEnable) return;

            // 重力模块只负责垂直方向移动
            _characterVerticalDirection.Set(0, _characterVerticalVelocity, 0);
            _control.Move(_characterVerticalDirection * Time.deltaTime);
        }

        // 斜坡处理，使角色贴着斜坡平移
        private Vector3 SlopResetDirection(Vector3 moveDirection) {
            if (
                Physics.Raycast(
                    transform.position + transform.up * 0.5f, // transform.position 在脚上，可能在地面下，所以加 0.5*up
                    Vector3.down, // todo 应该是 -transform.up
                    out var hit,
                    _control.height * 0.85f,
                    _whatIsGround,
                    QueryTriggerInteraction.Ignore
                )
            ) {
                if (Vector3.Dot(Vector3.up, hit.normal) != 0) {
                    // 如果身下是地面且地面不是完全垂直，就把 moveDirection 设置为 moveDirection 在地面上的投影
                    // 相当于把 moveDirection 垂直于地面的运动去掉，垂直于地面的运动由重力模块负责
                    // 会导致斜坡上速度变慢，更完善的做法是 catlikecoding 的做法，创建斜坡坐标系
                    moveDirection = Vector3.ProjectOnPlane(moveDirection, hit.normal);
                }
            }

            return moveDirection;
        }


        protected void UpdateCharacterMoveDirection(Vector3 direction) {
            _moveDirection = SlopResetDirection(direction);
            _control.Move(_moveDirection * Time.deltaTime);
        }

        // 设置 _isGravityEnable
        private void SetIsGravityEnable(bool enable) {
            _isGravityEnable = enable;
            _characterVerticalVelocity = enable ? -2f : 0f;
        }

        void OnDrawGizmos() {
            // 绘制地面检测范围
            var position = transform.position;
            var detectionPosition = new Vector3(position.x, position.y - _groundDetectionPositionOffset, position.z);
            Gizmos.DrawWireSphere(detectionPosition, DetectionRange);
        }
    }
}