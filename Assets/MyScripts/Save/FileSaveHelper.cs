using System.Runtime.InteropServices;
using UnityEngine;

public class FileSaveHelper : MonoBehaviour
{
    // Импортируем JavaScript-функцию с помощью DllImport
    [DllImport("__Internal")]
    private static extern void DownloadFile(string filename, string content);

    // Метод для вызова скачивания
    public void SaveTextFile(string filename, string textContent)
    {
        // Проверяем, что это WebGL-сборка, чтобы избежать ошибок в редакторе
#if UNITY_WEBGL == true && !UNITY_EDITOR
        DownloadFile(filename, textContent);
#else
        Debug.Log("Вызов сохранения файла доступен только в WebGL-сборке.");
#endif
    }
}
