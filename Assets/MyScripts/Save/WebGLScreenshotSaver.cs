using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class WebGLScreenshotSaver : MonoBehaviour
{
    [SerializeField] private RectTransform scrollContent; // Контейнер со строками
    [SerializeField] private RectTransform screenshotCanvas;
    [SerializeField] private Camera mainCamera;              // Камера, которая рендерит UI

    // Импорт функции из JSLIB
    [DllImport("__Internal")]
    private static extern void DownloadFileFromWebGL(IntPtr arrayPtr, int size, string fileName);

    public void TakeLongUIScreenshot()
    {
        StartCoroutine(CaptureRoutine());
    }

    private IEnumerator CaptureRoutine()
    {
        // 2. Запоминаем исходное место и настройки ScrollView
        Transform originalParent = scrollContent.parent;
        int originalSiblingIndex = scrollContent.GetSiblingIndex();
        Vector2 originalPosition = scrollContent.anchoredPosition;

        // 3. ПЕРЕКИДЫВАЕМ В НОВЫЙ КАНВАС И ЦЕНТРИРУЕМ
        scrollContent.SetParent(screenshotCanvas, false);
        scrollContent.anchoredPosition = Vector2.zero; // Сброс позиции в центр

        // Принудительно обновляем UI внутри нового Canvas
        Canvas.ForceUpdateCanvases();

        int uiLayer = LayerMask.NameToLayer("ListUI"); // Замените на имя вашего слоя
        mainCamera.cullingMask = 1 << uiLayer;

        yield return new WaitForEndOfFrame();

        Canvas.ForceUpdateCanvases();
        int width = Mathf.CeilToInt(scrollContent.rect.width);
        int height = Mathf.CeilToInt(scrollContent.rect.height);

        if (width <= 0 || height <= 0) yield break;

        // Создаем RenderTexture
        RenderTexture rt = new RenderTexture(width, height, 24);
        RenderTexture previousRT = mainCamera.targetTexture;

        // Временно перенаправляем основную камеру в текстуру
        mainCamera.targetTexture = rt;
        mainCamera.Render(); // Рендерим текущий кадр целиком в текстуру

        // Читаем пиксели
        RenderTexture.active = rt;
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGBA32, false);
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        // Возвращаем всё как было. Игрок даже не заметит моргания
        mainCamera.targetTexture = previousRT;
        RenderTexture.active = null;
        Destroy(rt);

        // 4. Кодируем в PNG
        byte[] pngBytes = screenshot.EncodeToPNG();
        Destroy(screenshot);

        scrollContent.SetParent(originalParent, false);
        scrollContent.anchoredPosition = originalPosition;
        scrollContent.SetSiblingIndex(originalSiblingIndex);

        mainCamera.cullingMask = -1;

        // 5. Отправляем в браузер для скачивания
#if UNITY_WEBGL && !UNITY_EDITOR
        GCHandle handle = GCHandle.Alloc(pngBytes, GCHandleType.Pinned);
        try
        {
            DownloadFileFromWebGL(handle.AddrOfPinnedObject(), pngBytes.Length, "screenshot.png");
        }
        finally
        {
            handle.Free();
        }
#else
        // Для теста в редакторе Unity
        System.IO.File.WriteAllBytes(Application.dataPath + "/screenshot.png", pngBytes);
        Debug.Log("Скриншот сохранен в папку Assets");
#endif


    }
}

