using UnityEngine;

public class Unit3Factory : UnitFactory
{
    [SerializeField] private Unit3 _unit3Prefab;
    [SerializeField] private int _initialPoolSize;
    [SerializeField] private int _expandSize;

    public override void Initialize()
    {
        new ObjectPooler<Unit3>(_unit3Prefab, _initialPoolSize, _expandSize, transform);
    }

    public override Unit CreateUnit(Vector2 pos)
    {
        var unit = ObjectPooler<Unit3>.Instance.SpawnFromPool(pos);
        return unit;
    }
}
