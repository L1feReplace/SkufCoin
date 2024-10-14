using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EasyUI.Popup;

public class EnergyManager : MonoBehaviour
{
    public Image[] energyIcons; // Массив иконок энергии
    public Sprite fullEnergyIcon; // Спрайт для полной энергии
    public Sprite emptyEnergyIcon; // Спрайт для пустой энергии
    public int clicksPerEnergy = 10; // Количество кликов для расхода одной энергии
    public float energyRestoreTime = 5f; // Время для восстановления одной энергии

    private int currentEnergy; // Текущий уровень энергии
    private int currentClicks = 0; // Текущее количество кликов
    private Coroutine restoreCoroutine; // Для управления корутиной восстановления энергии

    void Start()
    {
        currentEnergy = energyIcons.Length; // Начальная энергия равна количеству иконок
        for (int i = 0; i < currentEnergy; i++)
        {
            energyIcons[i].sprite = fullEnergyIcon; // Устанавливаем полные иконки при старте
        }
    }

    public void OnObjectClick()
    {
        if (currentEnergy > 0)
        {
            currentClicks++; // Увеличиваем количество кликов

            if (currentClicks >= clicksPerEnergy)
            {
                UseEnergy(); // Используем одну единицу энергии
                currentClicks = 0; // Сбрасываем счетчик кликов
            }
        }
    }

    void UseEnergy()
    {
        currentEnergy--; // Уменьшаем количество энергии
        energyIcons[currentEnergy].sprite = emptyEnergyIcon; // Заменяем иконку на пустую

        // Запускаем корутину восстановления энергии, если она исчерпана
        if (currentEnergy == 0)
        {
            Debug.Log("Energy depleted!"); // Выводим сообщение, когда энергия закончилась
            Popup.Show("Энергия закончилась, возвращайтесь через некоторое время");
            if (restoreCoroutine == null) // Проверяем, запущена ли корутина
            {
                restoreCoroutine = StartCoroutine(RestoreEnergy()); // Запускаем восстановление энергии
            }
        }
        else if (restoreCoroutine == null) // Проверяем, нужно ли запустить восстановление энергии
        {
            restoreCoroutine = StartCoroutine(RestoreEnergy()); // Запускаем восстановление энергии
        }
    }

    // Метод для проверки, есть ли еще энергия
    public bool HasEnergy()
    {
        return currentEnergy > 0;
    }

    void Update()
    {
        // Восстанавливаем энергию, если текущая энергия меньше максимума и корутина не запущена
        if (currentEnergy < energyIcons.Length && restoreCoroutine == null)
        {
            restoreCoroutine = StartCoroutine(RestoreEnergy());
        }
    }

    IEnumerator RestoreEnergy()
    {
        while (currentEnergy < energyIcons.Length)
        {
            yield return new WaitForSeconds(energyRestoreTime); // Ждем определенное время

            // Восстанавливаем иконку, только если есть возможность
            energyIcons[currentEnergy].sprite = fullEnergyIcon; // Восстанавливаем иконку
            currentEnergy++; // Увеличиваем количество энергии
        }

        restoreCoroutine = null; // Освобождаем корутину, когда восстановление завершено
    }
}
