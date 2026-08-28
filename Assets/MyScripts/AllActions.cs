using System;

public static class AllActions
{
    public static Action CreateNewLine;
    public static Action<int> RemoveLine;
    public static Action<int, int> SwapLines;
    public static Action SaveData;
    public static Action<InventorySlot> OpenItemSelection;
}