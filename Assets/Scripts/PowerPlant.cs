
public class PowerPlant : Building
{
    public override void Deactivate()
    {
        ObjectPooler<PowerPlant>.Instance.ReturnToPool(this);
        base.Deactivate();
    }
}
