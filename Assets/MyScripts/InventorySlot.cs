using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private ItemSlotType _slotType;

    private ItemData _currentItem;
    private LineData _lineData;
    private int _slotIndex;

    public ItemData CurrentItem => _currentItem;
    public ItemSlotType SlotType => _slotType;
    public bool IsEmpty => _currentItem == null;

    public void Initialize(ItemSlotType slotType, int index, LineData lineData)
    {
        _slotType = slotType;
        _slotIndex = index;
        _lineData = lineData;
        Clear();
    }

    public void SetItem(ItemData item)
    {
        _currentItem = item;
        if (item != null && item.icon != null)
        {
            _iconImage.sprite = item.icon;
            _iconImage.color = Color.white;
            _iconImage.gameObject.SetActive(true);
        }
        else
        {
            Clear();
        }

        _lineData?.UpdateSlot(_slotIndex, item);
    }

    public void Clear()
    {
        _currentItem = null;
        _iconImage.sprite = null;
        _iconImage.color = new Color(1, 1, 1, 0);
        _iconImage.gameObject.SetActive(false);

        _lineData?.UpdateSlot(_slotIndex, null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ItemSelectionUI.Instance?.OpenForSlot(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_highlight != null)
            _highlight.SetActive(true);

        if (_currentItem != null)
        {
            TooltipManager.Instance?.ShowTooltip(_currentItem, transform.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_highlight != null)
            _highlight.SetActive(false);

        TooltipManager.Instance?.HideTooltip();
    }
}