using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private PlatformBehaviour[] prefabs;
    public int maxPrefabInstances = 2;
    private List<PlatformBehaviour> platformPool = new List<PlatformBehaviour>();
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

        PopulatePool();
        SpawnPlatform();
    }

    private void PopulatePool()
    {
        foreach (PlatformBehaviour platform in prefabs)
        {
            for (int i = 0; i < maxPrefabInstances; i++)
            {
                PlatformBehaviour temp = Instantiate(platform, gameObject.transform);
                platformPool.Add(temp);
                temp.gameObject.transform.position = spawnPoint;
                temp.gameObject.SetActive(false);
            }
        }
    }

    public void SpawnPlatform()
    {
        if (!spawnTransform) return;

        int randomPlatform = Random.Range(0, platformPool.Count);
        PlatformBehaviour temp = platformPool[randomPlatform];
        platformPool.Remove(temp);
        temp.gameObject.transform.position = spawnPoint;
        temp.gameObject.SetActive(true);
        Debug.Log("Spawn Platform");
    }

    public void EnlistPlatform(PlatformBehaviour platform)
    {
        if (!platform.IsStarterPlatform) platformPool.Add(platform);
        platform.gameObject.SetActive(false);
        SpawnPlatform();
    }
}
