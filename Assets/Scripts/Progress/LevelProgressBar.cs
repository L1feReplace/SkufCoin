using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Для управления сценами
using EasyUI.Popup; // Для использования попапов

public class LevelProgressBar : MonoBehaviour
{
    public TextMeshProUGUI levelText; // Ссылка на TextMeshPro для отображения уровня
    public Slider progressBar; // Ссылка на прогресс-бар

    private int currentLevel = 1; // Текущий уровень
    private int currentClicks = 0; // Количество текущих кликов
    public int clicksForNextLevel = 10; // Количество кликов для перехода на следующий уровень
    public int levelToLoadNextScene = 5; // Уровень для перехода на следующую сцену

    // Метод, вызываемый при клике на объект
    public void OnObjectClick()
    {
        currentClicks++; // Увеличиваем количество кликов
        UpdateProgressBar(); // Обновляем прогресс-бар

        if (currentClicks >= clicksForNextLevel)
        {
            LevelUp(); // Переходим на следующий уровень
        }
    }

    // Метод для обновления прогресс-бара
    void UpdateProgressBar()
    {
        float progress = (float)currentClicks / clicksForNextLevel;
        progressBar.value = progress; // Обновляем значение слайдера
    }

    // Метод для перехода на следующий уровень
    void LevelUp()
    {
        currentLevel++; // Увеличиваем уровень
        levelText.text = "LEVEL: " + currentLevel; // Обновляем текст уровня
        currentClicks = 0; // Сбрасываем количество кликов
        clicksForNextLevel += 10; // Увеличиваем количество кликов для следующего уровня

        UpdateProgressBar(); // Сбрасываем прогресс-бар

        // Проверяем, достигнут ли необходимый уровень для перехода на следующую сцену
        if (currentLevel >= levelToLoadNextScene)
        {
            ShowLevelUpPopup(); // Показываем попап
        }
    }
    // Метод для показа попапа и загрузки следующей сцены
    void ShowLevelUpPopup()
    {
        Popup.Show("Успех", "Вы достигли уровня " + levelToLoadNextScene + ".", "OK", PopupColor.Green, OnPopupClose);
    }


    // Метод, вызываемый при закрытии попапа
    private void OnPopupClose()
    {
        LoadNextScene(); // Загрузка следующей сцены
    }

    // Метод для загрузки следующей сцены
    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Загружаем следующую сцену по индексу
    }
}
