using UnityEngine;
using UnityEngine.SceneManagement; // Для загрузки сцен
using EasyUI.Popup; // Подключаем библиотеку для работы с Popup

public class LevelController : MonoBehaviour
{
    public int[] levelThresholds; // Массив пороговых значений для перехода на новый уровень
    private int currentLevel = 0; // Текущий уровень

    // Метод для проверки уровня
    public void CheckLevelUp(int score)
    {
        // Проверяем, достиг ли игрок порога для перехода на следующий уровень
        if (currentLevel < levelThresholds.Length && score >= levelThresholds[currentLevel])
        {
            LevelUp(); // Переход на новый уровень
        }
    }

    private void LevelUp()
    {
        Popup.Show("Success", "Your account updated successfully.", "OK", PopupColor.Green, OnPopupClose);
    }

    private void OnPopupClose()
    {
        currentLevel++; // Увеличиваем уровень

        // Проверяем, достигли ли мы последнего уровня
        if (currentLevel < levelThresholds.Length)
        {
            Debug.Log($"Переход на уровень {currentLevel + 1}");
            SceneManager.LoadScene(currentLevel); // Загружаем новую сцену
        }
        else
        {
            Debug.Log("Все уровни пройдены!");
            // Здесь можно добавить дополнительную логику, если все уровни пройдены
        }
    }
}
