using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

public class UpgradeWindowManager : MonoBehaviour
{
    private Province province;
    [SerializeField]
    private GameObject MainSection;
    [SerializeField]
    private GameObject LumberMillSection;
    [SerializeField]
    private GameObject FarmSection;
    [SerializeField]
    private GameObject StoneQuarrySection;
    [SerializeField]
    private GameObject WallSection;
    [SerializeField]
    private GameObject StrongholdSection;

    private readonly List<int[]> LumberMillCosts = new List<int[]>
    {
        new int[] { 150, 50, 0, 2},
        new int[] { 750, 100, 0, 5},
        new int[] { 1500, 200, 100, 10}
    };

    private readonly List<int[]> FarmCosts = new List<int[]>
    {
        new int[] { 200, 50, 0, 1},
        new int[] { 250, 100, 0, 3},
        new int[] { 300, 150, 50, 5}
    };

    private readonly List<int[]> StoneQuarryCosts = new List<int[]>
    {
        new int[] { 100, 100, 0, 2},
        new int[] { 500, 300, 0, 5},
        new int[] { 1000, 700, 100, 10}
    };

    private readonly List<int[]> WallsCosts = new List<int[]>
    {
        new int[] { 50, 250, 0, 2},
        new int[] { 100, 750, 0, 3},
        new int[] { 200, 1500, 50, 5}
    };

    private readonly List<int[]> StrongholdCosts = new List<int[]>
    {
        new int[] { 500, 500, 50, 5},
        new int[] { 1000, 1000, 100, 10},
        new int[] { 2000, 2000, 200, 15}
    };

    private readonly int[] taxGains = new int[] { 0, 5, 10 };
    private readonly int[] UpgradedWoodAndStoneGains = new int[] { 10, 15, 25 };
    private readonly int[] UpgradedFoodGains = new int[] { 30, 60, 90 };
    private readonly int[] UpgradedHPBonus = new int[] { 500, 1000, 2000 };
    private readonly double[] UpgradedAttackBonus = new double[] {0.02, 0.05, 0.20};

    public void OpenUpgradeWindowForProvince(Province provinceToBeUpgraded)
    {
        province = provinceToBeUpgraded;
        UpdateUI();
    }

    public void UpdateUI()
    {
        UpdateMainSection();
        UpdateSection(LumberMillSection, LumberMillCosts[province.lumberMillLevel], province.lumberMillLevel);
        UpdateSection(StoneQuarrySection, StoneQuarryCosts[province.StoneQuarryLevel], province.StoneQuarryLevel);
        UpdateSection(FarmSection, FarmCosts[province.farmLevel], province.farmLevel);
        UpdateSection(WallSection, WallsCosts[province.wallsLevel], province.wallsLevel);
        UpdateSection(StrongholdSection, StrongholdCosts[province.strongholdLevel], province.strongholdLevel);
    }

    private void UpdateMainSection()
    {
        var provinceText = MainSection.transform.Find("ProvinceName").GetComponent<TMP_Text>();
        provinceText.text = province.name;

        SetText("GoldNumber", $"+{province.taxGain}");
        SetText("StoneNumber", $"+{province.GetStoneGain()}");
        SetText("WoodNumber", $"+{province.GetWoodGain()}");
        SetText("FoodBalance", $"{province.foodGain-province.foodNeed}");
        SetText("TaxLevel", $"{province.taxLevel}");

        SetVisibilityOfTaxButtons();
        UpdateConstructionStatus();

        string happinessBalanceText;
        if (province.happinessBalance >= 0)
            happinessBalanceText = $"{province.happinessLevel}+{province.happinessBalance}";
        else
            happinessBalanceText = $"{province.happinessLevel}{province.happinessBalance}";

        SetText("HappinessNumber", happinessBalanceText);
    }

    private void UpdateConstructionStatus()
    {
        var buildStatusText = MainSection.transform.Find("ConstructionInfo").GetComponent<TMP_Text>();
        switch (province.currentConstruction)
            {
            case Buildings.StoneQuarry:
                {
                    buildStatusText.text = $"Budowa: Kamienio³om {province.StoneQuarryLevel + 1} za {province.constructionTurns} tur";
                } break;
            case Buildings.LumberMill:
                {
                    buildStatusText.text = $"Budowa: Tartak {province.lumberMillLevel + 1} za {province.constructionTurns} tur";
                } break;
            case Buildings.Farm:
                {
                    buildStatusText.text = $"Budowa: Farma {province.farmLevel + 1} za {province.constructionTurns} tur";
                } break;
            case Buildings.Walls:
                {
                    buildStatusText.text = $"Budowa: Mury {province.wallsLevel + 1} za {province.constructionTurns} tur";
                } break;
            case Buildings.Stronghold:
                {
                    buildStatusText.text = $"Budowa: Twierdza {province.strongholdLevel + 1} za {province.constructionTurns} tur";
                }
                break;
            default:
                {
                    buildStatusText.text = "";
                } break;

        }
    }

    private void SetVisibilityOfTaxButtons()
    {
        var decreaseButton = MainSection.transform.Find("DecreaseTaxButton").gameObject;
        var increaseButton = MainSection.transform.Find("IncreaseTaxButton").gameObject;

        if(province.taxLevel == 0)
        {
            decreaseButton.SetActive(false);
        }
        else
        {
            decreaseButton.SetActive(true);
        }

        if(province.taxLevel == 2)
        {
            increaseButton.SetActive(false);
        }
        else
        {
            increaseButton.SetActive(true);
        }
    }

    private void SetText(string textFieldName, string resourceGainNumber)
    {
        var fieldText = MainSection.transform.Find(textFieldName).GetComponent<TMP_Text>();
        fieldText.text = resourceGainNumber;
    }

    private void UpdateSection(GameObject section, int[] costs, int buildingLevel)
    {
        var costText = section.transform.Find("Cost").GetComponent<TMP_Text>();
        var progressBar = section.transform.Find("ProgressBarBackground").GetComponent<Slider>();
        var currentLevelText = section.transform.Find("CurrentLevel").GetComponent<TMP_Text>();

        costText.text = $"Koszt: {costs[0]} drewna, {costs[1]} kamienia, {costs[2]} z³ota";
        progressBar.value = (float) buildingLevel / 3;
        currentLevelText.text = $"{buildingLevel}/3";

        var upgradeButton = section.transform.Find("UpgradeButton").gameObject;

        if(Resources.woodNumber < costs[0] || Resources.stoneNumber < costs[1] || Resources.goldNumber < costs[2] || province.currentConstruction != Buildings.None)
        {
            upgradeButton.SetActive(false);
        }
        else
        {
            upgradeButton.SetActive(true);
        }
    }

    public void UpgradeLumberMill()
    {
        var costs = LumberMillCosts[province.lumberMillLevel];
        Resources.woodNumber -= costs[0];
        Resources.stoneNumber -= costs[1];
        Resources.goldNumber -= costs[2];

        province.currentConstruction = Buildings.LumberMill;
        province.constructionTurns = costs[3];

        if (province.bonusResource == ResourceTypes.Wood)
            province.valueAfterUpgrade += (int)Math.Ceiling(1.3 * UpgradedWoodAndStoneGains[province.lumberMillLevel]);
        else
            province.valueAfterUpgrade += UpgradedWoodAndStoneGains[province.lumberMillLevel];

        UpdateUI();
    }

    public void UpgradeStoneQuary()
    {
        var costs = StoneQuarryCosts[province.StoneQuarryLevel];
        Resources.woodNumber -= costs[0];
        Resources.stoneNumber -= costs[1];
        Resources.goldNumber -= costs[2];

        province.currentConstruction = Buildings.StoneQuarry;
        province.constructionTurns = costs[3];

        if (province.bonusResource == ResourceTypes.Stone)
            province.valueAfterUpgrade += (int)Math.Ceiling(1.3 * UpgradedWoodAndStoneGains[province.StoneQuarryLevel]);
        else
            province.valueAfterUpgrade += UpgradedWoodAndStoneGains[province.StoneQuarryLevel];
       
        UpdateUI();
    }

    public void IncreaseTaxLevel()
    {
        var newLevel = province.taxLevel + 1;
        var newTaxGains = taxGains[newLevel];

        if (province.bonusResource == ResourceTypes.Gold)
            newTaxGains = (int)Math.Ceiling(1.3 * newTaxGains);

        province.taxGain = newTaxGains;
        province.taxLevel = newLevel;
        province.UpdateHappinessBalance();
        UpdateUI();
    }

    public void DecreaseTaxLevel()
    {
        var newLevel = province.taxLevel - 1;
        var newTaxGains = taxGains[newLevel];

        if (province.bonusResource == ResourceTypes.Gold)
            newTaxGains = (int)Math.Ceiling(1.3 * newTaxGains);

        province.taxGain = newTaxGains;
        province.taxLevel = newLevel;
        province.UpdateHappinessBalance();
        UpdateUI();
    }

    public void UpgradeFarm()
    {
        var costs = FarmCosts[province.farmLevel];
        Resources.woodNumber -= costs[0];
        Resources.stoneNumber -= costs[1];
        Resources.goldNumber -= costs[2];


        province.currentConstruction = Buildings.Farm;
        province.constructionTurns = costs[3];

        province.valueAfterUpgrade += UpgradedFoodGains[province.farmLevel];

        UpdateUI();
    }

    public void UpgradeWalls()
    {
        var costs = WallsCosts[province.wallsLevel];
        Resources.woodNumber -= costs[0];
        Resources.stoneNumber -= costs[1];
        Resources.goldNumber -= costs[2];

        province.currentConstruction = Buildings.Walls;
        province.constructionTurns = costs[3];
        province.valueAfterUpgrade += UpgradedHPBonus[province.wallsLevel];

        UpdateUI();
    }

    public void UpgradeStronghold()
    {
        var costs = WallsCosts[province.strongholdLevel];
        Resources.woodNumber -= costs[0];
        Resources.stoneNumber -= costs[1];
        Resources.goldNumber -= costs[2];

        province.currentConstruction = Buildings.Stronghold;
        province.constructionTurns = costs[3];
        province.valueAfterUpgrade += UpgradedAttackBonus[province.strongholdLevel];
        UpdateUI();
    }
}
