using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Resources : MonoBehaviour
{
    public int turn = 1;
    public int woodNumber = 0;
    public int stoneNumber = 0;
    public int goldNumber = 0;

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
        turn++;
        UpdateResources();
        _turnCounter.text = $"Tura: {turn}";
        _woodCounter.text = $"{woodNumber}";
        _stoneCounter.text = $"{stoneNumber}";
        _goldCounter.text = $"{goldNumber}";
    }

    private void UpdateResources()
    {
        woodNumber += Provinces.listOfProvinces.Where(x => !x.isLost).Sum(x => x.woodGain);
        stoneNumber += Provinces.listOfProvinces.Where(x => !x.isLost).Sum(x => x.stoneGain);
        goldNumber += Provinces.listOfProvinces.Where(x => !x.isLost).Sum(x => x.taxGain);
    }
}
