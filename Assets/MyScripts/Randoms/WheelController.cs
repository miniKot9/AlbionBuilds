using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WheelController : MonoBehaviour
{
    [Header("Настройки колеса")]
    [SerializeField] private RectTransform wheelRect;
    [SerializeField] private GameObject segmentPrefab;
    [SerializeField] private float spinDuration = 7f;
    [SerializeField] private int minSpins = 5;
    [SerializeField] private int maxSpins = 10;

    [Header("Списки иконок")]
    [SerializeField] private List<ItemList> itemLists;
    [SerializeField] private int selectedListIndex = 0;

    [Header("Результат")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Image resultImage;
    [SerializeField] private Text resultText;
    [SerializeField] private Button spinButton;

    [Header("События")]
    public UnityEvent onSpinStart;
    public UnityEvent<Sprite> onSpinEnd;

    [System.Serializable]
    public class ItemList
    {
        public string listName;
        public List<Sprite> items;
        public Color sectionColor = Color.white;
    }

    private List<WheelSegment> segments = new List<WheelSegment>();
    private bool isSpinning = false;
    private float currentAngle = 0f;

    void Start()
    {
        if (resultPanel != null)
            resultPanel.SetActive(false);

        if (spinButton != null)
            spinButton.onClick.AddListener(Spin);

        BuildWheel();
    }

    public void BuildWheel()
    {
        // Очищаем колесо
        foreach (Transform child in wheelRect)
        {
            Destroy(child.gameObject);
        }
        segments.Clear();

        if (itemLists.Count == 0 || selectedListIndex >= itemLists.Count)
            return;

        var currentList = itemLists[selectedListIndex];
        int itemCount = currentList.items.Count;

        if (itemCount == 0)
            return;

        float anglePerSegment = 360f / itemCount;

        for (int i = 0; i < itemCount; i++)
        {
            GameObject segmentObj = Instantiate(segmentPrefab, wheelRect);
            RectTransform segmentRect = segmentObj.GetComponent<RectTransform>();

            // Настройка сегмента
            WheelSegment segment = segmentObj.GetComponent<WheelSegment>();
            if (segment == null)
                segment = segmentObj.AddComponent<WheelSegment>();

            segment.Setup(currentList.items[i], anglePerSegment, i);
            segments.Add(segment);

            // Настройка UI элемента
            Image segmentImage = segmentObj.GetComponent<Image>();
            if (segmentImage != null)
            {
                segmentImage.color = GetColorForIndex(i, currentList.sectionColor);
                segmentImage.raycastTarget = false;
            }

            // Поворот сегмента
            float startAngle = i * anglePerSegment + anglePerSegment / 2f;
            segmentRect.localRotation = Quaternion.Euler(0, 0, -startAngle);

            // Настройка иконки
            Transform iconTransform = segmentObj.transform.Find("Icon");
            if (iconTransform != null)
            {
                Image iconImage = iconTransform.GetComponent<Image>();
                if (iconImage != null && currentList.items[i] != null)
                {
                    iconImage.sprite = currentList.items[i];
                    iconImage.raycastTarget = false;
                }
            }
        }
    }

    Color GetColorForIndex(int index, Color baseColor)
    {
        float hue = (index * 0.15f + 0.5f) % 1f;
        return Color.HSVToRGB(hue, 0.7f, 0.9f);
    }

    public void Spin()
    {
        if (isSpinning || segments.Count == 0)
            return;

        if (resultPanel != null)
            resultPanel.SetActive(false);

        isSpinning = true;
        if (spinButton != null)
            spinButton.interactable = false;

        onSpinStart?.Invoke();

        int randomIndex = Random.Range(0, segments.Count);
        int spins = Random.Range(minSpins, maxSpins + 1);

        // Вычисляем целевой угол
        float targetAngle = currentAngle + spins * 360f - (randomIndex * 360f / segments.Count);

        StartCoroutine(SpinCoroutine(targetAngle, randomIndex));
    }

    IEnumerator SpinCoroutine(float targetAngle, int winIndex)
    {
        float startAngle = currentAngle;
        float elapsed = 0f;

        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / spinDuration;

            // Easing с замедлением
            float easedT = 1f - Mathf.Pow(1f - t, 3f);

            currentAngle = Mathf.Lerp(startAngle, targetAngle, easedT);
            wheelRect.localRotation = Quaternion.Euler(0, 0, currentAngle);

            yield return null;
        }

        currentAngle = targetAngle;
        wheelRect.localRotation = Quaternion.Euler(0, 0, currentAngle);

        // Показываем результат
        ShowResult(winIndex);

        isSpinning = false;
        if (spinButton != null)
            spinButton.interactable = true;

        onSpinEnd?.Invoke(segments[winIndex].GetSprite());
    }

    void ShowResult(int index)
    {
        if (resultPanel == null || resultImage == null)
            return;

        resultPanel.SetActive(true);
        resultImage.sprite = segments[index].GetSprite();

        if (resultText != null)
            resultText.text = "Вы выиграли!";
    }

    public void SelectList(int index)
    {
        if (isSpinning)
            return;

        selectedListIndex = Mathf.Clamp(index, 0, itemLists.Count - 1);
        BuildWheel();
    }

    public void SelectListByName(string name)
    {
        for (int i = 0; i < itemLists.Count; i++)
        {
            if (itemLists[i].listName == name)
            {
                SelectList(i);
                return;
            }
        }
    }
}