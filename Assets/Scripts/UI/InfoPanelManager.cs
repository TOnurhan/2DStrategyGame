using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelManager : MonoBehaviour
{
    [SerializeField] private Image _buildingImage;
    [SerializeField] private TMP_Text _buildingText;
    [SerializeField] private RectTransform _infoPanel;
    [SerializeField] private List<UnitSpawnButton> _unitInfoList;

    public event Action<UnitType> OnUnitButtonPressed;

    public void Initialize()
    {
        _infoPanel.gameObject.SetActive(false);
    }

    public void OpenInformationPanel(BuildingDataSO buildingSettings)
    {
        _buildingImage.sprite = buildingSettings.buildingSprite;
        _buildingText.text = buildingSettings.buildingType.ToString();

        for (int i = 0; i < buildingSettings.unitSettings.Length; i++)
        {
            var unitInfo = _unitInfoList[i];
            var unitSettings = buildingSettings.unitSettings[i];

            unitInfo.Initialize(unitSettings.unitSprite,unitSettings.unitType);
            unitInfo.UnitButtonPressed += UnitButtonPressed;
            unitInfo.gameObject.SetActive(true);
        }

        _infoPanel.gameObject.SetActive(true);
    }

    public void CloseInformationPanel()
    {
        _infoPanel.gameObject.SetActive(false);
        _unitInfoList.ForEach(unitInfo =>
        {
            unitInfo.Dispose();
            unitInfo.UnitButtonPressed -= UnitButtonPressed;
            unitInfo.gameObject.SetActive(false);
        });
    }

    public void UnitButtonPressed(UnitType unitType)
    {
        OnUnitButtonPressed(unitType);
    }
}
