using UnityEngine;

[CreateAssetMenu(menuName = "BuildingData")]
public class BuildingDataSO : ScriptableObject
{
    public Sprite buildingSprite;
    public Vector2Int buildingSize;
    public float buildingHealth;
    public BuildingType buildingType;
    public UnitDataSO[] unitSettings;
}
