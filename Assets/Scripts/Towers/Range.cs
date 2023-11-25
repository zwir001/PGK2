using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    [Header("Range")]
    [SerializeField] private float _radius;
    private float _previuosRadius;
    [SerializeField] private Transform _rangeObject;

    [Header("Object in range")]
    [SerializeField] private LayerMask _layerMask;

    [Header("Tower")]
    [SerializeField] private AttackTower _tower;

    private List<Transform> _enemies;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    private void Start()
    {
        _enemies = new List<Transform>();
        ChangeRadiusSize(); // Set radius at the beginning from the inspector
    }

    private void Update()
    {
        DetectEnemyInRange();

        if (_enemies.Count > 0)
        {
            _tower.Attack(ChooseTarget());
        }

        // Changing Range Size
        if (_previuosRadius != _radius)
        {
            ChangeRadiusSize();
            _previuosRadius = _radius;
        }
    }

    private void DetectEnemyInRange()
    {
        _enemies.Clear();
        var hitColliders = Physics2D.OverlapCircleAll(transform.position, _radius, _layerMask);

        foreach (var hitCollider in hitColliders)
        {
            var enemyTransform = hitCollider.transform;
            _enemies.Add(enemyTransform);
        }
    }

    private Transform ChooseTarget()
    {
        var biggestProgress = 0f;
        Transform targetToReturn = null;

        foreach (var enemy in _enemies)
        {
            if (enemy.GetComponent<Movement>().GetProgress() > biggestProgress)
            {
                biggestProgress = enemy.GetComponent<Movement>().GetProgress();
                targetToReturn = enemy;
            }
        }

        return targetToReturn;
    }

    public void ChangeRadiusSize()
    {
        _rangeObject.localScale = new Vector3(_radius * 2.0f, _radius * 2.0f, _rangeObject.localScale.z);
    }

    // Getters and setters
    public void SetRadius(float radius)
    {
        _radius = radius;
    }
}
