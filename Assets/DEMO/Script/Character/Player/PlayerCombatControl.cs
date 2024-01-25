using System;
using System.Collections;
using System.Collections.Generic;
using DEMO.Combo;
using DEMO.Tool;
using UnityEngine;

namespace DEMO.Combat {
    public class PlayerCombatControl : MonoBehaviour {
        private Animator _animator;

        [SerializeField, Header("角色组合技")] private CharacterComboSO _baseCombo; // 基础连招

        private CharacterComboSO _currentCombo; // 当前连招
        private int _currentComboIndex; // 当前连招的招式索引
        private float _currentColdTime; // 当前招式冷却时间
        private int _currentHitIndex; // 当前招式的受击动画索引

        private bool _canAttack; // 是否能进行攻击，用来处理冷却时间


        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        private void Start() {
            _canAttack = true;
            _currentCombo = _baseCombo; // 默认基础连招
        }

        private void Update() {
            CharacterBaseAttackInput();
        }


        #region BaseCombo

        // 是否能进行 Base Combo
        private bool CanBaseAttack() {
            if (
                /* 冷却中 */ !_canAttack ||
                /* 正在挨揍 */ _animator.AnimationAtTag("Hit") ||
                /* 正在格挡 */ _animator.AnimationAtTag("Parry")
                /* 正在处决...... */
            ) {
                return false;
            }

            return true;
        }

        private void CharacterBaseAttackInput() {
            if (!CanBaseAttack()) return;

            if (GameInputManager.MainInstance.LAttack) {
                // 将当前连招切换为基础连招
                if (_currentCombo != _baseCombo) {
                    _currentCombo = _baseCombo;
                    ResetComboInfo();
                }

                ExecuteComboAction();
            }
        }

        private void ExecuteComboAction() {
            // 重置招式 Info
            _currentHitIndex = 0;

            // 执行到连招最后招式时，重头连招
            if (_currentComboIndex == _currentCombo.TryGetComboMaxCount()) {
                _currentComboIndex = 0;
            }

            _currentColdTime = _currentCombo.TryGetComboColdTime(_currentComboIndex);

            _animator.CrossFadeInFixedTime(_currentCombo.TryGetOneComboAction(_currentComboIndex), 0.15555f, 0, 0f);

            TimerManager.MainInstance.TryStartOneTimer(_currentColdTime, UpdateComboInfo);
            _canAttack = false; // 冷却期间禁止攻击
        }

        private void UpdateComboInfo() {
            _currentComboIndex++;

            _currentColdTime = 0f;
            _canAttack = true;
        }

        private void ResetComboInfo() {
            _currentComboIndex = 0;
            _currentHitIndex = 0;
            _currentColdTime = 0f;
        }

        #endregion
    }
}