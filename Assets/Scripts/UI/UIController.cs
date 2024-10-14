using UnityEngine;

public class UIController : MonoBehaviour
{
    // Ссылка на панель
    public GameObject _dailiesPanel;

    // Метод для переключения видимости панели
    public void TogglePanel()
    {
        if (_dailiesPanel != null)
        {
            // Переключаем состояние панели
            _dailiesPanel.SetActive(!_dailiesPanel.activeSelf);
        }
    }
}
