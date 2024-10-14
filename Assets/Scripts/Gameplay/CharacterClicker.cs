using UnityEngine;
using TMPro;

public class CharacterClicker : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Используем TextMeshPro для UI текста
    private int score = 0;

    public LevelProgressBar levelProgressBar;
    public EnergyManager energyManager; // Ссылка на EnergyManager

    public void OnImageClick() // Этот метод можно назначить через Inspector
    {
        // Проверяем, есть ли энергия перед выполнением действий
        if (energyManager.HasEnergy())
        {
            AddScore(1); // Добавляем 1 к счету при клике
            levelProgressBar.OnObjectClick();
            energyManager.OnObjectClick(); // Обновляем энергию при клике
        }
        else
        {
            Debug.Log("Энергия закончилась!"); // Выводим сообщение, если энергия закончилась
        }
    }

    public void AddScore(int amount) // Метод теперь принимает количество очков
    {
        score += amount; // Увеличиваем счет на заданное количество
        scoreText.text = "Заработано скуф \r\nкойнов: " + score.ToString(); // Обновляем текст
    }
}
