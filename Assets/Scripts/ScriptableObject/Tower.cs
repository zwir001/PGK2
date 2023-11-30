using UnityEngine;

[CreateAssetMenu(menuName = "New Tower Info")]
public class Tower : ScriptableObject
{
    public string towerName;
    public int towerPrice;
    public string towerSpeed;
    [TextArea] public string towerDescription;

}
