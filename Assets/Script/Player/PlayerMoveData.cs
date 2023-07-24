using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Player Move Data")]

public class PlayerMoveData : ScriptableObject
{
    [Header("Gravity")]
    [HideInInspector] public float GravityStrength;
    [HideInInspector] public float GravityScale;
    [Space(5)]
    public float FallGravityMultiplier;
    public float MaxFallSpeed;
    [Space(5)]
    public float MaxFallGravityMultiplier;
    public float MaxFastFallSpeed;
    [Space(20)]

    [Header("Run")]
    public float RunMaxSpeed;
    public float RunAcceleration;
    [HideInInspector] public float RunAccelerationAmount;
    public float RunDecceleration;
    [HideInInspector]public float RunDeccelerationAmount;
    [Space(5)]
    [Range(0f, 1f)] public float RunAccelerationInAirBorne;
    [Range(0f, 1f)] public float RunDeccelerationInAirBorne;
    [Space(5)]
    public bool DoConseverMomentum = true;

    [Space(20)]

    [Header("Jump")]
    public float JumpHeight;
    public float JumpTimeToApex;
    [HideInInspector] public float JumpForce;

    [Header("BothJump")]
    public float JumpCutGravityMultiplier;
    [Range(0f, 1f)] public float JumpHangGraviyMultiplier;
    public float JumpHangTimeThreshold;
    [Space(5)]
    public float JumHangAccelerationMultiplier;
    public float JumHangMaxSpeedMultiplier;

    [Space(20)]

    [Header("WallJump")]
    public Vector2 WallJumpForce;
    [Space(5)]
    [Range(0f, 1f)] public float WallJumpRunLerp;
    [Range(0f, 1f)] public float WallJumpTime;
    public bool DoTurnOnWallJump;

    [Space(20)]

    [Header("Cling")]
    public float WallHangingTimeAllowed;

    [Space(20)]
    [Header("Climp")]
    public float ClimpUpSpeed;
    public float ClimpDownSpeed;

    [Space(20)]
    [Header("Slide")]
    public float SlideSpeed;
    public float SlideAcceleration;

    [Space(20)]
    [Header("Assists")]
    [Range(0.01f, 0.5f)] public float Coyotime;
    [Range(0.01f, 0.5f)] public float JumpInputBufferTime;
    [Range(0.01f, 0.5f)] public float ClingInputBufferTime;
    [Range(0.01f, 0.5f)] public float DashInputBufferTime;

    [Space(20)]

    [Header("Dash")]
    public int DashAmount;
    public float DashSpeed;
    public float DashSleepTime;
    [Space(5)]
    public float DashAttackTime;
    public float DashEndTime;
    public float DashEndSpeed;
    [Range(0f, 1f)] public float DashEndRunLerp;
    [Space(5)]
    public float DashRefillTime;

    private void OnValidate()
    {
        GravityStrength = -(2*JumpHeight)/Mathf.Pow(JumpTimeToApex, 2);
        GravityScale = GravityStrength / Physics2D.gravity.y;

        RunAccelerationAmount = ((1 / Time.fixedDeltaTime) * RunAcceleration) / RunMaxSpeed;
        RunDeccelerationAmount = ((1 / Time.fixedDeltaTime) *RunDecceleration) / RunMaxSpeed;

        JumpForce = Mathf.Abs(GravityStrength)  *JumpTimeToApex;

        RunAcceleration = Mathf.Clamp(RunAcceleration, 0.01f, RunMaxSpeed);
        RunDecceleration = Mathf.Clamp(RunDecceleration, 0.01f, RunMaxSpeed);
    }
}
