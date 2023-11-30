using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerUI : MonoBehaviour
{
    [Header("Tower")]
    [SerializeField] private GameObject _towerToPlace;
    [SerializeField] private List<LayerMask> _layersNotToPlaceTower;

    private static GameObject _moveableTower;

    private Transform _towerRange;
    private Color _towerColor;
    private Vector3 _mousePosition;
    private bool _isTowerMoving;
    private Collider2D[] _colliders;

    private void Start()
    {
        _colliders = new Collider2D[_layersNotToPlaceTower.Count];
        _isTowerMoving = false;
    }

    private void Update()
    {
        if (_isTowerMoving)
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (_moveableTower != null)
                _moveableTower.transform.position = new Vector3(_mousePosition.x, _mousePosition.y, 0);
            else if (_moveableTower == null)
                _isTowerMoving = false;

            // Check what the tower collides with right now
            for (int i = 0; i < _layersNotToPlaceTower.Count; i++)
            {
                _colliders = Physics2D.OverlapCircleAll(_mousePosition, 0.7f, _layersNotToPlaceTower[i]);

                if (_colliders.Length > 0)
                    break;
            }

            if (_isTowerMoving)
                ChangeRangeColor();

            if (Input.GetMouseButtonDown(0))
            {
                if (TowerCanBePlaced())
                    PlaceTower();
            }
        }
    }

    // Change the tower range color based on the info if the tower can be placed somewhere or not
    private void ChangeRangeColor()
    {
        if (_towerRange != null)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.OverlapPoint(mousePosition);
            if (_colliders.Length > 0 || EventSystem.current.IsPointerOverGameObject())
            {
                _towerRange.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, _towerColor.a);
            }
            else
            {
                _towerRange.GetComponent<SpriteRenderer>().color = _towerColor;
            }
        }
    }

    // on button click
    public void SelectTower()
    {
        // I want to buy the tower! :>
        if (_moveableTower == null || _moveableTower.GetComponent<TowerManager>().name != (_towerToPlace.name + "(Clone)"))
        {
            var towerPrice = _towerToPlace.GetComponent<TowerManager>().GetTowerInfo().towerPrice;

            if (Statistics.Instance.GetMoneyAmount() >= towerPrice)
            {
                if (_moveableTower != null)
                {
                    Destroy(_moveableTower);
                    _moveableTower = Instantiate(_towerToPlace, gameObject.transform.position, gameObject.transform.rotation);
                }
                else
                    _moveableTower = Instantiate(_towerToPlace, gameObject.transform.position, gameObject.transform.rotation);

                if (_moveableTower.GetComponent<Range>() != null)
                    _moveableTower.GetComponent<Range>().ChangeRadiusSize();

                if (_moveableTower.GetComponent<AttackTower>() != null)
                    _moveableTower.GetComponent<AttackTower>().enabled = false;
                if (_moveableTower.GetComponent<Range>() != null)
                    _moveableTower.GetComponent<Range>().enabled = false;
                if (_moveableTower.GetComponent<BoxCollider2D>() != null)
                    _moveableTower.GetComponent<BoxCollider2D>().enabled = false;

                _towerRange = _moveableTower.transform.Find("Range");
                _towerRange.GetComponent<SpriteRenderer>().enabled = true;
                _towerColor = _towerRange.GetComponent<SpriteRenderer>().color;

                _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _isTowerMoving = true;
            }
        }
        else
        {
            _isTowerMoving = false;
            Destroy(_moveableTower);
            _moveableTower = null;
        }
    }

    // Check if the tower is not going to be placed on path or other monkey or interface
    private bool TowerCanBePlaced()
    {
        if (_moveableTower != null)
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            for (int i = 0; i < _layersNotToPlaceTower.Count; i++)
            {
                if (_colliders.Length > 0)
                {
                    return _colliders.Length == 0;
                }
            }
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return false;
            }

            return true;
        }
        return false;
    }

    private void PlaceTower()
    {
        _isTowerMoving = false;

        if (_moveableTower.GetComponent<AttackTower>() != null)
            _moveableTower.GetComponent<AttackTower>().enabled = true;
        if (_moveableTower.GetComponent<Range>() != null)
            _moveableTower.GetComponent<Range>().enabled = true;
        if (_moveableTower.GetComponent<BoxCollider2D>() != null)
            _moveableTower.GetComponent<BoxCollider2D>().enabled = true;

        _towerRange = _moveableTower.transform.Find("Range");
        _towerRange.GetComponent<SpriteRenderer>().enabled = false;

        Statistics.Instance.AddInstantiatedTower(ref _moveableTower);

        // Set position of the placed tower
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _moveableTower.transform.position = new Vector3(_mousePosition.x, _mousePosition.y, 0);

        Statistics.Instance.DecreaseMoneyForBoughtTower(_moveableTower.GetComponent<TowerManager>().GetTowerInfo().towerPrice);

        _moveableTower = null;
    }

    public int GetTowerCost()
    {
        return _towerToPlace.GetComponent<TowerManager>().GetTowerInfo().towerPrice;
    }

    public GameObject GetTowerToPlace()
    {
        return _towerToPlace;
    }
}
