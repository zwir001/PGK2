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

    private Transform _target;
    private Vector3 _startingPosition;
    private Vector3 _shootingDirection;
    private Vector3 _shootingPosition;
    private float _currentDistance = 0f;

    private void Awake()
    {
        _startingPosition = transform.parent.parent.position;
    }

    private void Update()
    {
        _currentTimer += Time.deltaTime;

        if (_currentTimer >= _timer)
        {
            ResetBullet();
            _currentTimer = 0;
        }
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
        }
    }

    public void Attack(Transform target)
    {
        gameObject.SetActive(true);
        transform.position = new Vector3(_startingPosition.x, _startingPosition.y + transform.parent.parent.GetComponent<Range>().GetRadius()/2, _startingPosition.z);
        transform.localScale = new Vector3(transform.localScale.x, transform.parent.parent.GetComponent<Range>().GetRadius(), transform.localScale.z);
        _target = target;
        _shootingDirection = _target.transform.position - transform.position;
        transform.rotation = Quaternion.AngleAxis(-180, Vector3.forward);
        var angle = Mathf.Atan2(_shootingDirection.y, _shootingDirection.x) * Mathf.Rad2Deg - 90;
        var vector = new Vector3(0, 0, 0);
        transform.RotateAround(transform.parent.parent.position, Vector3.forward, angle);
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
