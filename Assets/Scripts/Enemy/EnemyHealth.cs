using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _health;

    public void AttackEnemy(float damage)
    {
        _health -= damage;
    }

    public bool IsKilled()
    {
        return _health <= 0f;
    }
}
