using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner<T> : MonoBehaviour, ISpawner where T : MonoBehaviour, ISpawn
{
    [SerializeField] private T prefab;
    private Queue<ISpawn> objectQueue;
    public int maxObjects = 4;
    [SerializeField] private float spawnPoint;
    [Range(-5, 4)] private float variableHeight;

    public float spawnTime = 8f;
    private float timeUntilSpawn;

    private void Start()
    {
        timeUntilSpawn = spawnTime;
        objectQueue = new Queue<ISpawn>();
        for (int i = 1; i < maxObjects; i++)
        {
            T temp = Instantiate(prefab);
            temp.AssignSpawner(this);
            temp.transform.parent = transform;
            temp.gameObject.SetActive(false);
            objectQueue.Enqueue(temp);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.PLAY) return;
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn < 0)
        {
            variableHeight = Random.Range(-5f, 4f);
            Vector2 spawnPos = new Vector2(spawnPoint, variableHeight);
            Spawn(spawnPos);
            timeUntilSpawn = spawnTime;
        }
    }

    public void ReturnObject(ISpawn poolObj)
    {
        objectQueue.Enqueue(poolObj);
    }

    public void Spawn(Vector3 pos)
    {
        if (objectQueue.Count <= 0) return;

        Debug.Log($"{name}: Spawning {prefab}");
        ISpawn poolObj = objectQueue.Dequeue();
        poolObj.Spawn(pos);
    }
}
