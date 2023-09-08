using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    public PlayerMoveData Data;
    public Rigidbody2D Rb;

    #region State Parameter
    [HideInInspector] public bool IsDie { get; private set; }
    [HideInInspector] public bool IsFacingRight {  get; private set; }
    [HideInInspector] public bool IsJumping { get; set; }
    [HideInInspector] public bool IsWallJumping { get; set; }
    [HideInInspector] public bool IsDashing { get; private set; }
    [HideInInspector] public bool IsClinging { get; private set; }
    [HideInInspector] public bool IsClimping { get; private set; }
    [HideInInspector] public bool IsSliding { get; private set; }
    [HideInInspector] public bool IsRuning { get; private set; }
    [HideInInspector] public bool IsIdle { get; private set; }
    [HideInInspector] public bool IsFalling { get; private set; }
    [HideInInspector] public bool IsStillStrength => WallHangingTime <= Data.WallHangingTimeAllowed;
    #endregion

    #region Coyote Time Parameter
    public float WallHangingTime { get; private set; }
    public float LastOnGroundTime { get; set; }
    public float LastOnWallTime { get; private set; }
    #endregion

    public bool isJumpCut;
    public bool isJumpFalling;

    float wallJumpStartTime;

    int DashLeft;
    bool DashRefilling;
    Vector2 DashDirection;
    bool isDashAttacking;

    #region Animation 
    enum AnimationName
    {
        Idle,
        Run,
        Jump,
        Fall,
        WallJump,
        Dash,
        Climb,
        Cling,
        DashUp
    }
    Animator animator;
    AnimationName currentAnimation;
    AnimationController animationController;
    #endregion

    Vector2 MoveInput;

    #region Time parameter
    public float LastPressedJumpTime { get; set; }
    public float LastPressedDashTime { get; private set; }
    public float LastPressedClingingTime { get; private set; }
    #endregion

    #region Check Parameter
    [Header("Check")]
    [SerializeField] Transform GroundCheckPoint;
    [SerializeField] Vector2 GroundCheckSize = new Vector2(0.8f,0.2f);
    [Space(5)]
    [SerializeField] Transform WallChellPoint;
    [SerializeField] Vector2 WallCheckSize = new Vector2(0.2f,1.11f);
    #endregion
    #region layer mask
    [SerializeField] LayerMask GroundLayer;
    #endregion

    Vector3 checkPoint;
    

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animationController = new AnimationController(animator);
    }
    private void Start()
    {
        if (Prefs.IsGameEntered)
        {
            transform.position = Prefs.CheckPoint;
        }
        IsFacingRight = true;
        SetGravityScale(Data.GravityScale);
        checkPoint = GameManager.Instance.GetCheckPoint();
    }
    private void Update()
    {
        #region Times
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;

        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;
        LastPressedClingingTime -= Time.deltaTime;
        #endregion

        #region Input
        MoveInput.x = Input.GetAxisRaw("Horizontal");
        MoveInput.y = Input.GetAxisRaw("Vertical");
        
        if(MoveInput.x != 0 && !IsDashing) CheckFace(MoveInput.x);

        if(Input.GetKeyDown(KeyCode.Space)) OnjumpInput();
        if(Input.GetKeyUp(KeyCode.Space)) OnJumpUpInput();
        if (Input.GetKey(KeyCode.L)) OnClingInput();
        if (Input.GetKeyDown(KeyCode.LeftShift)) OnDashInput();
        #endregion

        #region Colision Check
        if (!IsDashing && !IsJumping && !IsWallJumping)
        {
            if (Physics2D.OverlapBox(GroundCheckPoint.position, GroundCheckSize, 0, GroundLayer))
            {
                LastOnGroundTime = Data.Coyotime;
                WallHangingTime = 0;
            }
            if (Physics2D.OverlapBox(WallChellPoint.position, WallCheckSize, 0, GroundLayer))
            {
                LastOnWallTime = Data.Coyotime;
            }
        }
        #endregion

        #region Jump Check
        if (Rb.velocity.y <= 0 && IsJumping)
        {
            IsJumping = false;
            isJumpFalling = true;
        }

        if (IsClinging || IsSliding) IsJumping = false;

        if(LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
            isJumpCut = false;
            isJumpFalling = false;
        }

        if (IsWallJumping && Time.time - wallJumpStartTime > Data.WallJumpTime)
            IsWallJumping = false;

        if (!IsDashing)
        {
            if(CanJump() && LastPressedJumpTime > 0)
            {
                IsJumping = true;
                isJumpFalling = false;
                isJumpCut = false;
                IsWallJumping = false;
                IsIdle = false;
                IsRuning = false;
                Jump();
                AudioController.Instance.PlaySound(AudioController.Instance.Jump);
            }
            else if(CanWallJump() && LastPressedJumpTime > 0)
            {
                IsWallJumping = true;
                IsJumping = false;
                isJumpCut = false;
                isJumpFalling = false;

                wallJumpStartTime = Time.time;
                WallJump();
            }
        }
        #endregion

        #region Slide check
        IsSliding = MoveInput.x != 0 && CanSlide();
        #endregion

        #region Cling check
        if(CanCling() && LastPressedClingingTime > 0)
        {
            IsClinging = true;
            Cling();
        }
        else
        {
            IsClinging = false;
        }
        #endregion

        #region Climb check
        if (CanClimb() && MoveInput.y != 0)
            IsClimping = true;
        else
            IsClimping = false;
        #endregion

        #region Dash check
        if (CanDash() && LastPressedDashTime > 0)
        {
            Sleep(Data.DashSleepTime);
            if(MoveInput != Vector2.zero)
            {
                DashDirection = MoveInput;
            }
            else
            {
                DashDirection = IsFacingRight ? Vector2.right : Vector2.left;
            }
            IsDashing = true;
            IsJumping = false;
            isJumpCut = false;
            IsWallJumping = false;
            StartCoroutine(Dash(DashDirection));
            CameraShake.Instance.ShakeCamera(1.5f, 10f, 0.1f);
            AudioController.Instance.PlaySound(AudioController.Instance.Dash);
        }
        #endregion

        #region Gravity
        if (IsClinging || isDashAttacking || IsSliding) SetGravityScale(0);
        // nếu đang rơi người chơi bấm nút xuống
        else if (Rb.velocity.y < 0 && MoveInput.y < 0)
        {
            SetGravityScale(Data.GravityScale * Data.MaxFallGravityMultiplier);
            // giới hạn tốc độ rơi không cho rơi quá nhanh
            // do giá trị là âm nên max thực ra nó là lấy min
            Rb.velocity = new Vector2(Rb.velocity.x, Mathf.Max(Rb.velocity.y, -Data.MaxFastFallSpeed));
        }
        else if (isJumpCut)
        {
            SetGravityScale(Data.GravityScale * Data.JumpCutGravityMultiplier);
            Rb.velocity = new Vector2(Rb.velocity.x, Mathf.Max(Rb.velocity.y, -Data.MaxFallSpeed));
        }
        // nếu nhảy hoặc rơi chưa đạt vận tốc cài sẵn thì thay đổi gravity
        else if ((IsJumping || isJumpFalling || IsWallJumping) && Mathf.Abs(Rb.velocity.y) < Data.JumpHangTimeThreshold)
        {
            SetGravityScale(Data.JumpHangGraviyMultiplier);
        }
        else if (Rb.velocity.y < 0)
        {
            SetGravityScale(Data.GravityScale * Data.FallGravityMultiplier);
            Rb.velocity = new Vector2(Rb.velocity.x, Mathf.Max(Rb.velocity.y, -Data.MaxFallSpeed));
        }
        else
        {
            SetGravityScale(Data.GravityScale);
        }
        #endregion

        #region Check state
        if (LastOnGroundTime > 0 && MoveInput.x != 0 && !IsDashing && !IsSliding)
        {
            IsRuning = true;
            IsIdle = false;
        }
        else if (LastOnGroundTime > 0 && MoveInput.x == 0 && !IsDashing && !IsSliding)
        {
            IsRuning = false;
            IsIdle = true;
        }
        else
        {
            IsRuning = false;
            IsIdle = false;
        }

        if(LastOnGroundTime < 0 && Rb.velocity.y < 0 && !IsDashing && !IsSliding && !IsClimping)
        {
            IsFalling = true;
        }
        else IsFalling = false;
        #endregion

        #region Animation Handler
        if (IsRuning) 
            animationController.ChangeAnimationState(AnimationName.Run.ToString());

        if(IsIdle) 
            animationController.ChangeAnimationState(AnimationName.Idle.ToString());

        if(IsJumping) 
            animationController.ChangeAnimationState(AnimationName.Jump.ToString());

        if(IsFalling) 
            animationController.ChangeAnimationState(AnimationName.Fall.ToString());

        if((IsClinging || IsSliding) && !IsClimping) 
            animationController.ChangeAnimationState(AnimationName.Cling.ToString());

        if (IsWallJumping) 
            animationController.ChangeAnimationState(AnimationName.WallJump.ToString());

        if(IsClinging && IsClimping) 
            animationController.ChangeAnimationState(AnimationName.Climb.ToString());

        if (IsDashing)
        {
            if (MoveInput == Vector2.up) 
                animationController.ChangeAnimationState(AnimationName.DashUp.ToString());
            else 
                animationController.ChangeAnimationState(AnimationName.Dash.ToString());
        }

        #endregion
    }
    private void FixedUpdate()
    {
        if (!IsDashing)
        {
            if (IsWallJumping)
                Run(Data.WallJumpRunLerp);
            else
                Run(1);
        }
        else
        {
            if (!isDashAttacking)
                Run(Data.DashEndRunLerp);
        }

        if (IsSliding) Slide();
        if(IsClimping) Climb();
    }

    #region Control method
    void Run(float LerpAmout)
    {
        float targetSpeed = MoveInput.x * Data.RunMaxSpeed;
        targetSpeed = Mathf.Lerp(Rb.velocity.x, targetSpeed, LerpAmout);

        float acceleration;
        if (LastOnGroundTime > 0)
            acceleration = Mathf.Abs(targetSpeed) > 0.01f ?
                Data.RunAccelerationAmount : Data.RunDeccelerationAmount;
        else
            acceleration = Mathf.Abs(targetSpeed) > 0.01f ? 
                Data.RunAccelerationAmount*Data.RunAccelerationInAirBorne :
                Data.RunDeccelerationAmount*Data.RunAccelerationInAirBorne;

        if((IsJumping || IsWallJumping || isJumpFalling ) && Mathf.Abs(Rb.velocity.y) < Data.JumpHangTimeThreshold)
        {
            acceleration *= Data.JumHangAccelerationMultiplier;
            targetSpeed *= Data.JumHangMaxSpeedMultiplier;
        }

        float speedDifferent = targetSpeed - Rb.velocity.x;
        float movemet = speedDifferent * acceleration;
        Rb.AddForce(movemet * Vector2.right, ForceMode2D.Force);
    }

    void Jump()
    {
        LastOnGroundTime = 0;
        LastPressedJumpTime = 0;

        float force = Data.JumpForce;
        if(Rb.velocity.y < 0) force -= Rb.velocity.y;

        Rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    void WallJump()
    {
        LastPressedJumpTime = 0;
        LastOnWallTime = 0;

        // kiến người chơi nhanh hết lực bám hơn khi nhảy tường
        WallHangingTime++;

        int direction = IsFacingRight ? -1 : 1;
        Vector2 force = Data.WallJumpForce;
        force.x *= direction;

        if(Mathf.Sign(Rb.velocity.x) != Mathf.Sign(force.x))
            force.x -= Rb.velocity.x; // trái dấu nên thực ra đây là phép cộng
        if(Rb.velocity.y < 0)
            force.y -= Rb.velocity.y;
        Rb.AddForce(force, ForceMode2D.Impulse);

    }

    void Slide()
    {
        if(Rb.velocity.y > 0)
            Rb.AddForce(Vector2.down*Rb.velocity.y, ForceMode2D.Impulse);

        // nhìn có vẻ ngược logic nhưng bên data để slide Speed là âm thì trượt xuống, dương thì trượt lên
        float speedDifferent = Data.SlideSpeed - Rb.velocity.y;

        float movemet = speedDifferent * 1 / Time.fixedDeltaTime;
        Rb.AddForce(movemet*Vector2.up, ForceMode2D.Force);
    }

    void Cling()
    {
        if (!IsClimping)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, 0);
        }
        WallHangingTime += Time.deltaTime;
    }

    void Climb()
    {
        float targetSpeed;
        if (MoveInput.y > 0)
            targetSpeed = MoveInput.y * Data.ClimpUpSpeed;
        else if (MoveInput.y < 0)
            targetSpeed = MoveInput.y * Data.ClimpDownSpeed;
        else 
            targetSpeed = 0;

        float speedDifferent = targetSpeed - Rb.velocity.y;
        float movement = speedDifferent * 1 / Time.fixedDeltaTime;
        Rb.AddForce(movement * Vector2.up, ForceMode2D.Force);
    }

    IEnumerator Dash(Vector2 direction)
    {
        LastOnGroundTime = 0;
        LastPressedDashTime = 0;

        float startTime = Time.time;

        DashLeft--;
        isDashAttacking = true;

        SetGravityScale(0);

        direction.Normalize();
        while(Time.time -  startTime < Data.DashAttackTime)
        {
            Rb.velocity = direction * Data.DashSpeed;
            yield return null;
        }

        startTime = Time.time;
        isDashAttacking = false;
        SetGravityScale(Data.GravityScale);
        Rb.velocity = direction * Data.DashEndSpeed;
        // đợi 1 khoảng thời gian
        while(Time.time - startTime < Data.DashEndTime)
        {
            yield return null;
        }
        IsDashing = false;
    }

    IEnumerator RefillDash()
    {
        DashRefilling = true;
        yield return new WaitForSeconds(Data.DashRefillTime);
        DashLeft = Mathf.Min(Data.DashAmount, DashLeft + 1);
        DashRefilling = false;
    }
    #endregion

    #region Input CallBack
    void OnjumpInput()
    {
        LastPressedJumpTime = Data.JumpInputBufferTime;
    }
    void OnJumpUpInput()
    {
        if(CanJumpCut() || CanWallJumCut() ) isJumpCut = true;
    }
    void OnClingInput()
    {
        LastPressedClingingTime = Data.ClingInputBufferTime;
    }
    void OnDashInput()
    {
        LastPressedDashTime = Data.DashInputBufferTime;
    }
    #endregion

    #region Check
    bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }
    bool CanJumpCut()
    {
        return Rb.velocity.y > 0 && IsJumping;
    }
    bool CanWallJump()
    {
        return LastOnWallTime > 0 && !IsWallJumping && LastOnGroundTime < 0 && IsStillStrength;
    }
    bool CanWallJumCut()
    {
        return Rb.velocity.y > 0 && IsWallJumping;
    }
    bool CanSlide()
    {
        return LastOnGroundTime < 0 && LastOnWallTime > 0 && !IsDashing && !IsClinging;
    }
    bool CanCling()
    {
        return LastOnGroundTime <= 0 && LastOnWallTime > 0 && IsStillStrength && !IsWallJumping && !IsDashing;
    }
    bool CanClimb()
    {
        return LastOnWallTime > 0 && !IsJumping && !IsDashing && IsClinging;
    }
    bool CanDash()
    {
        if(!IsDashing && DashLeft < Data.DashAmount && LastOnGroundTime > 0 && !DashRefilling)
            StartCoroutine(RefillDash());
        return DashLeft > 0;
    }
    #endregion

    #region other method
    public void BonusDash()
    {
        DashLeft = DashLeft = Mathf.Min(Data.DashAmount, DashLeft + 1);
    }
    void CheckFace(float xDirection)
    {
        float eulerAnglesY = xDirection > 0 ? 0 : 180;
        IsFacingRight = xDirection > 0;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, eulerAnglesY, transform.eulerAngles.z);
    }
    void SetGravityScale(float gravityScale)
    {
        Rb.gravityScale = gravityScale;
    }
    void Sleep(float duration)
    {
        StartCoroutine(PerformSleep(duration));
    }
    IEnumerator PerformSleep(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }
    #endregion


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConst.DEATH_ZONE_TAG))
        {
            GameManager.Instance.DeathCount();
            Die();
        }
        if (collision.CompareTag(GameConst.CHECK_POINT_TAG))
        {
            GameManager.Instance.SetCheckPoint(transform.position);
        }
    } 
    void Die()
    {
        IsDie = true;
        transform.parent = null;
        AudioController.Instance.PlaySound(AudioController.Instance.Die);
        //1 cách để người chơi biến mất
        transform.localScale = Vector3.zero;
        StartCoroutine(Respawn());
        TransitionEffect.Instance.CallTransitionEffect(0.8f);
    }
    IEnumerator Respawn()
    {
        SetGravityScale(0);
        Rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(1f);
        SetGravityScale(Data.GravityScale * Data.FallGravityMultiplier);
        transform.position = GameManager.Instance.GetCheckPoint();
        // phóng to hiện lại người chơi
        transform.localScale = new Vector3(1, 1, 1);
        IsDie = false;
    }
}
