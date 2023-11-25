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

        SetResourceGainNumber("GoldNumber", province.taxGain);
        SetResourceGainNumber("StoneNumber", province.stoneGain);
        SetResourceGainNumber("WoodNumber", province.woodGain);
    }

    private void SetResourceGainNumber(string textFieldName, int resourceGainNumber)
    {
        var fieldText = MainSection.transform.Find(textFieldName).GetComponent<TMP_Text>();
        fieldText.text = $"+{resourceGainNumber}";
    }

    private void UpdateSection(GameObject section, int[] costs, int buildingLevel)
    {
        var costText = section.transform.Find("Cost").GetComponent<TMP_Text>();
        var progressBar = section.transform.Find("ProgressBarBackground").GetComponent<Slider>();
        var currentLevelText = section.transform.Find("CurrentLevel").GetComponent<TMP_Text>();

        costText.text = $"Koszt: {costs[0]} drewna, {costs[1]} kamienia, {costs[2]} z³ota";
        progressBar.value = buildingLevel / 3;
        currentLevelText.text = $"{buildingLevel}/3";
    }
}
