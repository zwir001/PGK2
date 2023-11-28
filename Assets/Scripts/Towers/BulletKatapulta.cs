using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletKatapulta : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _damage;

    private Transform _target;
    private Vector3 _startingPosition;
    private Vector3 _shootingDirection;
    private Vector3 _shootingPosition;
    private float _currentDistance = 0f;

    [Header("Explosion")]
    [SerializeField] private float _explosionDiameter;
    [SerializeField] private float _numberOfEnemiesToDestroy; // around the whole diameter

    [Header("Enemy")]
    [SerializeField] private LayerMask _layerMask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject;

            transform.GetComponentInParent<AttackTower>().ExplosionDuration(collision.gameObject.transform, _explosionDiameter);
            gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        _startingPosition = transform.parent.parent.position;
    }

    public void Attack(Transform target)
    {
        gameObject.SetActive(true);
        transform.position = _startingPosition;
        _target = target;
        _shootingDirection = _target.transform.position - transform.position;
    }

    private void Update()
    {
        var move = Time.deltaTime * _bulletSpeed;
        _currentDistance += move;

        if (_target != null)
        {
            _shootingPosition = _target.position;

            transform.position += _shootingDirection * _bulletSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += _shootingDirection * _bulletSpeed * Time.deltaTime;
        }

        if (_currentDistance >= _maxDistance)
        {
            ResetBullet();
        }
    }

    private void ResetBullet()
    {
        _currentDistance = 0f;
        transform.position = _startingPosition;

        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, _explosionDiameter / 2, _layerMask);

        var enemiesToDestroy = new List<GameObject>();
        foreach (var col in colliders)
        {
            enemiesToDestroy.Add(col.gameObject);
        }

        var maxObjects = 0f;
        if (_numberOfEnemiesToDestroy < enemiesToDestroy.Count)
            maxObjects = _numberOfEnemiesToDestroy;
        else
            maxObjects = enemiesToDestroy.Count;

        for (int i = 0; i < maxObjects; i++)
        {
            if (enemiesToDestroy.Count == 0)
                break;

            var index = Random.Range(0, enemiesToDestroy.Count);
            var enemyToAttack = enemiesToDestroy[index];
            var characteristics = enemyToAttack.GetComponent<EnemyHealth>().GetCharacteristics();

            if (characteristics.immuneToFire || characteristics.immuneToArrows || characteristics.immuneToSiegeArtillery)
                enemyToAttack.GetComponent<EnemyHealth>().AttackEnemy(_damage * 0.5f);
            else if (characteristics.vulnerableToFire || characteristics.vulnerableToArrows || characteristics.vulnerableToSiegeArtillery)
                enemyToAttack.GetComponent<EnemyHealth>().AttackEnemy(_damage * 2f);
            else
                enemyToAttack.GetComponent<EnemyHealth>().AttackEnemy(_damage);

            if (enemyToAttack.GetComponent<EnemyHealth>().IsKilled())
            {
                var moneyToAdd = enemyToAttack.GetComponent<EnemyHealth>().GetEnemyInitialHealth();
                Statistics.Instance.AddMoneyEnemyDestroy(moneyToAdd);
                Destroy(enemyToAttack);
            }
            enemiesToDestroy.RemoveAt(index);
        }

        ResetBullet();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _explosionDiameter / 2);
    }

    public void SetDamage(float increase)
    {
        _damage += _damage * increase;
    }
}
