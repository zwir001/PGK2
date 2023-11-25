using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private TMP_Text _turnCounter;
    [SerializeField]
    private TMP_Text _woodCounter;
    [SerializeField]
    private TMP_Text _stoneCounter;
    [SerializeField]
    private TMP_Text _goldCounter;
    
    public void NextTurn()
    {
        Resources.turn++;
        UpdateResources();
        _turnCounter.text = $"Tura: {Resources.turn}";
        _woodCounter.text = $"{Resources.woodNumber}";
        _stoneCounter.text = $"{Resources.stoneNumber}";
        _goldCounter.text = $"{Resources.goldNumber}";
    }

    private void UpdateResources()
    {
        Resources.woodNumber += Resources.listOfProvinces.Where(x => !x.isLost).Sum(x => x.woodGain);
        Resources.stoneNumber += Resources.listOfProvinces.Where(x => !x.isLost).Sum(x => x.stoneGain);
        Resources.goldNumber += Resources.listOfProvinces.Where(x => !x.isLost).Sum(x => x.taxGain);
    }
}
