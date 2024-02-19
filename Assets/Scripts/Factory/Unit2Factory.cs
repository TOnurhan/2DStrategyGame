using UnityEngine;

public class Unit2Factory : UnitFactory
{
    [SerializeField] private Unit2 _unit2Prefab;
    [SerializeField] private int _initialPoolSize;
    [SerializeField] private int _expandSize;

    public override void Initialize()
    {
        new ObjectPooler<Unit2>(_unit2Prefab, _initialPoolSize, _expandSize, transform);
    }

    public override Unit CreateUnit(Vector2 pos)
    {
        var unit = ObjectPooler<Unit2>.Instance.SpawnFromPool(pos);
        return unit;
    }
}
