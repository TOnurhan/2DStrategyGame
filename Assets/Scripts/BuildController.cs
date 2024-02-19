using System;
using UnityEngine;

public class BuildController : PlayerController
{
    [SerializeField] private Transform _cellSelector;
    [SerializeField] private SpriteRenderer _cellSelectorSprite;

    [SerializeField] private Color errorColor;
    [SerializeField] private Color normalColor;

    private float _cellRadius;
    private CellData _previousCellData;
    private BuildingDataSO _buildingSettings;
    private bool _notPlacable;
    private bool _insideGrid;
    private Vector2 _cellSelectorScale;
    private BuildingFactory _factory;

    public event Action BuildingCompleted;

    public void Initialize()
    {
        _cellRadius = GridController.GetCellRadius();
        _cellSelector.localScale = Vector2.zero;
    }

    private void Update()
    {
        if(!IsActive) return;

        MousePos = GetMousePos();
        _insideGrid = GridController.IsMouseInGrid(MousePos);

        _cellSelector.localScale = _insideGrid ? _cellSelectorScale : Vector2.zero;

        var selectedCell = GridController.GridData.GetCellFromWorldPosition(MousePos);

        if (selectedCell == null) return;


        if (_previousCellData != selectedCell)
        {
            SelectBuildingCell(selectedCell);
        }

        if (Input.GetMouseButtonUp(0) && !_notPlacable)
        {
            _notPlacable = true;
            BuildOnCell(selectedCell);
        }
    }

    private void SelectBuildingCell(CellData selectedCell)
    {
        _notPlacable = false;
        for (int x = 0; x < _buildingSettings.buildingSize.x && !_notPlacable; x++)
        {
            for (int y = 0; y < _buildingSettings.buildingSize.y && !_notPlacable; y++)
            {
                var newX = selectedCell.Index.x + x;
                var newY = selectedCell.Index.y + y;

                var isIndexValid = GridController.GridData.IsCellValid(newX, newY);
                if (!isIndexValid)
                {
                    _notPlacable = true;
                    break;
                }
            }
        }

        _cellSelector.position = new Vector2(selectedCell.Pos.x - _cellRadius / 2, selectedCell.Pos.y - _cellRadius / 2);
        _cellSelectorSprite.color = _notPlacable ? errorColor : normalColor;
        _previousCellData = selectedCell;
    }
    private void BuildOnCell(CellData selectedCell)
    {
        var spawnedBuilding = _factory.CreateBuilding(selectedCell.Pos);

        spawnedBuilding.Initialize(DestroyBuilding);

        for (int x = 0; x < _buildingSettings.buildingSize.x; x++)
        {
            for (int y = 0; y < _buildingSettings.buildingSize.y; y++)
            {
                var newX = selectedCell.Index.x + x;
                var newY = selectedCell.Index.y + y;

                var cell = GridController.GridData.CellArray[newX, newY];
                cell.cellState = _buildingSettings.buildingType;
            }
        }

        BuildingCompleted?.Invoke();
        _previousCellData = null;
    }

    public void ChangeBuildSettings(BuildingDataSO buildingData, BuildingFactory factory)
    {
        _factory = factory;
        _buildingSettings = buildingData;
        _cellSelectorScale = Vector2.one * _buildingSettings.buildingSize * _cellRadius;
    }

    public override void Activate()
    {
        base.Activate();
    }
    public override void Deactivate()
    {
        _cellSelector.localScale = Vector2.zero;
        base.Deactivate();
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    public void DestroyBuilding(Building building)
    {
        var cell = GridController.GridData.GetCellFromWorldPosition(building.transform.position);
        building.BuildingDestroyed -= DestroyBuilding;
        for (int x = 0; x < _buildingSettings.buildingSize.x; x++)
        {
            for (int y = 0; y < _buildingSettings.buildingSize.y; y++)
            {
                var newX = cell.Index.x + x;
                var newY = cell.Index.y + y;

                GridController.GridData.CellArray[newX, newY].cellState = BuildingType.None;
            }
        }
    }
}
