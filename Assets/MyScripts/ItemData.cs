using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public string description;
    public Sprite icon;
    
    [Header("Slot")]
    public ItemSlotType slot;
    
    [Header("Weapon Properties (optional)")]
    public WeaponType weaponType = WeaponType.None;
    public bool isTwoHanded = false;
}

public enum ItemSlotType
{
    Head,
    Body,
    RightHand,
    LeftHand,
    Boots,
    Cloak,
    Potion,
    Food
}

public enum WeaponType
{
    None,
    Sword,
    Axe,
    Shield,
    Torch,
    Bow,
    Staff,
    Dagger,
    Spear
}