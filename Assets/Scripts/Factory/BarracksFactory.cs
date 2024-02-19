using UnityEngine;

public class BarracksFactory : BuildingFactory
{
    [SerializeField] private Barracks _barracksPrefab;
    [SerializeField] private int _initialPoolSize;
    [SerializeField] private int _expandSize;

    public override void Initialize()
    {
        new ObjectPooler<Barracks>(_barracksPrefab, _initialPoolSize, _expandSize, transform);
    }

    public override Building CreateBuilding(Vector2 pos)
    {
        var building = ObjectPooler<Barracks>.Instance.SpawnFromPool(pos);
        return building;
    }


}
