using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SavedData
{
	public LineData[] LinesData;

}

[System.Serializable]
public class LineData
{
	public Sprite[] Sprites = new Sprite[8];

}

