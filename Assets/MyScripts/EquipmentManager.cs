using System.Collections.Generic;
using UnityEngine;

public static class EquipmentManager
{
    public static bool CanEquipItem(ItemData item, Dictionary<ItemSlotType, ItemData> currentEquipment)
    {
        if (item == null) return false;

        if (currentEquipment.ContainsKey(item.slot) && currentEquipment[item.slot] != null)
            return false;

        if (item.slot == ItemSlotType.RightHand && item.isTwoHanded)
        {
            if (currentEquipment.ContainsKey(ItemSlotType.LeftHand) &&
                currentEquipment[ItemSlotType.LeftHand] != null)
                return false;
        }

        if (item.slot == ItemSlotType.LeftHand)
        {
            if (currentEquipment.ContainsKey(ItemSlotType.RightHand) &&
                currentEquipment[ItemSlotType.RightHand] != null &&
                currentEquipment[ItemSlotType.RightHand].isTwoHanded)
                return false;
        }

        return true;
    }

    public static List<ItemSlotType> GetConflictingSlots(ItemData item, Dictionary<ItemSlotType, ItemData> currentEquipment)
    {
        var conflicts = new List<ItemSlotType>();

        if (item == null) return conflicts;

        if (item.slot == ItemSlotType.RightHand && item.isTwoHanded)
        {
            if (currentEquipment.ContainsKey(ItemSlotType.LeftHand) &&
                currentEquipment[ItemSlotType.LeftHand] != null)
                conflicts.Add(ItemSlotType.LeftHand);
        }

        if (item.slot == ItemSlotType.LeftHand)
        {
            if (currentEquipment.ContainsKey(ItemSlotType.RightHand) &&
                currentEquipment[ItemSlotType.RightHand] != null &&
                currentEquipment[ItemSlotType.RightHand].isTwoHanded)
                conflicts.Add(ItemSlotType.RightHand);
        }

        return conflicts;
    }
}