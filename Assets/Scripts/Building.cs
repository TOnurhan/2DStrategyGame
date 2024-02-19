using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour, IDamagable
{
    [SerializeField] private BuildingDataSO _buildingData;
    [SerializeField] protected SpriteRenderer _selectionSprite;
    [SerializeField] protected HealthBar _healthBar;
    protected float _currentHealth;
    public event Action<Building> BuildingDestroyed;
    public List<CellData> CellDataList;

    public BuildingDataSO GetBuildingData() => _buildingData;

    public void Initialize(Action<Building> actionOnDestroy)
    {
        _selectionSprite.gameObject.SetActive(false);
        _currentHealth = _buildingData.buildingHealth;
        _healthBar.Initialize();
        BuildingDestroyed += actionOnDestroy;
    }

    public void GetDamage(float damage)
    {
        _healthBar.DecreaseHealth(_currentHealth, damage);
        _currentHealth -= damage;
        if( _currentHealth <= 0 )
        {
            Deactivate();
        }
    }

    public void GetSelected(bool isSelected)
    {
        _selectionSprite.gameObject.SetActive(isSelected);
    }

    public virtual void Deactivate()
    {
        _healthBar.Dispose();
        BuildingDestroyed?.Invoke(this);
    }

    public bool IsAlive()
    {
        return _currentHealth > 0 && enabled;
    }

    public void ChangeHealth(float damage)
    {
        var health = transform.localScale;
        var value = (_currentHealth - damage) / 10;
        health.x = value;
        transform.localScale = health;
    }
}
