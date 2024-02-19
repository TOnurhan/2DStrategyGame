using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSelectButton : MonoBehaviour
{
    public event Action<BuildingType> BuildingButtonPressed;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Button _button;
    [SerializeField] private BuildingType _buildingType;

    public void Initialize(Sprite buildingSprite)
    {
        _buttonImage.sprite = buildingSprite;
        _button.onClick.AddListener(() =>
        {
            BuildingButtonPressed?.Invoke(_buildingType);
        });
    }
}
