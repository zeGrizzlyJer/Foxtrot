using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private PlatformBehaviour[] prefabs;
    private Transform spawnTransform;
    private Vector3 spawnPoint;

    void Start()
    {
        spawnTransform = GameObject.FindGameObjectWithTag("PlatformSpawnPoint").transform;
        if (!spawnTransform)
        {
            Debug.Log("Add Platform Spawn Point To Scene");
            return;
        }
        spawnPoint = spawnTransform.position;
    }

    public void SpawnPlatform()
    {
        if (!spawnTransform) return;

        int randomPlatform = Random.Range(0, prefabs.Length);
        PlatformBehaviour temp = Instantiate(prefabs[randomPlatform], gameObject.transform);
        temp.gameObject.transform.position = spawnPoint;
        Debug.Log("Spawn Platform");
    }
}
