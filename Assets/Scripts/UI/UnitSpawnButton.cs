using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawnButton : MonoBehaviour
{
    public event Action<UnitType> UnitButtonPressed;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _nameText;

    public void Initialize(Sprite buildingSprite, UnitType unitType)
    {
        _buttonImage.sprite = buildingSprite;
        _nameText.text = unitType.ToString();
        _button.onClick.AddListener(() =>
        {
            UnitButtonPressed?.Invoke(unitType);
        });
    }

    public void Dispose()
    {
        _button.onClick.RemoveAllListeners();
    }
}
