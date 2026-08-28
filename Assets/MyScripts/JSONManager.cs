using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class JSONManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _savedDataText;
    [SerializeField] private TextMeshProUGUI _loadDataText;
    [SerializeField] private GameObject _saveLoadPannel;
    [SerializeField] private GameObject _errorPanel;

    [SerializeField] private MainList _mainList;

    [SerializeField] private FileSaveHelper _fileSaver;

    public SavedData currentData = new SavedData();


    public void SaveDataToJSON()
    {
        try
        {
            string jsonData = JsonUtility.ToJson(currentData, true);
            _fileSaver.SaveTextFile("Builds", jsonData);
            //_savedDataText.text = jsonData;
            QuickSave();
        }
        catch (Exception e)
        {
            _errorPanel.SetActive(true);
        }
    }

    // Загрузка данных из JSON
    public void LoadDataFromJSON()
    {
        string jsonData = _loadDataText.text;
        try
        {
            currentData = JsonUtility.FromJson<SavedData>(jsonData);
        }
        catch (Exception e)
        {
            _errorPanel.SetActive(true);
        }
    }

    // Загрузка данных по умолчанию
    public void LoadDefaultData()
    {
        currentData = JsonUtility.FromJson<SavedData>(PlayerPrefs.GetString("quicksave"));
        if (currentData != null)
            _mainList.LoadData(currentData);
    }


    public void OnSaveButtonClick()
    {
        SaveDataToJSON();
    }
    public void OnLoadButtonClick()
    {
        LoadDataFromJSON();
    }


    public void QuickSave()
    {
        string jsonData = JsonUtility.ToJson(currentData, true);
        PlayerPrefs.SetString("quicksave", jsonData);
        PlayerPrefs.Save();
    }
}
