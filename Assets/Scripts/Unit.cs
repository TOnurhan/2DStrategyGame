using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IDamagable, IDamager
{
    [SerializeField] protected SpriteRenderer _selectionSprite;
    [SerializeField] protected UnitDataSO _unitData;
    [SerializeField] protected UnitType _unitType;
    protected float _currentHealth;
    private int _currentPathIndex;
    private Pathfinding _pathfinding;

    public UnitType GetUnitType() => _unitType;

    public virtual void Initialize(Pathfinding pathFinding)
    {
        _currentHealth = _unitData.unitHealth;
        _selectionSprite.gameObject.SetActive(false);
        _pathfinding = pathFinding;
    }

    public void GetSelected(bool selected)
    {
        _selectionSprite.gameObject.SetActive(selected);
    }

    public void GetDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Deactivate();
        }
    }

    public IEnumerator Attack(IDamagable damagable)
    {
        if (!damagable.IsAlive()) yield break;
        damagable.GetDamage(_unitData.unitAttack);
        yield return new WaitForSeconds(_unitData.AttackCooldown);
        StartCoroutine(Attack(damagable));
    }

    private IEnumerator MoveToTarget(List<Vector2> pathList, IDamagable damagable = null)
    {
        while (pathList != null && pathList.Count > 0)
        {
            var targetPos = pathList[_currentPathIndex];
            if (Vector2.Distance(transform.position, targetPos) > 0.1f)
            {
                var moveDir = ((Vector3)targetPos - transform.position).normalized;

                transform.position += _unitData.UnitSpeed * Time.deltaTime * moveDir;
            }
            else
            {
                _currentPathIndex++;
                if (_currentPathIndex >= pathList.Count)
                {
                    pathList.Clear();
                    yield return null;
                }
            }
            yield return null;
        }

        if(pathList != null && damagable != null)
        {
            StartCoroutine(Attack(damagable));
        }
    }

    public void MoveToTarget(Vector2 targetPosition, IDamagable damagable = null)
    {
        _currentPathIndex = 0;
        var pathList = _pathfinding.FindPath(transform.position, targetPosition);
        StopAllCoroutines();
        StartCoroutine(MoveToTarget(pathList, damagable));
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public bool IsAlive()
    {
        return _currentHealth > 0;
    }
}
