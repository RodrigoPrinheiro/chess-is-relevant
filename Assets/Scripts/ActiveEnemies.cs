using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game Systems/ActiveEnemies")]
public class ActiveEnemies : ScriptableObject
{
    private List<EnemyActor> _enemies = new List<EnemyActor>();
    public int ActiveEnemiesCount => _enemies.Count;

    public void AddNewEnemyActor(EnemyActor EnemyActor)
    {
        if (_enemies.Contains(EnemyActor)) return;
        _enemies.Add(EnemyActor);
    }

    public void RemoveEnemyActor(EnemyActor EnemyActor)
    {
        if (!_enemies.Contains(EnemyActor)) return;
        _enemies.Remove(EnemyActor);
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