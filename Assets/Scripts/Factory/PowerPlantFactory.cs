using UnityEngine;

public class PowerPlantFactory : BuildingFactory
{
    [SerializeField] private PowerPlant _powerPlantPrefab;
    [SerializeField] private int _initialPoolSize;
    [SerializeField] private int _expandSize;

    public override void Initialize()
    {
        new ObjectPooler<PowerPlant>(_powerPlantPrefab, _initialPoolSize, _expandSize, transform);
    }

    public override Building CreateBuilding(Vector2 pos)
    {
        var building = ObjectPooler<PowerPlant>.Instance.SpawnFromPool(pos);
        return building;
    }


}
