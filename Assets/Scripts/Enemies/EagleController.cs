using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EagleController : EntityController, IRequireCleanup, IEagle
{
    private Rigidbody2D rb;
    private EagleSpawner spawner;
    private EagleAnim eAnim;
    private GameObject player;
    [SerializeField] private AnimationClip injureClip;

    [SerializeField] private float verticalSpeed = 3f;
    [SerializeField] private float horizontalSpeedRate = 0.5f;
    [SerializeField] private float despawnPoint = -10f;
    [SerializeField] private int defeatValue = 1000;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        eAnim = GetComponentInChildren<EagleAnim>();
    }

    private void DetermineEntityState()
    {
        Debug.Log($"{name} is determining state: " + GameManager.Instance.GameState);
        switch (GameManager.Instance.GameState)
        {
            case GameState.END:
            case GameState.GAMESTART:
            case GameState.PAUSE:
                state = State.Idle;
                break;
            case GameState.RESPAWN:
            case GameState.PLAY:
                state = State.Run;
                break;
            default:
                break;
        }
    }

    protected override void Idle()
    {
        Debug.Log("Idle");
        rb.velocity = Vector3.zero;
        eAnim.Idle();
    }

    protected override void Run()
    {
        Vector3 direction;

        if (player.transform.position.y > transform.position.y)
            direction = new Vector3(-GameManager.Instance.gameSpeed * horizontalSpeedRate, verticalSpeed, 0f);
        else if (player.transform.position.y < transform.position.y)
            direction = new Vector3(-GameManager.Instance.gameSpeed * horizontalSpeedRate, -verticalSpeed, 0f);
        else
            direction = new Vector3(-GameManager.Instance.gameSpeed * horizontalSpeedRate, 0f, 0f);

        rb.velocity = direction;
        if (transform.position.x <= despawnPoint) Despawn();
        eAnim.Run();
    }

    protected override void Injured()
    {
        rb.velocity = Vector3.zero;
        eAnim.Injured();
    }

    public void Spawn(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
        DetermineEntityState();
        GameManager.Instance.OnGameStateChanged += DetermineEntityState;
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
        spawner.ReturnEagle(this);
    }

    public void AssignSpawner(EagleSpawner s)
    {
        spawner = s;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.transform.position.y <= transform.position.y)
        {
            Debug.Log("Death by Eagle");
            GameObject.FindGameObjectWithTag("Respawn").GetComponent<PlayerSpawn>().RespawnPlayer();
            return;
        }
        player.GetComponent<PlayerController>().Bounce();
        state = State.Injured;
        StartCoroutine(DefeatEnemy());
        Debug.Log($"{name} has been hit");
    }

    IEnumerator DefeatEnemy()
    {
        float timeToDespawn = injureClip.length;
        GameManager.Instance.Score += defeatValue; 
        yield return new WaitForSeconds(timeToDespawn);
        Despawn();

    }

    #region Cleanup
    public void OnCleanup()
    {
        if (!GameManager.cleanedUp) OnDisable();
    }

    public void OnDisable()
    {
        Debug.Log($"{name}: Unsubscribing in progress...");
        GameManager.Instance.OnGameStateChanged -= DetermineEntityState;
    }
    #endregion
}
