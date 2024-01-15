using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 动画播放过程中，每帧调整 GameObject 的 Position 和 Rotation
// 使动画能和场景匹配 
public class AnimationMatchSMB : StateMachineBehaviour {
    // 参数的 Time 都是 Normalized Time，0 表示动画开头，1 表示动画结尾。
    [SerializeField, Header("Match 信息")] private float _startTime; // Match 开始时间
    [SerializeField] private float _endTime; // Match 完成时间
    [SerializeField] private AvatarTarget _avatarTarget; // Match 的目标部位（左右手、脚、身体）

    [SerializeField, Header("是否激活重力")] private bool _enableGravity;
    [SerializeField] private float _enableTime;


    private Vector3 _matchPosition;
    private Quaternion _matchRotation;

    private void OnEnable() {
        // TODO 多个实例咋办？明显设计有问题
        GameEventManager.MainInstance.AddListener<Vector3, Quaternion>("SetAnimationMatchInfo", GetMatchInfo);
    }

    private void OnDisable() {
        GameEventManager.MainInstance.RemoveListener<Vector3, Quaternion>("SetAnimationMatchInfo", GetMatchInfo);
    }


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // Called on each Update frame between OnStateEnter and OnStateExit
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (!animator.isMatchingTarget) {
            // _startTime 时开始匹配，
            // 每帧调整 GameObject 的 position 和 rotation，
            // 使得 _endTime 时，_avatarTarget 到达 _matchPosition 和 _matchRotation
            animator.MatchTarget(
                _matchPosition, _matchRotation, _avatarTarget,
                new MatchTargetWeightMask(Vector3.one, 0f),
                _startTime, _endTime
            );
        }

        if (_enableGravity) {
            // 当动画的当前播放时间 > _enableTime 时，激活重力
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > _enableTime) {
                GameEventManager.MainInstance.CallEvent("EnableCharacterGravity", true);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    private void GetMatchInfo(Vector3 position, Quaternion rotation) {
        _matchPosition = position;
        _matchRotation = rotation;
    }
}