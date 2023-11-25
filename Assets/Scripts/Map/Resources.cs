
using System.Collections.Generic;

public static class Resources
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
    public ResourceTypes bonusResource;
    public int farmLevel;
    public int StoneQuarryLevel;
    public int lumberMillLevel;
    public int wallsLevel;
    public int strongholdLevel;

    public Province(int id, string name, int woodGain, int stoneGain, int taxGain, ResourceTypes resource)
    {
        this.id = id;
        this.name = name;
        this.woodGain = woodGain;
        this.stoneGain = stoneGain;
        foodGain = 10;
        foodNeed = 10;
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
    }
}

public enum ResourceTypes
{
    Wood,
    Gold,
    Stone
}
