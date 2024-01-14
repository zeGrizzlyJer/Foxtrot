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
    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 boxSize;

    public int maxJumps = 2;
    private bool isGrounded;
    private int jumps;

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
        pInput.input.Player.Jump.performed += ctx => StartJump(ctx);
        pInput.input.Player.ScreenGrab.performed += ctx => ScreenGrab(ctx);
    }

    #region Cleanup
    public void OnDisable()
    {
        Debug.Log($"{name}: Unsubscribing in progress...");
        pInput.input.Player.Jump.performed -= ctx => StartJump(ctx);
        pInput.input.Player.ScreenGrab.performed -= ctx => ScreenGrab(ctx);
        GameManager.Instance.OnGameStateChanged -= DeterminePlayerState;
        GameManager.Instance.OnApplicationCleanup -= OnCleanup;
    }

    public void OnCleanup()
    {
        if (!GameManager.cleanedUp) OnDisable();
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
        if (rb.velocity.y > 0.01f)
        {
            state = State.Jump;
            return;
        }
        else if (rb.velocity.y < 0.01f)
        {
            state = State.Fall;
            return;
        }
    }

    protected override void Jump()
    {
        CheckIfGrounded();
        pAnim.Jump();
        if (rb.velocity.y < 0.01f)
        {
            state = State.Fall;
            return;
        }
    }

    protected override void Fall()
    {
        CheckIfGrounded();
        pAnim.Fall();
        if (rb.velocity.y > 0.01f)
        {
            state = State.Jump;
            return;
        }
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
        rb.AddForce(new Vector3(0f, jumpForce, 0f));
        Debug.Log("Jumping");
    }

    public void Bounce()
    {
        rb.AddForce(new Vector3(0f, jumpForce * 1.5f, 0f));
        Debug.Log("Bouncing");
    }

    private void CheckIfGrounded()
    {
        Collider2D groundCollider = Physics2D.OverlapBox(transform.position, boxSize, 0f, groundLayer);

        if (groundCollider != null)
        {
            Grounded();
            return;
        }
        else
        {
            isGrounded = false;
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
