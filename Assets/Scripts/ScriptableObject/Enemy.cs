using UnityEngine;

[CreateAssetMenu(menuName = "New enemy details")]
public class Enemy : ScriptableObject
{
    public bool immuneToArrows;
    public bool immuneToSiegeArtillery;
    public bool immuneToFire;
    public bool vulnerableToArrows;
    public bool vulnerableToSiegeArtillery;
    public bool vulnerableToFire;
}
