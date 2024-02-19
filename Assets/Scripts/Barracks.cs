
using UnityEngine;

public class Barracks : Building
{
    public override void Deactivate()
    {
        ObjectPooler<Barracks>.Instance.ReturnToPool(this);
        base.Deactivate();
    }
}
