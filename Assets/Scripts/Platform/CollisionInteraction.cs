using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionInteraction : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(0).normal == Vector2.up || collision.GetContact(0).normal == Vector2.down || collision.GetContact(0).normal == Vector2.left) return;

        Debug.Log("Death by Collision");
        GameObject.FindGameObjectWithTag("Respawn").GetComponent<PlayerSpawn>().RespawnPlayer();
    }
}
