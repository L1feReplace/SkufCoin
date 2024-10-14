using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TaskController : MonoBehaviour
{
    [System.Serializable]
    public class Task
    {
        public string taskName;  // Название задания
        public Sprite taskImage; // Картинка задания
        public string taskId;    // Уникальный идентификатор задания
    }

    public List<Task> tasksList;  // Список всех возможных заданий
    public Transform contentParent;  // Родительский объект для UI (например, ScrollView content)
    public GameObject taskPrefab;    // Префаб элемента задания
    public CharacterClicker characterClicker; // Ссылка на CharacterClicker

    private const string TASKS_KEY = "generatedTasks";   // Ключ для сохранения сгенерированных заданий
    private const string GENERATION_DATE_KEY = "tasksGenerationDate";  // Ключ для даты последней генерации

    void Start()
    {
        GenerateOrLoadTasks();
    }

    // Генерация заданий или загрузка сгенерированных заданий
    void GenerateOrLoadTasks()
    {
        // Проверяем, если ли сгенерированные задания и была ли их генерация в течение последнего дня
        if (PlayerPrefs.HasKey(GENERATION_DATE_KEY))
        {
            DateTime lastGenerationDate = DateTime.Parse(PlayerPrefs.GetString(GENERATION_DATE_KEY));

            // Если с момента последней генерации прошли сутки, перегенерируем задания
            if ((DateTime.Now - lastGenerationDate).TotalHours >= 24)
            {
                GenerateNewTasks();
            }
            else
            {
                // Иначе загружаем существующие задания
                LoadGeneratedTasks();
            }
        }
        else
        {
            // Если это первый запуск, генерируем задания
            GenerateNewTasks();
        }
    }

    // Метод для генерации новых заданий и их сохранения
    void GenerateNewTasks()
    {
        // Перемешиваем список заданий
        List<Task> shuffledTasks = new List<Task>(tasksList);
        ShuffleList(shuffledTasks);

        // Запоминаем первые 6 (или меньше, если в списке меньше 6 заданий)
        int tasksToShow = Mathf.Min(6, shuffledTasks.Count);
        List<string> generatedTaskIds = new List<string>();

        // Удаляем все старые элементы UI
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < tasksToShow; i++)
        {
            Task task = shuffledTasks[i];
            generatedTaskIds.Add(task.taskId);

            // Отображаем задание на UI
            CreateTaskUI(task);
        }

        // Сохраняем сгенерированные задания и дату генерации
        PlayerPrefs.SetString(TASKS_KEY, string.Join(",", generatedTaskIds));
        PlayerPrefs.SetString(GENERATION_DATE_KEY, DateTime.Now.ToString());
        PlayerPrefs.Save();

        Debug.Log("Сгенерированы новые задания.");
    }

    // Метод для загрузки ранее сгенерированных заданий
    void LoadGeneratedTasks()
    {
        // Удаляем старые элементы UI
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Загружаем сохраненные задания
        string[] savedTaskIds = PlayerPrefs.GetString(TASKS_KEY).Split(',');

        foreach (string taskId in savedTaskIds)
        {
            Task task = tasksList.Find(t => t.taskId == taskId);
            if (task != null)
            {
                CreateTaskUI(task);
            }
        }

        Debug.Log("Задания загружены.");
    }

    // Метод для создания UI задания
    void CreateTaskUI(Task task)
    {
        GameObject taskObject = Instantiate(taskPrefab, contentParent);

        // Устанавливаем текст и картинку
        taskObject.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = task.taskName;
        taskObject.transform.Find("icon").GetComponent<Image>().sprite = task.taskImage;

        // Настраиваем кнопку
        Button taskButton = taskObject.transform.Find("Button").GetComponent<Button>();

        if (IsTaskCompletedWithinLastDay(task.taskId))
        {
            taskButton.interactable = false;
            taskButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Завершено";
        }
        else
        {
            taskButton.onClick.AddListener(() => OnTaskComplete(task, taskButton));
        }
    }

    // Метод для перемешивания списка
    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    // Метод для выполнения задания
    void OnTaskComplete(Task completedTask, Button taskButton)
    {
        Debug.Log("Задание выполнено: " + completedTask.taskName);

        // Сохраняем текущее время выполнения задания
        PlayerPrefs.SetString(completedTask.taskId, DateTime.Now.ToString());
        PlayerPrefs.Save();

        // Обновляем кнопку
        taskButton.interactable = false;
        taskButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Завершено";

        // Добавляем очки к счету
        characterClicker.AddScore(10); // Увеличиваем счет на 1 (или другое количество по вашему выбору)
    }

    // Проверяем, выполнено ли задание за последние 24 часа
    bool IsTaskCompletedWithinLastDay(string taskId)
    {
        if (PlayerPrefs.HasKey(taskId))
        {
            DateTime lastCompletedTime;
            if (DateTime.TryParse(PlayerPrefs.GetString(taskId), out lastCompletedTime))
            {
                return (DateTime.Now - lastCompletedTime).TotalHours < 24;
            }
        }
        return false;
    }
}
