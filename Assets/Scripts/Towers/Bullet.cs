using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float _bulletSpeed;
    [SerializeField] protected float _maxDistance;
    [SerializeField] protected int _damage;

    protected Transform _target;
    protected Vector3 _startingPosition;
    protected Vector3 _shootingDirection;
    protected Vector3 _shootingPosition;
    protected float _currentDistance = 0f;

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
