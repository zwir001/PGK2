using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Resources : MonoBehaviour
{
    public int turn = 1;
    [SerializeField]
    private TMP_Text _turnCounter;
    
    public void NextTurn()
    {
        turn++;
        _turnCounter.text = $"Tura: {turn}";
    }
}
