using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public string currentAnimation = "";
    Animator animator;
    public AnimationController (Animator animator)
    {
        this.animator = animator;
    }
    public void ChangeAnimationState(string nameAnimation)
    {
        if (nameAnimation == currentAnimation) return;
        animator.Play(nameAnimation);
        currentAnimation = nameAnimation;
    }
    public void PlayOnShotAnimation(string nameAnimation)
    {
        animator.Play(nameAnimation);
    }
}
