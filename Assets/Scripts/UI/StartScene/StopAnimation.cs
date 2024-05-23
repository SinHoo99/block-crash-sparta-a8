using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAnimation : MonoBehaviour
{
    public Animator animator;
    private float animSpeed;

    public void AnimStop()
    {
        animator.StopPlayback();
        animSpeed = animator.speed;
        animator.speed = 0f;
        //animator.SetFloat("speed", 0);
    }

    public void AnimStart()
    {
        animator.StartPlayback();
        //animator.speed = animSpeed;
        animator.SetFloat("speed", animSpeed);
    }
}
