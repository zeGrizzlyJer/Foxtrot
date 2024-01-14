using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    BoxCollider2D[] colliders;

    private void Awake()
    {
        colliders = GetComponentsInChildren<BoxCollider2D>();

        foreach (BoxCollider2D col in colliders)
        {
            col.AddComponent<CollisionInteraction>();
        }
    }
}
