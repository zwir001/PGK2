using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Description : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _descriptionPanel;
    private int _counter;

    private void Start()
    {
        _counter = 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _descriptionPanel.SetActive(true);
        var tower = gameObject.GetComponent<TowerUI>().GetTowerToPlace().GetComponent<TowerManager>().GetTowerInfo();

        if (_counter == 0)
        {
            _descriptionPanel.GetComponentInChildren<TextMeshProUGUI>().text = tower.towerName + 
                "\n" + "Koszt: " + tower.towerPrice + "\n" + "Szybkoœæ: " + tower.towerSpeed + "\n\n" + tower.towerDescription;
            _counter++;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _descriptionPanel.SetActive(false);
    }
}
