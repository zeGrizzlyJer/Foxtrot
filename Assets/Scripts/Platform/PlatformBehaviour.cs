using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformBehaviour : MonoBehaviour
{
    [SerializeField] private float spawnPoint;
    [SerializeField] private float despawnPoint;
    public bool IsStarterPlatform = false;

    private Rigidbody2D rb;
    private PlatformSpawner spawner;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawner = GetComponentInParent<PlatformSpawner>();
    }

    private void Update()
    {
        switch(GameManager.Instance.GameState)
        {
            case GameState.PLAY:
                rb.velocity = new Vector3(-GameManager.Instance.gameSpeed, 0f, 0f);
                break;
            case GameState.GAMESTART:
            case GameState.RESPAWN:
            case GameState.END:
                rb.velocity = Vector3.zero;
                break;
            default:
                break;
        }
        if (gameObject.transform.position.x <= despawnPoint) spawner.EnlistPlatform(this);
    }
}
