using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    [SerializeField] private GameObject _tooltipPanel;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    private void Awake()
    {
        Instance = this;
        _tooltipPanel.SetActive(false);
    }

    public void ShowTooltip(ItemData item, Vector3 position)
    {
        if (item == null) return;

        _tooltipPanel.SetActive(true);
        _tooltipPanel.transform.position = position + new Vector3(150, 0, 0);
        
        _nameText.text = item.itemName;
        _descriptionText.text = item.description ?? "";
        
    }

    public void HideTooltip()
    {
        _tooltipPanel.SetActive(false);
    }
}