using UnityEngine;

public abstract class UnitFactory : MonoBehaviour
{
    [SerializeField] protected UnitType _unitType;

    public virtual UnitType GetUnitType() => _unitType;
    public abstract void Initialize();
    public abstract Unit CreateUnit(Vector2 pos);
}
