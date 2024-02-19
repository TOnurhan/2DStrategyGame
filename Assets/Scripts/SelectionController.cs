using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class SelectionController : PlayerController
{
    [SerializeField] private Transform _selectionAreaTransform;
    [SerializeField] private int _formUnitPerRow;
    [SerializeField] private int _formSpacingBetweenUnits;

    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private List<Unit> _selectedUnits = new();
    private Building _selectedBuilding;

    public event Action<Building> BuildingSelected;

    private bool _insideGrid;

    public void Initialize()
    {
        _selectionAreaTransform.gameObject.SetActive(false);
    }

    private void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0) && _insideGrid)
        {
            _startPosition = MousePos;
            _selectionAreaTransform.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(0) && _insideGrid)
        {
            var lowerLeft = new Vector3(
                Mathf.Min(_startPosition.x, MousePos.x),
                Mathf.Min(_startPosition.y, MousePos.y));
            var upperRight = new Vector3(
                Mathf.Max(_startPosition.x, MousePos.x),
                Mathf.Max(_startPosition.y, MousePos.y));

            _selectionAreaTransform.position = lowerLeft;
            _selectionAreaTransform.localScale = upperRight - lowerLeft;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _endPosition = MousePos;
            _selectionAreaTransform.gameObject.SetActive(false);

            _selectedUnits.ForEach(unit =>
            {
                unit.GetSelected(false);
            });
            _selectedUnits.Clear();

            if (!_insideGrid) return;
            if (_selectedBuilding != null) _selectedBuilding.GetSelected(false);
            _selectedBuilding = null;

            var colliders = Physics2D.OverlapAreaAll(_startPosition, _endPosition);
            var allSelectedUnits = colliders.Select(collider => collider.GetComponent<Unit>()).Where(unit => unit != null).ToArray();
            if (allSelectedUnits.Length > 0)
            {
                foreach (var selectedUnit in allSelectedUnits)
                {
                    _selectedUnits.Add(selectedUnit);
                    selectedUnit.GetSelected(true);
                }
                BuildingSelected?.Invoke(null);

            }
            else
            {
                _selectedBuilding = colliders.Select(collider => collider.GetComponent<Building>()).LastOrDefault(building => building != null);
                BuildingSelected?.Invoke(_selectedBuilding);
                if (_selectedBuilding != null)
                {
                    _selectedBuilding.GetSelected(true);
                }
            }

            _startPosition = _endPosition;
        }
    }

    private void Update()
    {
        if (!IsActive) return;
        MousePos = GetMousePos();

        _insideGrid = GridController.IsMouseInGrid(MousePos);

        HandleSelection();
        ManageUnits();  
    }

    public void ManageUnits()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var clickedObject = Physics2D.OverlapPoint(MousePos);
            if (clickedObject != null)
            {
                clickedObject.TryGetComponent(out IDamagable damagable);
                _selectedUnits.ForEach(unit =>
                {
                    unit.MoveToTarget(MousePos, damagable);
                });
            }
            else
            {
                UnitsFormation(_selectedUnits);
            }
        }
    }
    public override void Dispose()
    {
        base.Dispose();
    }

    public void PrepareToSpawn(UnitFactory unitFactory)
    {
        var cell = GridController.GridData.GetCellFromWorldPosition(_selectedBuilding.transform.position);
        var unit = unitFactory.CreateUnit(cell.Pos);
        unit.Initialize(new Pathfinding(GridController));
        unit.MoveToTarget(GridController.GridData.GetRandomNeighbour(cell).Pos);
    }

    public override void Activate()
    {
        _startPosition = _endPosition;
        base.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if( _selectedBuilding != null ) { _selectedBuilding.GetSelected(false); }
        _selectionAreaTransform.transform.position = MousePos;

    }

    public void UnitsFormation(List<Unit> selectedUnits)
    {
        int numRows = Mathf.CeilToInt((float)selectedUnits.Count / _formUnitPerRow);
        int numCols = Mathf.Min(_formUnitPerRow, selectedUnits.Count);

        float boxWidth = (numCols - 1) * _formSpacingBetweenUnits;
        float boxHeight = (numRows - 1) * _formSpacingBetweenUnits;

        Vector3 startPos = (Vector3)MousePos - new Vector3(boxWidth / 2, boxHeight / 2, 0);

        for (int i = 0; i < selectedUnits.Count; i++)
        {
            int row = i / _formUnitPerRow;
            int col = i % _formUnitPerRow;

            Vector3 objectPos = startPos + new Vector3(col * _formSpacingBetweenUnits, row * _formSpacingBetweenUnits, 0);

            selectedUnits[i].MoveToTarget(objectPos);
        }
    }
}
