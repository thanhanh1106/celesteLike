using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    PlayerController player;
    public float Force;
    Animator animator;
    AnimationController animationController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animationController = new AnimationController(animator);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameConst.PLAYER_TAG))
        {
            player = collision.gameObject.GetComponent<PlayerController>();
            player.LastOnGroundTime = 0;
            player.LastPressedJumpTime = 0;
            player.Rb.velocity = Vector2.zero;
            player.IsJumping = true;
            player.isJumpFalling = false;
            player.isJumpCut = false;
            player.IsWallJumping = false;
            player.Rb.AddForce(Vector2.up * Force,ForceMode2D.Impulse);
            animationController.PlayOnShotAnimation("Bouncing");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag(GameConst.PLAYER_TAG))
            
            
    }
}
