using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectionUI : MonoBehaviour
{
    public static ItemSelectionUI Instance;

    [SerializeField] private GameObject _panelContainer;
    [SerializeField] private GameObject _itemButtonPrefab;
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private Button _closeButton;

    private InventorySlot _currentSlot;
    private List<GameObject> _itemButtons = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        _panelContainer.SetActive(false);
        _closeButton.onClick.AddListener(Close);
    }

    public void OpenForSlot(InventorySlot slot)
    {
        _currentSlot = slot;
        _panelContainer.SetActive(true);

        var availableItems = GetItemsForSlot(slot.SlotType);
        PopulateItems(availableItems);
    }

    private void PopulateItems(List<ItemData> items)
    {
        foreach (var btn in _itemButtons)
            Destroy(btn);
        _itemButtons.Clear();

        foreach (var item in items)
        {
            var btn = Instantiate(_itemButtonPrefab, _itemsParent);
            var itemButton = btn.GetComponent<ItemSelectionButton>();
            itemButton.Initialize(item, OnItemSelected);
            _itemButtons.Add(btn);
        }
    }

    private List<ItemData> GetItemsForSlot(ItemSlotType slotType)
    {
        var result = new List<ItemData>();
        var allCollections = Resources.LoadAll<ItemCollection>("ItemCollections");

        foreach (var collection in allCollections)
        {
            foreach (var slot in collection.compatibleSlots)
            {
                if (slot == slotType)
                {
                    result.AddRange(collection.items);
                    break;
                }
            }
        }

        return result;
    }

    private void OnItemSelected(ItemData item)
    {
        if (_currentSlot == null) return;

        var lineScript = _currentSlot.GetComponentInParent<LineScript>();
        if (lineScript == null) return;

        var lineData = lineScript.GetLineData();
        if (lineData == null) return;

        var conflicts = EquipmentManager.GetConflictingSlots(item, lineData.GetEquipmentDictionary());

        foreach (var conflictSlot in conflicts)
        {
            lineData.ClearSlot(conflictSlot);
            // Обновляем UI для очищенного слота
            var slots = lineScript.GetComponentsInChildren<InventorySlot>();
            foreach (var slot in slots)
            {
                if (slot.SlotType == conflictSlot)
                {
                    slot.Clear();
                }
            }
        }

        _currentSlot.SetItem(item);

        SaveManager.Instance?.SaveData();
        Close();
    }

    public void Close()
    {
        _panelContainer.SetActive(false);
        _currentSlot = null;
    }
}