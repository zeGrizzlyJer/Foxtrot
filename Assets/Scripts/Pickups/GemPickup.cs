using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPickup : MonoBehaviour, ISpawn
{
    private Rigidbody2D rb;
    private ISpawner spawner;
    [SerializeField] private float verticalPeak = 3f;
    [SerializeField] private float horizontalSpeedRate = 0.5f;
    [SerializeField] private float verticalSpeedRate = 1f;
    [SerializeField] private float despawnPoint = -10f;
    [SerializeField] private int value = 1;

    [SerializeField] private AnimationClip pickupClip;

    private float t;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        t += Time.deltaTime;

        Vector2 direction;

        direction = new Vector2(-GameManager.Instance.gameSpeed * horizontalSpeedRate, verticalPeak * Mathf.Sin(Mathf.Rad2Deg * t * verticalSpeedRate));

        rb.velocity = direction;

        if (transform.position.x <= despawnPoint) Despawn();
    }

    public void Spawn(Vector2 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
        spawner.ReturnObject(this);
    }

    public void AssignSpawner(ISpawner s)
    {
        spawner = s;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int currency = GameManager.Instance.Money;
        currency += value;
        GameManager.Instance.Money = currency;

        StartCoroutine(PickupCollected());
        Debug.Log($"{name} has been picked up");
    }


    IEnumerator PickupCollected()
    {
        float timeToDespawn = (pickupClip) ? pickupClip.length : 0f;
        yield return new WaitForSeconds(timeToDespawn);
        Despawn();
    }
}
