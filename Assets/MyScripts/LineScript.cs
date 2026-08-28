using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class LineScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _numberText;
    [SerializeField] private InventorySlot[] _slots;
    [SerializeField] private Button _removeButton;
    [SerializeField] private Button _moveUpButton;
    [SerializeField] private Button _moveDownButton;

    private int _lineIndex;
    private LineData _lineData;

    private void Awake()
    {
        _lineData = new LineData();

        var slotTypes = System.Enum.GetValues(typeof(ItemSlotType));
        for (int i = 0; i < _slots.Length && i < slotTypes.Length; i++)
        {
            _slots[i].Initialize((ItemSlotType)slotTypes.GetValue(i), i, _lineData);
        }

        if (_removeButton != null)
            _removeButton.onClick.AddListener(() => AllActions.RemoveLine?.Invoke(_lineIndex));

        if (_moveUpButton != null)
            _moveUpButton.onClick.AddListener(() => AllActions.SwapLines?.Invoke(_lineIndex, _lineIndex - 1));

        if (_moveDownButton != null)
            _moveDownButton.onClick.AddListener(() => AllActions.SwapLines?.Invoke(_lineIndex, _lineIndex + 1));
    }

    public void SetLineIndex(int index)
    {
        _lineIndex = index;
        _numberText.text = $"#{index + 1}";
        gameObject.name = $"Line_{index}";
    }

    public LineData GetLineData()
    {
        return _lineData;
    }

    public void LoadLineData(LineData data)
    {
        _lineData = data;
        for (int i = 0; i < _slots.Length && i < data.ItemIds.Length; i++)
        {
            if (!string.IsNullOrEmpty(data.ItemIds[i]))
            {
                var item = Resources.Load<ItemData>($"Items/{data.ItemIds[i]}");
                _slots[i].SetItem(item);
            }
        }
    }
}