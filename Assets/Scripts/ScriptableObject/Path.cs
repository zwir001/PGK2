using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Path")]
public class Path : ScriptableObject
{
    public GameObject spawn;
    public List<GameObject> points;
}
