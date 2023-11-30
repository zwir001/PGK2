using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private TMP_Text _turnCounter;
    [SerializeField]
    private TMP_Text _woodCounter;
    [SerializeField]
    private TMP_Text _stoneCounter;
    [SerializeField]
    private TMP_Text _goldCounter;
    [SerializeField]
    private GameObject _containerWithProvinceButtons;
    [SerializeField]
    private Sprite _lostProvinceSprite;
    [SerializeField]
    private Sprite _attackedProvinceSprite;
    [SerializeField]
    private Sprite _normalProvinceSprite;

    public void NextTurn()
    {
        Resources.turn++;
        UpdateResources();
        _turnCounter.text = $"Tura: {Resources.turn}";
        _woodCounter.text = $"{Resources.woodNumber}";
        _stoneCounter.text = $"{Resources.stoneNumber}";
        _goldCounter.text = $"{Resources.goldNumber}";
        RandomAttackedProvince();
        UpdateButtons();

        Resources.listOfProvinces.ForEach(x => x.UpdateProvinceStatus());
        if (Resources.turn % 20 == 0)
        {
            IncreaseFoodConsumption();
        }
    }

    private void IncreaseFoodConsumption()
    {

        Resources.listOfProvinces.ForEach(x =>
        {
            if (x.foodNeed == 90)
                return;

            Random gen = new Random();
            int increase = gen.Next(5);
            x.foodNeed += increase;
        });
    }

    private void RandomAttackedProvince()
    {
        //Implementation only for Rome
        Random gen = new Random();
        int prob = gen.Next(100);
        Resources.listOfProvinces.FirstOrDefault(x => x.id == 9).isAttacked = prob < 50;
    }

    private void UpdateButtons()
    {
        foreach(Transform transform in _containerWithProvinceButtons.transform)
        {
            UpgradeWindow upgradeWindow = transform.gameObject.GetComponent<UpgradeWindow>();
            var image = transform.gameObject.GetComponent<Image>();

            var province = Resources.listOfProvinces.FirstOrDefault(x => x.id == upgradeWindow.provinceId);

            if (province.isLost)
            {
                image.sprite = _lostProvinceSprite;
            }
            else if (province.isAttacked)
            {
                image.sprite = _attackedProvinceSprite;
            }
            else
            {
                image.sprite = _normalProvinceSprite;
            }
        }
    }

    private void UpdateResources()
    {
        Resources.woodNumber += Resources.listOfProvinces.Where(x => !x.isLost).Sum(x => x.GetWoodGain());
        Resources.stoneNumber += Resources.listOfProvinces.Where(x => !x.isLost).Sum(x => x.GetStoneGain());
        Resources.goldNumber += Resources.listOfProvinces.Where(x => !x.isLost).Sum(x => x.taxGain);
    }
}
