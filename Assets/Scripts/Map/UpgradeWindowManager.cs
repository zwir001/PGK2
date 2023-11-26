using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
        new int[] { 150, 50, 0},
        new int[] { 750, 100, 0 },
        new int[] { 1500, 200, 100 }
    };

    private readonly List<int[]> FarmCosts = new List<int[]>
    {
        new int[] { 200, 50, 0},
        new int[] { 250, 100, 0 },
        new int[] { 300, 150, 50 }
    };

    private readonly List<int[]> StoneQuarryCosts = new List<int[]>
    {
        new int[] { 100, 100, 0},
        new int[] { 500, 300, 0 },
        new int[] { 1000, 700, 100 }
    };

    private readonly List<int[]> WallsCosts = new List<int[]>
    {
        new int[] { 50, 250, 0},
        new int[] { 100, 750, 0 },
        new int[] { 200, 1500, 50 }
    };

    private readonly List<int[]> StrongholdCosts = new List<int[]>
    {
        new int[] { 500, 500, 50},
        new int[] { 1000, 1000, 100 },
        new int[] { 2000, 2000, 200 }
    };

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

        SetResourceGainNumber("GoldNumber", $"+{province.taxGain}");
        SetResourceGainNumber("StoneNumber", $"+{province.GetStoneGain()}");
        SetResourceGainNumber("WoodNumber", $"+{province.GetWoodGain()}");
        SetResourceGainNumber("HappinessNumber", $"{province.happinessLevel}");
        SetResourceGainNumber("FoodBalance", $"{province.foodGain-province.foodNeed}");
    }

    private void SetResourceGainNumber(string textFieldName, string resourceGainNumber)
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

        if(Resources.woodNumber < costs[0] || Resources.stoneNumber < costs[1] || Resources.goldNumber < costs[2])
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

        if (province.bonusResource == ResourceTypes.Wood)
            province.woodGain += (int)Math.Ceiling(1.3 * UpgradedWoodAndStoneGains[province.lumberMillLevel]);
        else
            province.woodGain += UpgradedWoodAndStoneGains[province.lumberMillLevel];

        province.lumberMillLevel++;
        UpdateUI();
    }

    public void UpgradeStoneQuary()
    {
        var costs = StoneQuarryCosts[province.StoneQuarryLevel];
        Resources.woodNumber -= costs[0];
        Resources.stoneNumber -= costs[1];
        Resources.goldNumber -= costs[2];

        if (province.bonusResource == ResourceTypes.Stone)
            province.stoneGain += (int)Math.Ceiling(1.3 * UpgradedWoodAndStoneGains[province.StoneQuarryLevel]);
        else
            province.stoneGain += UpgradedWoodAndStoneGains[province.StoneQuarryLevel];

        province.StoneQuarryLevel++;
        UpdateUI();
    }

    public void UpgradeFarm()
    {
        var costs = FarmCosts[province.farmLevel];
        Resources.woodNumber -= costs[0];
        Resources.stoneNumber -= costs[1];
        Resources.goldNumber -= costs[2];
        province.foodGain += UpgradedFoodGains[province.farmLevel];

        province.farmLevel++;
        UpdateUI();
    }

    public void UpgradeWalls()
    {
        var costs = WallsCosts[province.wallsLevel];
        Resources.woodNumber -= costs[0];
        Resources.stoneNumber -= costs[1];
        Resources.goldNumber -= costs[2];
        province.hpBonus += UpgradedHPBonus[province.wallsLevel];

        province.wallsLevel++;
        UpdateUI();
    }

    public void UpgradeStronghold()
    {
        var costs = WallsCosts[province.strongholdLevel];
        Resources.woodNumber -= costs[0];
        Resources.stoneNumber -= costs[1];
        Resources.goldNumber -= costs[2];
        province.attackBonus += UpgradedHPBonus[province.strongholdLevel];

        province.strongholdLevel++;
        UpdateUI();
    }
}
