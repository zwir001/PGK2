using System.Linq;
using TMPro;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [Header("Tower")]
    [SerializeField] private Tower _towerInfo; // the tower cost, name etc.

    [Header("Upgrades")]
    //[SerializeField] private List<UpgradeData> _upgrades;
    private int _upgradeIndex;

    private int _onClickCounter = 0;

    // upgrade Panel
    private GameObject _upgradePanelToShow;
    private TextMeshProUGUI _towerName;
    private GameObject _upgradeButton;

    // sell
    private int _moneyToAdd;
    private int _towerValue;
    private int _towerCost;

    private void Start()
    {
        _upgradePanelToShow = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Upgrade Bar");
        _towerName = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "TowerName").GetComponent<TextMeshProUGUI>();
        //_upgradeButton = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "UpgradeButton");
        //_upgradeIndex = 0;

        var standardCost = _towerInfo.towerPrice;
        _towerCost = standardCost;
        _towerValue = _towerCost;
        _moneyToAdd = _towerValue;
    }

    public void ShowUpgradePanel()
    {
        if (!_upgradePanelToShow.activeSelf)
        {
            var _towerRange = gameObject.transform.Find("Range");
            _towerRange.GetComponent<SpriteRenderer>().enabled = true;

            _upgradePanelToShow.SetActive(true);
            _towerName.text = _towerInfo.towerName;

            //if (_upgradeindex >= _upgrades.count)
            //    _upgradebutton.transform.getchild(0).gameobject.getcomponent<textmeshprougui>().text = "max upgrades";
            //else
            //{
            //    var leveldifficulty = playerstats.instance.getleveldifficulty();
            //    _upgradebutton.transform.getchild(0).gameobject.getcomponent<textmeshprougui>().text = " (" + _upgrades[_upgradeindex].upgradenumber + ") " +
            //        mathf.roundtoint(_upgrades[_upgradeindex].upgradecost +
            //        _upgrades[_upgradeindex].upgradecost * leveldifficulty.upgradecost) + "$" + ":\n" +
            //        _upgrades[_upgradeindex].upgradedescription;
            //}
        }
        else
        {
            var towerRange = gameObject.transform.Find("Range");
            towerRange.GetComponent<SpriteRenderer>().enabled = false;

            _upgradePanelToShow.SetActive(false);
        }
    }

    public void SellTower()
    {
        Statistics.Instance.DeleteInstantiatedTower();
        Statistics.Instance.ForgetClickedTower();

        Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Upgrade Bar").SetActive(false);
    }

    //public void Upgrade()
    //{
    //    var levelDifficulty = PlayerStats.Instance.GetLevelDifficulty();

    //    var towerToUpgrade = PlayerStats.Instance.GetClickedTower();
    //    if (towerToUpgrade.GetComponent<ManageTower>()._upgradeIndex < towerToUpgrade.GetComponent<ManageTower>()._upgrades.Count)
    //    {
    //        var bullets = towerToUpgrade.GetComponent<TowerAttack>().GetBullets();
    //        var upgrade = towerToUpgrade.GetComponent<ManageTower>()._upgrades[towerToUpgrade.GetComponent<ManageTower>()._upgradeIndex];

    //        if (PlayerStats.Instance.GetMoneyAmount() >= Mathf.RoundToInt(upgrade.upgradeCost + upgrade.upgradeCost * levelDifficulty.upgradeCost))
    //        {
    //            // upgrade bullets
    //            foreach (var bullet in bullets)
    //            {
    //                if (bullet.GetComponent<Projectile>() != null)
    //                {
    //                    var dart = bullet.GetComponent<Projectile>();
    //                    dart.UpgradeBullet(upgrade);
    //                }
    //                else if (bullet.GetComponent<BulletSet>() != null)
    //                {
    //                    var bulletSet = bullet.GetComponent<BulletSet>();
    //                    bulletSet.UpgradeBullet(upgrade);
    //                }
    //            }

    //            // upgrade tower data
    //            towerToUpgrade.GetComponent<TowerAttack>().SetDelay(upgrade.delay);

    //            if (towerToUpgrade.GetComponent<MonkeyAceAttack>() != null)
    //                towerToUpgrade.transform.Find("MonkeyAceSprite").GetComponent<SpriteRenderer>().sprite = upgrade.upgradeSprite;
    //            else
    //                towerToUpgrade.GetComponentInChildren<SpriteRenderer>().sprite = upgrade.upgradeSprite;

    //            if (towerToUpgrade.GetComponent<RangeCollider>() != null)
    //                towerToUpgrade.GetComponent<RangeCollider>().SetRadius(upgrade.radius);

    //            towerToUpgrade.GetComponent<TowerAttack>().SetDamage(upgrade.damage);

    //            if (towerToUpgrade.GetComponentInChildren<OnMouseTowerRotation>() != null)
    //                towerToUpgrade.GetComponentInChildren<OnMouseTowerRotation>().SetRotationSpeed(upgrade.rotationSpeed);
    //            if (towerToUpgrade.GetComponent<SniperMonkeyAttack>() != null)
    //                towerToUpgrade.GetComponent<SniperMonkeyAttack>().SetCannotPopLead(upgrade.cannotPopLead);

    //            PlayerStats.Instance.DecreaseMoneyForBoughtTower(Mathf.RoundToInt(upgrade.upgradeCost + upgrade.upgradeCost * levelDifficulty.upgradeCost));
    //            ChangeSellCost(towerToUpgrade, upgrade);

    //            towerToUpgrade.GetComponent<ManageTower>()._upgradeIndex++;
    //            if (towerToUpgrade.GetComponent<ManageTower>()._upgradeIndex >= towerToUpgrade.GetComponent<ManageTower>()._upgrades.Count)
    //                towerToUpgrade.GetComponent<ManageTower>()._upgradeButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text
    //                    = "MAX UPGRADES";
    //            else
    //            {
    //                upgrade = towerToUpgrade.GetComponent<ManageTower>()._upgrades[towerToUpgrade.GetComponent<ManageTower>()._upgradeIndex];
    //                towerToUpgrade.GetComponent<ManageTower>()._upgradeButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text
    //                    = " (" + upgrade.upgradeNumber + ") " + Mathf.RoundToInt(upgrade.upgradeCost + upgrade.upgradeCost * levelDifficulty.upgradeCost) + "$" + ":\n" + upgrade.upgradeDescription;
    //            }

    //            SoundManager.Instance.PlaySound(_upgradeTowerSound);
    //        }
    //    }
    //}

    //////////////////////////////
    // Getters and setters
    //////////////////////////////

    public Tower GetTowerInfo()
    {
        return _towerInfo;
    }

    public int GetNumberOfClicks()
    {
        return _onClickCounter;
    }

    public void Click()
    {
        _onClickCounter++;
    }
}
