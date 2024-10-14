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
        public string taskName;  // �������� �������
        public Sprite taskImage; // �������� �������
        public string taskId;    // ���������� ������������� �������
    }

    public List<Task> tasksList;  // ������ ���� ��������� �������
    public Transform contentParent;  // ������������ ������ ��� UI (��������, ScrollView content)
    public GameObject taskPrefab;    // ������ �������� �������
    public CharacterClicker characterClicker; // ������ �� CharacterClicker

    private const string TASKS_KEY = "generatedTasks";   // ���� ��� ���������� ��������������� �������
    private const string GENERATION_DATE_KEY = "tasksGenerationDate";  // ���� ��� ���� ��������� ���������

    void Start()
    {
        GenerateOrLoadTasks();
    }

    // ��������� ������� ��� �������� ��������������� �������
    void GenerateOrLoadTasks()
    {
        // ���������, ���� �� ��������������� ������� � ���� �� �� ��������� � ������� ���������� ���
        if (PlayerPrefs.HasKey(GENERATION_DATE_KEY))
        {
            DateTime lastGenerationDate = DateTime.Parse(PlayerPrefs.GetString(GENERATION_DATE_KEY));

            // ���� � ������� ��������� ��������� ������ �����, �������������� �������
            if ((DateTime.Now - lastGenerationDate).TotalHours >= 24)
            {
                GenerateNewTasks();
            }
            else
            {
                // ����� ��������� ������������ �������
                LoadGeneratedTasks();
            }
        }
        else
        {
            // ���� ��� ������ ������, ���������� �������
            GenerateNewTasks();
        }
    }

    // ����� ��� ��������� ����� ������� � �� ����������
    void GenerateNewTasks()
    {
        // ������������ ������ �������
        List<Task> shuffledTasks = new List<Task>(tasksList);
        ShuffleList(shuffledTasks);

        // ���������� ������ 6 (��� ������, ���� � ������ ������ 6 �������)
        int tasksToShow = Mathf.Min(6, shuffledTasks.Count);
        List<string> generatedTaskIds = new List<string>();

        // ������� ��� ������ �������� UI
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < tasksToShow; i++)
        {
            Task task = shuffledTasks[i];
            generatedTaskIds.Add(task.taskId);

            // ���������� ������� �� UI
            CreateTaskUI(task);
        }

        // ��������� ��������������� ������� � ���� ���������
        PlayerPrefs.SetString(TASKS_KEY, string.Join(",", generatedTaskIds));
        PlayerPrefs.SetString(GENERATION_DATE_KEY, DateTime.Now.ToString());
        PlayerPrefs.Save();

        Debug.Log("������������� ����� �������.");
    }

    // ����� ��� �������� ����� ��������������� �������
    void LoadGeneratedTasks()
    {
        // ������� ������ �������� UI
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // ��������� ����������� �������
        string[] savedTaskIds = PlayerPrefs.GetString(TASKS_KEY).Split(',');

        foreach (string taskId in savedTaskIds)
        {
            Task task = tasksList.Find(t => t.taskId == taskId);
            if (task != null)
            {
                CreateTaskUI(task);
            }
        }

        Debug.Log("������� ���������.");
    }

    // ����� ��� �������� UI �������
    void CreateTaskUI(Task task)
    {
        GameObject taskObject = Instantiate(taskPrefab, contentParent);

        // ������������� ����� � ��������
        taskObject.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = task.taskName;
        taskObject.transform.Find("icon").GetComponent<Image>().sprite = task.taskImage;

        // ����������� ������
        Button taskButton = taskObject.transform.Find("Button").GetComponent<Button>();

        if (IsTaskCompletedWithinLastDay(task.taskId))
        {
            taskButton.interactable = false;
            taskButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "���������";
        }
        else
        {
            taskButton.onClick.AddListener(() => OnTaskComplete(task, taskButton));
        }
    }

    // ����� ��� ������������� ������
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

    // ����� ��� ���������� �������
    void OnTaskComplete(Task completedTask, Button taskButton)
    {
        Debug.Log("������� ���������: " + completedTask.taskName);

        // ��������� ������� ����� ���������� �������
        PlayerPrefs.SetString(completedTask.taskId, DateTime.Now.ToString());
        PlayerPrefs.Save();

        // ��������� ������
        taskButton.interactable = false;
        taskButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "���������";

        // ��������� ���� � �����
        characterClicker.AddScore(10); // ����������� ���� �� 1 (��� ������ ���������� �� ������ ������)
    }

    // ���������, ��������� �� ������� �� ��������� 24 ����
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
