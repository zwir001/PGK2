using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float _health;

    [Header("Characteristics")]
    [SerializeField] private Enemy _characteristics;

    public void AttackEnemy(float damage)
    {
        _health -= damage;
    }

    public bool IsKilled()
    {
        return _health <= 0f;
    }
}
