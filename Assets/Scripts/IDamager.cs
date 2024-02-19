using System.Collections;

public interface IDamager
{
    public IEnumerator Attack(IDamagable damagable);
}
