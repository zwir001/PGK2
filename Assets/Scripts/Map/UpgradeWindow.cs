using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        var province = Resources.listOfProvinces.FirstOrDefault(x => x.id == provinceId);
        if (province.isAttacked)
        {
            //call another scene
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
