using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSelectionButton : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _typeText;
    [SerializeField] private Button _button;

    private ItemData _item;
    private System.Action<ItemData> _onSelect;

    public void Initialize(ItemData item, System.Action<ItemData> onSelect)
    {
        _item = item;
        _onSelect = onSelect;

        if (item != null)
        {
            _iconImage.sprite = item.icon;
            _nameText.text = item.itemName;
            _typeText.text = item.weaponType != WeaponType.None ? item.weaponType.ToString() : item.slot.ToString();
            
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => _onSelect?.Invoke(_item));
        }
    }
}