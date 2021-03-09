using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game Systems/ActiveEnemies")]
public class ActiveEnemies : ScriptableObject
{
    [SerializeField] private bool _debugObject;
    private List<EnemyActor> _enemies = new List<EnemyActor>();
    public int ActiveEnemiesCount => _enemies.Count;

    public void AddNewEnemyActor(EnemyActor enemy)
    {
        if (_enemies.Contains(enemy)) return;
        
        if (_debugObject)
            Debug.Log("New Enemy Added: " + enemy.name);
        
        _enemies.Add(enemy);
    }

    public void RemoveEnemyActor(EnemyActor enemy)
    {
        if (!_enemies.Contains(enemy)) return;

        _enemies.Remove(enemy);

        if (_debugObject)
        {
            Debug.Log("Enemy Removed: " + enemy.name + "\n New Count: " + ActiveEnemiesCount);
        }
    }

    public void ResetList()
    {
        _enemies.Clear();
    }

    public EnemyActor ClosesTo(Vector3 position, float minimum = 0f)
    {
        EnemyActor closest = null;
        float min = minimum == 0f ? float.PositiveInfinity : minimum;

        for (int i = 0; i < _enemies.Count; i++)
        {
            float d = Vector3.Distance(position, _enemies[i].transform.position);

            if (d < min)
            {
                min = d;
                closest = _enemies[i];
            }
        }
        return closest;
    }

    public IEnumerable<EnemyActor> GetEnemies()
    {
        foreach (EnemyActor item in _enemies)
        {
            yield return item;
        }
    }
}