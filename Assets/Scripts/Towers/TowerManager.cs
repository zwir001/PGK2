using System.Linq;
using TMPro;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [Header("Tower")]
    [SerializeField] private Tower _towerInfo; // the tower cost, name etc.

    [Header("Upgrades")]
    [SerializeField] private float _upgradeCostMoc;
    [SerializeField] private float _upgradeCostIncreaseMoc;
    [SerializeField] private float _upgradeCostPrzeladowanie;
    [SerializeField] private float _upgradeCostIncreasePrzeladowanie;
    [SerializeField] private float _upgradeCostOptyka;
    [SerializeField] private float _upgradeCostIncreaseOptyka;

    private int _upgradeIndexMoc;
    private int _upgradeIndexPrzeladowanie;
    private int _upgradeIndexOptyka;

    private int _onClickCounter = 0;

    // upgrade Panel
    private GameObject _upgradePanelToShow;
    private TextMeshProUGUI _towerName;
    private GameObject _upgradeButtonMoc;
    private GameObject _upgradeButtonPrzeladowanie;
    private GameObject _upgradeButtonOptyka;

    // sell
    private int _moneyToAdd;
    private int _towerValue;
    private int _towerCost;

    private void Start()
    {
        _upgradePanelToShow = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Upgrade Bar");
        _towerName = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "TowerName").GetComponent<TextMeshProUGUI>();

        _upgradeButtonMoc = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Moc");
        _upgradeButtonPrzeladowanie = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Przeladowanie");
        _upgradeButtonOptyka = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Optyka");

        _upgradeIndexMoc = 0;
        _upgradeIndexPrzeladowanie = 0;
        _upgradeIndexOptyka = 0;

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

            var tower = Statistics.Instance.GetClickedTower();
            var towerManager = tower.GetComponent<TowerManager>();

            _upgradeButtonMoc.GetComponentInChildren<TextMeshProUGUI>().text = "Moc (" + (_upgradeIndexMoc + 1) + "): " + _upgradeCostMoc;
            _upgradeButtonPrzeladowanie.GetComponentInChildren<TextMeshProUGUI>().text = "Prze³adowanie (" + (_upgradeIndexPrzeladowanie + 1) + "): " + _upgradeCostPrzeladowanie;
            _upgradeButtonOptyka.GetComponentInChildren<TextMeshProUGUI>().text = "Optyka (" + (_upgradeIndexOptyka + 1) + "): " + _upgradeCostOptyka;

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

    public void UpgradeMoc()
    {
        if (Statistics.Instance.GetMoneyAmount() >= _upgradeCostMoc)
        {
            var towerToUpgrade = Statistics.Instance.GetClickedTower();

            var bullets = towerToUpgrade.GetComponent<AttackTower>().GetBullets();
            foreach (var bullet in bullets)
            {
                if (bullet.GetComponent<BulletLuk>() != null)
                    bullet.GetComponent<BulletLuk>().SetDamage(0.1f);
                if (bullet.GetComponent<BulletPlomien>() != null)
                    bullet.GetComponent<BulletPlomien>().SetDamage(0.1f);
                if (bullet.GetComponent<BulletKatapulta>() != null)
                    bullet.GetComponent<BulletKatapulta>().SetDamage(0.1f);
            }

            var towerManager = towerToUpgrade.GetComponent<TowerManager>();

            towerManager._upgradeIndexMoc++;
            towerManager._upgradeCostMoc += towerManager._upgradeCostMoc * towerManager._upgradeCostIncreaseMoc;

            towerManager._upgradeButtonMoc = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Moc");

            towerManager._upgradeButtonMoc.GetComponentInChildren<TextMeshProUGUI>().text = 
                "Moc (" + (towerManager._upgradeIndexMoc + 1) + "): " + towerManager._upgradeCostMoc;
        }
    }

    public void UpgradePrzeladowanie()
    {
        if (Statistics.Instance.GetMoneyAmount() >= _upgradeCostPrzeladowanie)
        {
            var towerToUpgrade = Statistics.Instance.GetClickedTower();

            if (towerToUpgrade.GetComponent<AttackTower>() != null)
                towerToUpgrade.GetComponent<AttackTower>().SetSpeed(0.1f);

            var towerManager = towerToUpgrade.GetComponent<TowerManager>();

            towerManager._upgradeIndexPrzeladowanie++;
            towerManager._upgradeCostPrzeladowanie += towerManager._upgradeCostPrzeladowanie * towerManager._upgradeCostIncreasePrzeladowanie;

            towerManager._upgradeButtonPrzeladowanie = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Przeladowanie");

            towerManager._upgradeButtonPrzeladowanie.GetComponentInChildren<TextMeshProUGUI>().text = 
                "Prze³adowanie (" + (towerManager._upgradeIndexPrzeladowanie + 1) + "): " + towerManager._upgradeCostPrzeladowanie;
        }
    }

    public void UpgradeOptyka()
    {
        if (Statistics.Instance.GetMoneyAmount() >= _upgradeCostOptyka)
        {
            var towerToUpgrade = Statistics.Instance.GetClickedTower();

            if (towerToUpgrade.GetComponent<Range>() != null)
                towerToUpgrade.GetComponent<Range>().SetRadius(0.1f);

            var towerManager = towerToUpgrade.GetComponent<TowerManager>();

            towerManager._upgradeIndexOptyka++;
            towerManager._upgradeCostOptyka += towerManager._upgradeCostOptyka * towerManager._upgradeCostIncreaseOptyka;

            towerManager._upgradeButtonOptyka = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Optyka");

            towerManager._upgradeButtonOptyka.GetComponentInChildren<TextMeshProUGUI>().text = "Optyka (" + (towerManager._upgradeIndexOptyka + 1) + "): " + towerManager._upgradeCostOptyka;
        }
    }

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
