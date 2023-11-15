using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _attackSpeed;
    private float _attackTimer;
    private bool _isAttacking;

    private Fortress _fortress; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fortress"))
        {
            GetComponent<Movement>().SetMovementSpeed(0f);
            _fortress = collision.gameObject.GetComponent<Fortress>();
            _isAttacking = true;
        }
    }

    private void Update()
    {
        _attackTimer += Time.deltaTime;

        if( _isAttacking)
        {
            DealDamage();
            if( _fortress.IsFortressDestroyed())
                GetComponent<Movement>().SetMovementSpeed();
        }
    }

    private void DealDamage()
    {
        if (_attackTimer >= (1 / _attackSpeed))
        {
            _fortress.DealDamage(_damage);
            _attackTimer = 0f;

            if(_fortress.IsFortressDestroyed())
                _isAttacking = false;
        }
    }
}
