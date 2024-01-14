using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public PlayerSpawn playerSpawn;

    private void Start()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("Respawn").GetComponent<PlayerSpawn>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        Debug.Log("Death by Falling");
        playerSpawn.RespawnPlayer();
    }
}
