using UnityEngine;

[CreateAssetMenu(fileName = "ItemCollection", menuName = "Inventory/Item Collection")]
public class ItemCollection : ScriptableObject
{
    public string collectionName;
    public ItemData[] items;
    public ItemSlotType[] compatibleSlots;
}