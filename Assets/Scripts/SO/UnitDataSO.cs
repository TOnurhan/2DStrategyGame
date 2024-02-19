using UnityEngine;

[CreateAssetMenu(menuName = "UnitData")]
public class UnitDataSO : ScriptableObject
{
    //public UnitBehaviour unitPrefab;
    public Sprite unitSprite;
    public float unitAttack;
    public float unitHealth;
    public UnitType unitType;
    public float UnitSpeed;
    public float AttackCooldown;
}
