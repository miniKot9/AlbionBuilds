using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private MainList _mainList;
    [SerializeField] private GameObject _addButton;

    private void Start()
    {
        if (_addButton != null)
        {
            var button = _addButton.GetComponent<UnityEngine.UI.Button>();
            if (button != null)
                button.onClick.AddListener(() => AllActions.CreateNewLine?.Invoke());
        }
    }

    public void OnApplicationQuit()
    {
        AllActions.SaveData?.Invoke();
    }
}