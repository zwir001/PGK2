using UnityEngine;

public class BulletLuk : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _maxDistance;
    [SerializeField] private int _damage;

    private Transform _target;
    private Vector3 _startingPosition;
    private Vector3 _shootingDirection;
    private Vector3 _shootingPosition;
    private float _currentDistance = 0f;

    private void Awake()
    {
        _startingPosition = transform.parent.parent.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject;

            enemy.GetComponent<EnemyHealth>().AttackEnemy(_damage);

            if (enemy.GetComponent<EnemyHealth>().IsKilled())
            {
                var moneyToAdd = enemy.GetComponent<EnemyHealth>().GetEnemyInitialHealth();
                Statistics.Instance.AddMoneyEnemyDestroy(moneyToAdd);
                Destroy(enemy);
            }
            ResetBullet();
        }
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

            // rotation of the projectile
            var angle = Mathf.Atan2(_shootingDirection.y, _shootingDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            // If there is no enemy left to defeat, remain the direction
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

    //public virtual void UpgradeBullet(UpgradeData _upgrade)
    //{
    //}
}
