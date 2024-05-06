using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Move/Move")]
public class MoveSO : ScriptableObject
{
    public AnimatorOverrideController animatorOV;
    public AnimationClip animationClip;
    public float speed;
}
