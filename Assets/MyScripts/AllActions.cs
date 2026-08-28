using System;
using UnityEngine;

public static class AllActions
{
	public static Action CreateNewLine;
	public static Action<int> RemoveLine;

	public static Action<int,GameObject> OpenItems;
	public static Action<Sprite> ItemTaked;
	public static Action CloseItems;
	//public static GameObject

	public static Action<int, int> ListItemsSwap;
}
