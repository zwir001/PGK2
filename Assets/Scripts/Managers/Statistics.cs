using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Statistics : MonoBehaviour
{
    [Header("Statistics")]
    [SerializeField] private float _numberOfLifes;
    [SerializeField] private int _moneyAtTheBeginning;
    private float _moneyAmount;
    private int _levelNumber;
    private int _oldLevelNumber;

    [Header("Interface")]
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _lifesText;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _remainingLevelsText;

    [Header("GameOver")]
    [SerializeField] private GameObject _gameOverScreen;

    [Header("Fortress")]
    [SerializeField] private Fortress _fortress;

    private List<GameObject> _towers;
    private GameObject _clickedTower;

    public static Statistics Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        _towers = new List<GameObject>();
    }

    private void Start()
    {
        _oldLevelNumber = 0;
        _levelNumber = 1;

        //_lifesText.text += " " + _numberOfLifes.ToString();
        _moneyAmount = _moneyAtTheBeginning;
        _moneyText.text += " " + _moneyAtTheBeginning.ToString();
    }

    private void Update()
    {
        ChangeLevelInInterface();

        if (Input.GetMouseButtonDown(0))
        {
            DetectClickedTower();
        }

        _timeText.text = "Czas:";
        if (LevelManager.Instance.IsGameStarted())
        {
            _timeText.text += " " + LevelManager.Instance.GetTime();
        }
    }

    public void DetectClickedTower()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics2D.RaycastAll(ray.origin, ray.direction);

        // We just want to click somewhere else to close tower interface
        if (_clickedTower != null)
        {
            // "Somewhere else" is not the tower interface
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                _clickedTower.GetComponent<TowerManager>().ShowUpgradePanel();
                _clickedTower = null;
            }
        }
        // The tower is being chosen already by the player
        else if (hits.Length != 0)
        {
            foreach (var hit in hits)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Tower"))
                    {
                        if (hit.collider.gameObject.GetComponent<TowerManager>().GetNumberOfClicks() <= 0)
                            hit.collider.gameObject.GetComponent<TowerManager>().Click();

                        _clickedTower = hit.collider.gameObject;
                        _clickedTower.GetComponent<TowerManager>().ShowUpgradePanel();
                        break;
                    }
                }
            }
        }
    }

    public void AddInstantiatedTower(ref GameObject newTower)
    {
        _towers.Add(newTower);
    }

    // Delete the tower chosen by user (on sell)
    public void DeleteInstantiatedTower()
    {
        _towers.Remove(_clickedTower);
        Destroy(_clickedTower);
    }

    // Show current level to the user
    private void ChangeLevelInInterface()
    {
        _levelNumber = LevelManager.Instance.GetLevelIndex();

        if(_oldLevelNumber != _levelNumber)
        {
            _levelText.text = "Runda:";
            _remainingLevelsText.text = "Liczba pozosta³ych rund:";

            _levelText.text += " " + _levelNumber.ToString();
            _remainingLevelsText.text += " " + (LevelManager.Instance.GetLevelsCount() - _levelNumber).ToString();
            _oldLevelNumber = _levelNumber;
        }
    }

    public void DecreaseLifeAmount(int _fortressHealth)
    {
        _numberOfLifes = _fortressHealth;
        _lifesText.text = "Pozosta³e ¿ycia:";
        _lifesText.text += " " + _numberOfLifes;

        if (_numberOfLifes <= 0)
            _gameOverScreen.SetActive(true);
    }

    public void SetLifeAmount(int _fortressHealth)
    {
        _numberOfLifes = _fortressHealth;
        _lifesText.text = "Pozosta³e ¿ycia: " + _numberOfLifes;

        if (_numberOfLifes <= 0)
            _gameOverScreen.SetActive(true);
    }

    // For every popped bloon you earn money. Show to the user amount of money
    public void AddMoneyEnemyDestroy(float money)
    {
        _moneyAmount += money;
        _moneyText.text = "Zasoby:";
        _moneyText.text += " " + _moneyAmount.ToString();
    }

    //// Show the amount of money after selling the tower
    //public void AddMoneyForSoldTower(int moneyAmount)
    //{
    //    _moneyAmount += moneyAmount;
    //    _moneyText.text = _moneyAmount.ToString();
    //}

    // Show the amount of money after buying the tower
    public void DecreaseMoneyForBoughtTower(int moneyAmount)
    {
        _moneyAmount -= moneyAmount;
        _moneyText.text = "Zasoby:";
        _moneyText.text += " " + _moneyAmount.ToString();
    }

    //////////////////////////
    // Getters and Setters
    //////////////////////////

    public GameObject GetClickedTower()
    {
        return _clickedTower;
    }

    public void ForgetClickedTower()
    {
        _clickedTower = null;
    }

    public float GetMoneyAmount()
    {
        return _moneyAmount;
    }
}
