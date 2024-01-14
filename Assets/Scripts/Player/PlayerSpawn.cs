using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    GameObject player;
    public float timeToRespawn;
    [SerializeField] private Vector2 raySize;
    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void RespawnPlayer()
    {
        GameManager.Instance.Lives--;
        if (GameManager.Instance.Lives <= 0)
        {
            GameManager.Instance.GameState = GameState.RESPAWN;
            SceneTransitionManager.Instance.LoadScene(2);
            return;
        }
        GameManager.Instance.GameState = GameState.RESPAWN;
        player.GetComponent<PlayerController>().DisableRB();

        StartCoroutine(TransitionGameState(timeToRespawn));
    }

    IEnumerator TransitionGameState(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance.GameState = GameState.GAMESTART;
        DetermineSafeSpawn();
        player.GetComponent<PlayerController>().EnableRB();
    }

    public void DetermineSafeSpawn()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, raySize, 0, Vector2.down, 30, groundLayer);
        //Debug.Log("Hits: " + hits.Length);
        if (hits != null && hits.Length > 0)
        {
            Debug.Log("Hit point discovered: " + hits[0].collider.name);
            player.transform.position = new Vector3(transform.position.x, hits[0].point.y, 0);
        }
        else
        {
            Debug.Log("No platform suitable to spawn on");
            player.transform.position = transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, raySize);
    }
}
