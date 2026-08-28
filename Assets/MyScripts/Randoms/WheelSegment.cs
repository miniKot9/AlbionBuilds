using UnityEngine;
using UnityEngine.UI;

public class WheelSegment : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    private Sprite itemSprite;
    private float angle;
    private int index;

    public void Setup(Sprite sprite, float segmentAngle, int segmentIndex)
    {
        itemSprite = sprite;
        angle = segmentAngle;
        index = segmentIndex;

        // Настройка размера
        RectTransform rect = GetComponent<RectTransform>();
        if (rect != null)
        {
            // Для сегмента используем Rect как сектор
            // Можно настроить через Image с типом Filled
        }

        // Находим иконку
        if (iconImage == null)
            iconImage = GetComponentInChildren<Image>();

        if (iconImage != null)
            iconImage.sprite = sprite;
    }

    public Sprite GetSprite()
    {
        return itemSprite;
    }

    public float GetAngle()
    {
        return angle;
    }

    public int GetIndex()
    {
        return index;
    }
}