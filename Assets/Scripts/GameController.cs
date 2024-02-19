using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GridController _gridController;
    [SerializeField] private BuildController _buildController;
    [SerializeField] private SelectionController _selectionController;
    [SerializeField] private UIManager _UIManager;
    [SerializeField] private List<BuildingDataSO> _buildingDataList;
    [SerializeField] private List<BuildingFactory> _buildingFactories;
    [SerializeField] private List<UnitFactory> _unitFactories;

    private PlayerState _currentState = PlayerState.None;
    private PlayerController _currentController;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {

        foreach (var buildingFactory in _buildingFactories)
        {
            buildingFactory.Initialize();
        };

        foreach (var unitFactory in _unitFactories)
        {
            unitFactory.Initialize();
        };
        _gridController.Initialize();
        _UIManager.Initialize(_buildingDataList);
        _buildController.Initialize();
        _buildController.BuildingCompleted += BuildingCompleted;
        _selectionController.Initialize();
        _selectionController.BuildingSelected += BuildingSelected;
        _UIManager.OnBuildingButtonPressed += UIBuildingButtonPressed;
        _UIManager.OnUnitButtonPressed += SpawnUnit;

        ChangeControlState(PlayerState.Select);
    }

    public void BuildingSelected(Building building)
    {
        _UIManager.OpenInfoPanel(building);
    }

    public void UIBuildingButtonPressed(BuildingType buildingType)
    {
        var foundData = _buildingDataList.Find(data => data.buildingType == buildingType);
        var factory = _buildingFactories.Find(building => building.GetBuildingType() == buildingType);
        _buildController.ChangeBuildSettings(foundData, factory);
        ChangeControlState(PlayerState.Build);
    }

    public void ChangeControlState(PlayerState state)
    {
        if(_currentState == state) return;

        if(_currentController != null) _currentController.Deactivate();
        _currentController = state switch
        {
            PlayerState.Build => _buildController,
            PlayerState.Select => _selectionController,
            _ => _selectionController
        };

        _currentController.Activate();
        _currentState = state;
    }

    public void BuildingCompleted()
    {
        ChangeControlState(PlayerState.Select);
    }

    public void SpawnUnit(UnitType unitType)
    {
        var factory = _unitFactories.Find(factory => factory.GetUnitType() == unitType);
        _selectionController.PrepareToSpawn(factory);
    }
}
