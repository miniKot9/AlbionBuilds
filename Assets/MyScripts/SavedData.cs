using System;

[Serializable]
public class SavedData
{
    public LineData[] LinesData;
    public int LastLineIndex;

    public SavedData()
    {
        LinesData = new LineData[0];
        LastLineIndex = 0;
    }
}