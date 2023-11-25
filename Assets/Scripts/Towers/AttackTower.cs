using System.Collections.Generic;
using UnityEngine;

public class AttackTower : MonoBehaviour
{
    [Header("Attack Details")]
    [SerializeField] private float _speed;
    private float _delay;

    [Header("Projectile Details")]
    [SerializeField] private Transform _parent; // where the objects need to be instantiated
    [SerializeField] private GameObject _objectToPool;
    [SerializeField] private List<GameObject> _bullets;
    [SerializeField] private int _amountToPool;

    private BulletLuk _projectileLuk;
    private BulletPlomien _projectilePlomien;
    //protected TowerRotation _towerRotation;
    private float _delayTimer;

    private void Start()
    {
        //_towerRotation = GetComponentInChildren<TowerRotation>();
        _delayTimer = 1/_speed;
        _delay = 1/_speed;

        // Object Pooling
        InitializeBullets();
    }

    public void Attack(Transform enemy)
    {
        if (enemy != null) // there is enemy
        {
            _delayTimer += Time.deltaTime;
            if (_delayTimer >= _delay)
            {
                // Rotate Tower towards target
                //_towerRotation.Rotate(enemy);

                // Attack
                if(GetPooledObject().GetComponent<BulletLuk>() != null)
                {
                    _projectileLuk = GetPooledObject().GetComponent<BulletLuk>();
                    _projectileLuk.Attack(enemy);
                }

                if(GetPooledObject().GetComponent<BulletPlomien>() != null)
                {
                    _projectilePlomien = GetPooledObject().GetComponent<BulletPlomien>();
                    _projectilePlomien.Attack(enemy);
                }

                _delayTimer = 0.0f;
            }

        }
    }

    private void InitializeBullets()
    {
        _bullets = new List<GameObject>();
        for (int i = 0; i < _amountToPool; i++)
        {
            var tmp = Instantiate(_objectToPool, _parent);
            tmp.SetActive(false);
            _bullets.Add(tmp);
        }
    }

    private GameObject GetPooledObject()
    {
        for (int i = 0; i < _amountToPool; i++)
        {
            if (!_bullets[i].activeInHierarchy)
            {
                return _bullets[i];
            }
        }
        return null;
    }

    // getters and setters
    public List<GameObject> GetBullets()
    {
        return _bullets;
    }
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    public void SetDamage(int damage)
    {

    }
}
