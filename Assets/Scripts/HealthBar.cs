using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _remainingHealthBar;

    public void Initialize()
    {
        gameObject.SetActive(false);
    }

    public void DecreaseHealth(float currentHealth, float damage)
    {
        gameObject.SetActive(true);

        var health = _remainingHealthBar.localScale;
        var value = (currentHealth - damage) / 100;
        health.x = value;
        _remainingHealthBar.localScale = health;
    }

    public void Dispose()
    {
        _remainingHealthBar.localScale = Vector3.one;
        gameObject.SetActive(false);
    }
}
