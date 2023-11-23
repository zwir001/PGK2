using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement details")]
    [SerializeField] private float _movementSpeed;
    private float _initialMovementSpeed;
    private float _progress;
    private float _currentDistance;

    [Header("Path")]
    [SerializeField] private Path _path;
    private GameObject _currentPoint;
    private int _currentPointIndex;

    // Enemy position
    private Vector3 currentPosition;

    private void Awake()
    {
        _currentPoint = _path.points[0];
        _currentPointIndex = 0;
        currentPosition = _path.spawn.transform.position;
        _initialMovementSpeed = _movementSpeed;
    }

    private void Update()
    {
        ApproachToPoint();

        if(_progress >= 1.0f)
            Destroy(gameObject);
    }

    public void ApproachToPoint()
    {
        if (_currentPointIndex < _path.points.Count)
        {
            _currentPoint = _path.points[_currentPointIndex];
            currentPosition = Vector3.MoveTowards(currentPosition, _currentPoint.transform.position, _movementSpeed * Time.deltaTime);
            transform.position = currentPosition;

            if (transform.position == _currentPoint.transform.position)
                _currentPointIndex++;

            float movement = _movementSpeed * Time.deltaTime;
            _currentDistance += movement;
            _progress = _currentDistance / LevelManager.Instance.GetPathLength();
        }
    }

    public void SetMovementSpeed()
    {
        _movementSpeed = _initialMovementSpeed;
    }

    public void SetMovementSpeed(float movementSpeed)
    {
        _movementSpeed = movementSpeed;
    }
}
