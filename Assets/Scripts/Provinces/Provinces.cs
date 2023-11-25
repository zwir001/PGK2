
using System.Collections.Generic;

public static class Provinces
{
    public static List<Province> listOfProvince = new()
    {
        new Province(0, "Asturica", 13, 10, 10),
        new Province(1, "Corduba", 13, 10, 10),
        new Province(2, "Osca", 10, 13, 10),
        new Province(3, "Massalia", 10, 13, 10),
        new Province(4, "Alesia", 13, 10, 10),
        new Province(5, "Londinium", 10, 10, 13),
        new Province(6, "Deva", 13, 10, 10),
        new Province(7, "Batavodurum", 10, 13, 10),
        new Province(8, "Patavium", 10, 13, 10),
        new Province(9, "Rzym", 10, 10, 13),
        new Province(10, "Syrakuza", 10, 10, 13),
        new Province(11, "Segestica", 13, 10, 10),
        new Province(12, "Thessalonika", 10, 13, 10),
        new Province(13, "Ateny", 10, 10, 13),
        new Province(14, "Konstantynopol", 10, 10, 13)
    };
}

public class Province
{
    public int id;
    public string name;
    public int woodGain;
    public int stoneGain;
    public int foodBalance;
    public int taxGain;
    public int happinessLevel;

    public int farmLevel;
    public int StoneQuarryLevel;
    public int lumberMillLevel;
    public int wallsLevel;
    public int strongholdLevel;

    public Province(int id, string name, int woodGain, int stoneGain, int taxGain)
    {
        this.id = id;
        this.name = name;
        this.woodGain = woodGain;
        this.stoneGain = stoneGain;
        this.foodBalance = 0;
        this.taxGain = taxGain;
        this.happinessLevel = 100;
        this.farmLevel = 1;
        StoneQuarryLevel = 1;
        this.lumberMillLevel = 1;
        this.wallsLevel = 1;
        this.strongholdLevel = 1;
    }
}
