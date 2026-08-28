using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [SerializeField] private string _saveFileName = "equipment_data.dat";
    private SavedData _currentData;
    private string _savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _savePath = Path.Combine(Application.persistentDataPath, _saveFileName);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadData();
    }

    public void SaveData()
    {
        if (_currentData == null) return;

        try
        {
            string json = JsonUtility.ToJson(_currentData, true);
            File.WriteAllText(_savePath, json);
            Debug.Log($"Data saved to: {_savePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving data: {e.Message}");
        }
    }

    public void LoadData()
    {
        if (File.Exists(_savePath))
        {
            try
            {
                string json = File.ReadAllText(_savePath);
                _currentData = JsonUtility.FromJson<SavedData>(json);
                Debug.Log("Data loaded successfully");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error loading data: {e.Message}");
                CreateDefaultData();
            }
        }
        else
        {
            Debug.Log("No save file found, creating default data");
            CreateDefaultData();
        }
    }

    private void CreateDefaultData()
    {
        _currentData = new SavedData();
        _currentData.LinesData = new LineData[1];
        _currentData.LinesData[0] = new LineData();
        SaveData();
    }

    public SavedData GetCurrentData()
    {
        return _currentData;
    }

    public void UpdateLinesData(LineData[] linesData)
    {
        if (_currentData != null)
        {
            _currentData.LinesData = linesData;
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveData();
        }
    }
}