using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradeWindow : MonoBehaviour
{
    [SerializeField]
    public int provinceId;

    [SerializeField]
    private GameObject upgradeWindow;

    [SerializeField]
    private GameObject clickableImages;

    [SerializeField]
    private GameObject nextTurnButton;

    public void OpenUpgradeWindow()
    {
        var province = MapResources.listOfProvinces.FirstOrDefault(x => x.id == provinceId);
        if (province.isAttacked && !province.isLost)
        {
            PlayerPrefs.SetFloat("attackBonus", (float)province.attackBonus);
            PlayerPrefs.SetInt("hpBonus", province.hpBonus);
            SceneManager.LoadScene("Rome");
        }
        else if (!province.isAttacked && !province.isLost)
        {
            clickableImages.SetActive(false);
            upgradeWindow.SetActive(true);
            nextTurnButton.SetActive(false);
            var component = upgradeWindow.GetComponent<UpgradeWindowManager>();
            component.OpenUpgradeWindowForProvince(province);
        }
    }
}
