using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<BuildingSelectButton> _buildingButtonList = new();
    [SerializeField] private InfoPanelManager _infoPanelManager;

    public event Action<BuildingType> OnBuildingButtonPressed;
    public event Action<UnitType> OnUnitButtonPressed;

    public void Initialize(List<BuildingDataSO> buildingDataArray)
    {
        _infoPanelManager.Initialize();
        _infoPanelManager.OnUnitButtonPressed += UnitButtonPressed;
        for (int i = 0; i < _buildingButtonList.Count; i++)
        {
            var button = _buildingButtonList[i];
            var buildingData = buildingDataArray[i];

            button.Initialize(buildingData.buildingSprite);
            button.BuildingButtonPressed += BuildingButtonPressed;

        }
    }

    private void BuildingButtonPressed(BuildingType buildingType)
    {
        OnBuildingButtonPressed?.Invoke(buildingType);
        _infoPanelManager.CloseInformationPanel();
    }

    private void UnitButtonPressed(UnitType unitType)
    {
        OnUnitButtonPressed?.Invoke(unitType);
    }

    public void OpenInfoPanel(Building building)
    {
        _infoPanelManager.CloseInformationPanel();

        if (building == null) return;

        _infoPanelManager.OpenInformationPanel(building.GetBuildingData());
    }
}
