using UnityEngine;

public class Unit1Factory : UnitFactory
{
    [SerializeField] private Unit1 _unit1Prefab;
    [SerializeField] private int _initialPoolSize;
    [SerializeField] private int _expandSize;

    public override void Initialize()
    {
        new ObjectPooler<Unit1>(_unit1Prefab, _initialPoolSize, _expandSize, transform);
    }

    public override Unit CreateUnit(Vector2 pos)
    {
        var unit = ObjectPooler<Unit1>.Instance.SpawnFromPool(pos);
        return unit;
    }
}
