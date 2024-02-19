using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEditor.U2D.Path;
using UnityEngine;

public class SelectionController : PlayerController
{
    [SerializeField] private Transform _selectionAreaTransform;
    private Pathfinding _pathfinding;

    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private List<Unit> _selectedUnits = new();
    private Building _selectedBuilding;

    public event Action<Building> BuildingSelected;
    public event Action<Unit> UnitSelected;

    private bool _insideGrid;

    public void Initialize()
    {
        _pathfinding = new(GridController);
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

        if (Input.GetMouseButtonUp(0) && _insideGrid)
        {
            _endPosition = MousePos;
            _selectionAreaTransform.gameObject.SetActive(false);

            _selectedUnits.ForEach(unit =>
            {
                unit.GetSelected(false);
            });
            _selectedUnits.Clear();

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
                    BuildingSelected?.Invoke(null);
                }
            }
            else
            {
                _selectedBuilding = colliders.Select(collider => collider.GetComponent<Building>()).LastOrDefault(building => building != null); //Son noktaya uzaklýk?
                BuildingSelected?.Invoke(_selectedBuilding);
                if (_selectedBuilding != null)
                {
                    _selectedBuilding.GetSelected(true);
                }
            }
        }
    }

    private void Update()
    {
        if (!IsActive) return;
        MousePos = GetMousePos();

        _insideGrid = GridController.IsMouseInGrid(MousePos);

        HandleSelection();

        if (Input.GetMouseButtonDown(1))
        {
            var clickedObject = Physics2D.OverlapPoint(MousePos);
            if(clickedObject != null)
            {
                clickedObject.TryGetComponent(out IDamagable damagable);
                _selectedUnits.ForEach(unit =>
                {
                    unit.MoveToTarget(MousePos, damagable);
                });
            }
            else
            {
                var numRows = Mathf.CeilToInt((float)_selectedUnits.Count / 5);
                var numCols = Mathf.Min(5, _selectedUnits.Count);
                var spacing = GridController.GetCellRadius();
                // Calculate the total width and height of the box
                var boxWidth = (numCols - 1) * spacing;
                var boxHeight = (numRows - 1) * spacing;
                for (int i = 0; i < _selectedUnits.Count; i++)
                {
                    var unit = _selectedUnits[i];
                    var row = i / 5;
                    var col = i % 5;

                    // Calculate the position of the current object within the box
                    var objectPos = MousePos + new Vector2(col * spacing, row * spacing);
                    // Move the object to its calculated position
                    
                    unit.MoveToTarget(objectPos);
                };
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
        base.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if( _selectedBuilding != null ) { _selectedBuilding.GetSelected(false); }
        _selectionAreaTransform.transform.position = MousePos;
        _startPosition = Vector2.zero;
        _endPosition = Vector2.zero;
    }
}
