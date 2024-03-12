using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class PlayerController : EntityController, IRequireCleanup
{
    private Rigidbody2D rb;
    private PlayerInput pInput;
    private PlayerAnim pAnim;
    [SerializeField] private float initialJumpForce = 10f;
    [SerializeField] private float finalJumpForce = 2f;
    [SerializeField] private float maxFallSpeed = 15f;
    [SerializeField] private float maxJumpTimer = 1f;
    [SerializeField] private float gravityStrength = 30f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 boxSize;

    public int maxJumps = 2;
    private bool isGrounded;
    private int jumps;
    private bool isJumping;
    private float jumpTimer = 0f;

    private void Awake()
    {
        GameManager.Instance.OnGameStateChanged += DeterminePlayerState;
        GameManager.Instance.OnApplicationCleanup += OnCleanup;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pInput = GetComponent<PlayerInput>();
        pAnim = GetComponentInChildren<PlayerAnim>();

        Physics2D.gravity = new Vector2(0f, -gravityStrength);

        pInput.input.Player.Jump.started += ctx => StartJump(ctx);
        pInput.input.Player.Jump.canceled += ctx => EndJump(ctx);
        pInput.input.Player.ScreenGrab.performed += ctx => ScreenGrab(ctx);
    }
    
    #region Cleanup
    public void OnDisable()
    {
        if (!GameManager.cleanedUp) OnCleanup();
    }


    public void OnCleanup()
    {
        Debug.Log($"{name}: Unsubscribing in progress...");
        pInput.input.Player.Jump.performed -= ctx => StartJump(ctx);
        pInput.input.Player.ScreenGrab.performed -= ctx => ScreenGrab(ctx);
        GameManager.Instance.OnGameStateChanged -= DeterminePlayerState;
        GameManager.Instance.OnApplicationCleanup -= OnCleanup;
    }
    #endregion

    #region State Functions
    private void DeterminePlayerState()
    {
        switch (GameManager.Instance.GameState)
        {
            case GameState.GAMESTART:
                state = State.Idle;
                break;
            case GameState.PLAY:
                rb.gravityScale = 1f;
                state = State.Run;
                break;
            case GameState.RESPAWN:
                state = State.Injured;
                break;
            default:
                break;
        }
    }

    protected override void Idle()
    {
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        pAnim.Idle();
    }

    protected override void Run()
    {
        pAnim.Run();
        CheckIfGrounded();
        if (isGrounded) return;
        /*        if (rb.velocity.y > 0.01f)
                {
                    state = State.Jump;
                    return;
                }*/
        DetermineIfFalling();
    }

    protected override void Jump()
    {
        CheckIfGrounded();
        pAnim.Jump();
        DetermineIfFalling();
        UpdateJump();
    }

    protected override void Fall()
    {
        CheckIfGrounded();
        pAnim.Fall();
        UpdateFalling();
        /*if (rb.velocity.y > 0.01f)
        {
            state = State.Jump;
            return;
        }*/
    }

    protected override void Injured()
    {
        pAnim.Injured();
    }
    #endregion

    public void StartJump(InputAction.CallbackContext ctx)
    {
        if (jumps <= 0) return;

        jumps--;
        state = State.Jump;
        isJumping = true;
        jumpTimer = 0f;
        rb.velocity = new Vector2(0f, initialJumpForce);
        Debug.Log("Jumping");
    }

    public void EndJump(InputAction.CallbackContext ctx)
    {
        if (!isJumping) return;

        isJumping = false;
        rb.velocity = new Vector2(0f, finalJumpForce);
        Debug.Log("End Jumping");
    }

    private void UpdateJump()
    {
        if (!isJumping) return;
        if (jumpTimer > maxJumpTimer)
        {
            isJumping = false;
            rb.velocity = new Vector2(0f, 0f);
            return;
        }

        float t = jumpTimer / maxJumpTimer;
        float jumpForce = Mathf.Lerp(initialJumpForce, finalJumpForce, t) * JumpCurve(t);

        rb.velocity = new Vector2(0f, jumpForce);

        jumpTimer += Time.deltaTime;
    }

    private float JumpCurve(float t)
    {
        return Mathf.Pow(1.5f, t);
    }

    public void Bounce()
    {
        rb.velocity = new Vector2(0f, initialJumpForce * 1.2f);
        state = State.Jump;
        Debug.Log("Bouncing");
    }

    private void CheckIfGrounded()
    {
        Collider2D groundCollider = Physics2D.OverlapBox(transform.position, boxSize, 0f, groundLayer);

        if (groundCollider != null && (state == State.Fall || state == State.Run))
        {
            Grounded();
            return;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void DetermineIfFalling()
    {
        if (rb.velocity.y < -0.01f)
        {
            state = State.Fall;
            return;
        }
    }

    private void UpdateFalling()
    {
        if (rb.velocity.y < -maxFallSpeed)
        {
            rb.velocity = new Vector2(0, -maxFallSpeed);
        }
    }

    public void Grounded()
    {
        isGrounded = true;
        jumps = maxJumps;
        state = State.Run;
    }

    public void EnableRB()
    {
        rb.simulated = true;
        rb.velocity = Vector3.zero;
    }

    public void DisableRB()
    {
        rb.simulated = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
    public void ScreenGrab(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.ReadValue<Vector2>());
    }
}
