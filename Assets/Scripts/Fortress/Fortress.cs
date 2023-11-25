using System.Collections.Generic;
using UnityEngine;

public class Fortress : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int _fortressHealth;

    [Header("Attack")]
    [SerializeField] private int _fortressDamage;
    [SerializeField] private float _fortressAttackSpeed;
    private float _attackTimer;

    // Enemies
    private readonly Queue<EnemyHealth> _enemies = new Queue<EnemyHealth>();
    private EnemyHealth _currentEnemy;

    private void Awake()
    {
        _attackTimer = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
            _enemies.Enqueue(collision.gameObject.GetComponent<EnemyHealth>());
    }

    private void Update()
    {
        _attackTimer += Time.deltaTime;

        if (_enemies.Count > 0 || _currentEnemy != null)
        {
            AttackApproachingEnemies();
        }
    }

    private void AttackApproachingEnemies()
    {
        if (_attackTimer >= (1 / _fortressAttackSpeed))
        {
            if (_currentEnemy == null && _enemies.Count > 0)
                _currentEnemy = _enemies.Dequeue();

            _currentEnemy.AttackEnemy(_fortressDamage);
            _attackTimer = 0f;
        }

        if (_currentEnemy != null && _currentEnemy.IsKilled())
        {
            Destroy(_currentEnemy.gameObject);
            _currentEnemy = null;
        }
    }

    private void DestroyFortress()
    {
        if( _fortressHealth <= 0 )
            gameObject.SetActive( false );
    }

    public void DealDamage(int damage)
    {
        _fortressHealth -= damage;

        if (_fortressHealth <= 0)
            _fortressHealth = 0;

        Statistics.Instance.DecreaseLifeAmount(_fortressHealth);
        DestroyFortress();
    }

    public bool IsFortressDestroyed()
    {
        return _fortressHealth <= 0;
    }
}
