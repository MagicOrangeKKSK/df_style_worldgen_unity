using UnityEngine;

[CreateAssetMenu]
public class GovernmentSO : ScriptableObject
{
    public string Name;
    public string Description;
    public int Aggressiveness;
    public float Militarization;
    public float TechBonus;
}