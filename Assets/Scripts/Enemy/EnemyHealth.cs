using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float _health;
    private float _initialHealth;

    [Header("Characteristics")]
    [SerializeField] private Enemy _characteristics;

    private void Start()
    {
        _initialHealth = _health;
    }

    public void AttackEnemy(float damage)
    {
        _health -= damage;
    }

    public void AttackEnemyDamagePerSecond(float damagePerSecond)
    {
        _health -= damagePerSecond * Time.deltaTime;
    }

    public float GetEnemyInitialHealth()
    {
        return _initialHealth;
    }

    public bool IsKilled()
    {
        return _health <= 0f;
    }
}
