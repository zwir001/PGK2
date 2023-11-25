using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlomien : MonoBehaviour
{
    [SerializeField] protected float _bulletSpeed;
    [SerializeField] protected float _maxDistance;
    [SerializeField] protected int _damagePerSecond;
    [SerializeField] private float _timer;
    private float _currentTimer;

    protected Transform _target;
    protected Vector3 _startingPosition;
    protected Vector3 _shootingDirection;
    protected Vector3 _shootingPosition;
    protected float _currentDistance = 0f;

    private void Awake()
    {
        _startingPosition = transform.parent.parent.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject;

            enemy.GetComponent<EnemyHealth>().AttackEnemyDamagePerSecond(_damagePerSecond);

            if (enemy.GetComponent<EnemyHealth>().IsKilled())
            {
                var moneyToAdd = enemy.GetComponent<EnemyHealth>().GetEnemyInitialHealth();
                Statistics.Instance.AddMoneyEnemyDestroy(moneyToAdd);
                Destroy(enemy);
            }

            _currentTimer += Time.deltaTime;

            if(_currentTimer >= _timer)
                ResetBullet();
        }
    }

    public void Attack(Transform target)
    {
        gameObject.SetActive(true);
        transform.position = _startingPosition;
        _target = target;
    }

    private void ResetBullet()
    {
        _currentDistance = 0f;
        transform.position = _startingPosition;

        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    //public virtual void UpgradeBullet(UpgradeData _upgrade)
    //{
    //}
}
