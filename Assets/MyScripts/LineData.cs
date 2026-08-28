using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LineData
{
    public string[] ItemIds = new string[8];

    public LineData()
    {
        for (int i = 0; i < ItemIds.Length; i++)
        {
            ItemIds[i] = "";
        }
    }

    public void UpdateSlot(int index, ItemData item)
    {
        if (index < 0 || index >= ItemIds.Length) return;

        if (item != null)
        {
            ItemIds[index] = item.name;
        }
        else
        {
            ItemIds[index] = "";
        }
    }

    public Dictionary<ItemSlotType, ItemData> GetEquipmentDictionary()
    {
        var dict = new Dictionary<ItemSlotType, ItemData>();
        var slotTypes = Enum.GetValues(typeof(ItemSlotType));

        for (int i = 0; i < ItemIds.Length && i < slotTypes.Length; i++)
        {
            if (!string.IsNullOrEmpty(ItemIds[i]))
            {
                var item = Resources.Load<ItemData>($"Items/{ItemIds[i]}");
                if (item != null)
                {
                    dict[(ItemSlotType)slotTypes.GetValue(i)] = item;
                }
            }
        }

        return dict;
    }

    public void ClearSlot(ItemSlotType slot)
    {
        int index = (int)slot;
        if (index < ItemIds.Length)
        {
            ItemIds[index] = "";
        }
    }
}