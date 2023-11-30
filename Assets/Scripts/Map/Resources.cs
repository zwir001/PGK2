
using System;
using System.Collections.Generic;

public static class MapResources
{
    public static List<Province> listOfProvinces = new()
    {
        new Province(0, "Asturica", 7, 5, 5, ResourceTypes.Wood),
        new Province(1, "Corduba", 7, 5, 5, ResourceTypes.Wood),
        new Province(2, "Osca", 5, 7, 5, ResourceTypes.Stone),
        new Province(3, "Massalia", 5, 7, 5, ResourceTypes.Stone),
        new Province(4, "Alesia", 7, 5, 5, ResourceTypes.Wood),
        new Province(5, "Londinium", 5, 5, 7, ResourceTypes.Gold),
        new Province(6, "Deva", 7, 5, 5, ResourceTypes.Wood),
        new Province(7, "Batavodurum", 5, 7, 5, ResourceTypes.Stone),
        new Province(8, "Patavium", 5, 7, 5, ResourceTypes.Stone),
        new Province(9, "Rzym", 5, 5, 7, ResourceTypes.Gold),
        new Province(10, "Syrakuza", 5, 5, 7, ResourceTypes.Gold),
        new Province(11, "Segestica", 7, 5, 5, ResourceTypes.Wood),
        new Province(12, "Thessalonika", 5, 7, 5, ResourceTypes.Stone),
        new Province(13, "Ateny", 5, 5, 7, ResourceTypes.Gold),
        new Province(14, "Konstantynopol", 5, 5, 7, ResourceTypes.Gold)
    };
    public static int turn = 1;
    public static int woodNumber = 0;
    public static int stoneNumber = 0;
    public static int goldNumber = 0;
}

public class Province
{
    public int id;
    public string name;
    public int woodGain;
    public int stoneGain;
    public int foodGain;
    public int foodNeed;
    public int taxGain;
    public int happinessLevel;
    public int hpBonus;
    public double attackBonus;
    public bool isLost;
    public bool isAttacked;
    public ResourceTypes bonusResource;
    public Buildings currentConstruction;
    public int constructionTurns;
    public double valueAfterUpgrade;
    public int farmLevel;
    public int StoneQuarryLevel;
    public int lumberMillLevel;
    public int wallsLevel;
    public int strongholdLevel;
    public int happinessBalance;
    public int taxLevel;

    public Province(int id, string name, int woodGain, int stoneGain, int taxGain, ResourceTypes resource)
    {
        this.id = id;
        this.name = name;
        this.woodGain = woodGain;
        this.stoneGain = stoneGain;
        foodGain = 10;
        foodNeed = 0;
        this.taxGain = taxGain;
        happinessLevel = 100;
        farmLevel = 0;
        StoneQuarryLevel = 0;
        lumberMillLevel = 0;
        wallsLevel = 0;
        strongholdLevel = 0;
        isLost = false;
        bonusResource = resource;
        hpBonus = 0;
        attackBonus = 0;
        isAttacked = false;
        taxLevel = 1;
        currentConstruction = Buildings.None;
        constructionTurns = -1;
    }

    public int GetStoneGain()
    {
        if (happinessLevel > 75)
            return stoneGain;
        else if (happinessLevel > 50)
            return (int)Math.Floor(0.95 * stoneGain);
        else if (happinessLevel > 25)
            return (int)Math.Floor(0.70 * stoneGain);
        else
            return (int)Math.Floor(0.30 * stoneGain);
    }

    public int GetWoodGain()
    {
        if (happinessLevel > 75)
            return woodGain;
        else if (happinessLevel > 50)
            return (int)Math.Floor(0.95 * woodGain);
        else if (happinessLevel > 25)
            return (int)Math.Floor(0.70 * woodGain);
        else
            return (int)Math.Floor(0.30 * woodGain);
    }

    private int GetImpactOfFoodOnHappiness()
    {
        if (foodGain < foodNeed)
        {
            double foodDeficit = (double)((foodNeed - foodGain) * 100) / foodNeed;

            if (foodDeficit >= 0.05 && foodDeficit < 0.25)
                return -5;
            else if (foodDeficit >= 0.25 && foodDeficit < 0.50)
                return -10;
            else if (foodDeficit >= 0.50)
                return -20;
        }

        return 0;
    }

    public void UpdateHappinessBalance()
    {
        happinessBalance = GetImpactOfFoodOnHappiness() + GetImpactOfTaxesOnHappiness();
    }

    public void UpdateProvinceStatus()
    {
        UpdateHappinessBalance();
        UpdateBuildProcess();

        if (happinessLevel == 100 && happinessBalance > 0 || happinessLevel == -100 && happinessBalance < 0)
            return;

        happinessLevel += happinessBalance;
    }

    private void UpdateBuildProcess()
    {
        constructionTurns--;

        if(constructionTurns == 0)
        {
            switch (currentConstruction)
            {
                case Buildings.LumberMill:
                    {
                        lumberMillLevel++;
                        woodGain = (int) valueAfterUpgrade;
                    } break;
                case Buildings.StoneQuarry: 
                    {
                        StoneQuarryLevel++;
                        stoneGain = (int)valueAfterUpgrade;
                    } break;
                case Buildings.Farm:
                    {
                        farmLevel++;
                        foodGain = (int)valueAfterUpgrade;
                    } break;
                case Buildings.Walls: {
                        wallsLevel++;
                        hpBonus = (int)valueAfterUpgrade;
                    }
                    break;
                case Buildings.Stronghold: 
                    {
                        strongholdLevel++;
                        attackBonus = valueAfterUpgrade;
                    } break;
            }
            currentConstruction = Buildings.None;
            constructionTurns = -1;
        }
    }

    private int GetImpactOfTaxesOnHappiness()
    {
        switch (taxLevel)
        {
            case 0: return 10;
            case 1: return 0;
            case 2: return -10;
        }

        return 0;
    }
}
public enum ResourceTypes
{
    Wood,
    Gold,
    Stone
}

public enum Buildings
{
    StoneQuarry,
    Farm,
    LumberMill,
    Walls,
    Stronghold,
    None
}
