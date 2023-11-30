using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseUpgradeWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject upgradeWindow;

    [SerializeField]
    private GameObject clickableObjects;

    [SerializeField]
    private GameObject nextTurnButton;
    public void OnButtonClick()
    {
        clickableObjects.SetActive(true);
        upgradeWindow.SetActive(false);
        nextTurnButton.SetActive(true);
    }
}
