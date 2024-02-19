using UnityEngine;

public abstract class BuildingFactory : MonoBehaviour
{
    [SerializeField] protected BuildingType _buildingType;

    public virtual BuildingType GetBuildingType() => _buildingType;
    public abstract void Initialize();
    public abstract Building CreateBuilding(Vector2 pos);
}
