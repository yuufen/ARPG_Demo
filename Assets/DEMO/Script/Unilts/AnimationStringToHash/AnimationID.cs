using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationID {
    public static readonly int MovementId = Animator.StringToHash("Movement");
    public static readonly int LockId = Animator.StringToHash("Lock");
    public static readonly int HorizontalId = Animator.StringToHash("Horizontal");
    public static readonly int VerticalId = Animator.StringToHash("Vertical");
    public static readonly int HasInputId = Animator.StringToHash("HasInput");
    public static readonly int RunId = Animator.StringToHash("Run");
}