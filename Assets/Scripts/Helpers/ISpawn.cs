using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawn
{
    void Spawn(Vector2 pos);
    void Despawn();
    void AssignSpawner(ISpawner s);
}
