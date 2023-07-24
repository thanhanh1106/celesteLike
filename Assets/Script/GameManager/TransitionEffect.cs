using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEffect : Singleton<TransitionEffect>
{
    [SerializeField] Animator animator;
    public void CallTransitionEffect(float timeLoad)
    {
        StartCoroutine(Load(timeLoad));
    }
    IEnumerator Load(float time)
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("End");
    }
}
