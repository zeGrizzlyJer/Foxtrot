using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleSpawner : MonoBehaviour, IEagle
{
    [SerializeField] private EagleController prefab;
    private Queue<IEagle> eagles;
    public int maxEagles = 4;
    [SerializeField] private float spawnPoint;
    [Range(-5, 4)] private float variableHeight;

    public float spawnTime = 8f;
    private float timeUntilSpawn;

    private void Start()
    {
        timeUntilSpawn = spawnTime;
        eagles = new Queue<IEagle>();
        for (int i = 1; i < maxEagles; i++)
        {
            EagleController temp = Instantiate(prefab);
            temp.AssignSpawner(this);
            temp.transform.parent = transform;
            temp.gameObject.SetActive(false);
            eagles.Enqueue(temp);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.PLAY) return;
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn < 0)
        {
            variableHeight = Random.Range(-5f, 4f);
            Vector3 spawnPos = new Vector3(spawnPoint, variableHeight, 0);
            Spawn(spawnPos);
            timeUntilSpawn = spawnTime;
        }
    }

    public void ReturnEagle(IEagle eagle)
    {
        eagles.Enqueue(eagle);
    }

    public void Spawn(Vector3 pos)
    {
        if (eagles.Count <= 0) return;

        Debug.Log($"{name}: Spawning Eagle");
        IEagle eagle = eagles.Dequeue();
        eagle.Spawn(pos);
    }
}
