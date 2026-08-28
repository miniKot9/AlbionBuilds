using System.Collections.Generic;
using UnityEngine;

public class MainList : MonoBehaviour
{
    [SerializeField] private GameObject _linePrefab;
    [SerializeField] private Transform _lineParent;
    [SerializeField] private float _lineHeight = 120f;

    private List<LineScript> _lines = new List<LineScript>();

    private void Start()
    {
        LoadAllLines();
    }

    private void LoadAllLines()
    {
        var data = SaveManager.Instance?.GetCurrentData();
        if (data == null || data.LinesData == null) return;

        foreach (var lineData in data.LinesData)
        {
            CreateLineWithData(lineData);
        }
    }

    // Метод для создания новой строки (без данных) - вызывается из UI
    private void CreateLine()
    {
        CreateLineWithData(null);
    }

    // Основной метод создания строки с данными
    private void CreateLineWithData(LineData lineData = null)
    {
        var go = Instantiate(_linePrefab, _lineParent);
        var lineScript = go.GetComponent<LineScript>();

        int index = _lines.Count;
        lineScript.SetLineIndex(index);

        if (lineData != null)
        {
            lineScript.LoadLineData(lineData);
        }

        _lines.Add(lineScript);
        UpdateAllLines();
        SaveData();
    }

    private void RemoveLine(int index)
    {
        if (index < 0 || index >= _lines.Count) return;

        var line = _lines[index];
        _lines.RemoveAt(index);
        Destroy(line.gameObject);

        UpdateAllLines();
        SaveData();
    }

    private void SwapLines(int index1, int index2)
    {
        if (index1 < 0 || index1 >= _lines.Count ||
            index2 < 0 || index2 >= _lines.Count ||
            index1 == index2) return;

        var temp = _lines[index1];
        _lines[index1] = _lines[index2];
        _lines[index2] = temp;

        UpdateAllLines();
        SaveData();
    }

    private void UpdateAllLines()
    {
        for (int i = 0; i < _lines.Count; i++)
        {
            _lines[i].SetLineIndex(i);
            _lines[i].transform.SetSiblingIndex(i);
        }
        UpdateContentHeight();
    }

    private void UpdateContentHeight()
    {
        var rectTransform = _lineParent.GetComponent<RectTransform>();
        if (rectTransform == null) return;

        float totalHeight = (_lineHeight * _lines.Count) + 20f;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, totalHeight);
    }

    private void SaveData()
    {
        var linesData = new LineData[_lines.Count];
        for (int i = 0; i < _lines.Count; i++)
        {
            linesData[i] = _lines[i].GetLineData();
        }

        SaveManager.Instance?.UpdateLinesData(linesData);
        SaveManager.Instance?.SaveData();
    }

    private void OnEnable()
    {
        AllActions.CreateNewLine = CreateLine; // Теперь это Action без параметров
        AllActions.RemoveLine = RemoveLine;
        AllActions.SwapLines = SwapLines;
        AllActions.SaveData = SaveData;
    }

    private void OnDisable()
    {
        AllActions.CreateNewLine = null;
        AllActions.RemoveLine = null;
        AllActions.SwapLines = null;
        AllActions.SaveData = null;
    }
}