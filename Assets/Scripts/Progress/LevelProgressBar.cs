using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ��� ���������� �������
using EasyUI.Popup; // ��� ������������� �������

public class LevelProgressBar : MonoBehaviour
{
    public TextMeshProUGUI levelText; // ������ �� TextMeshPro ��� ����������� ������
    public Slider progressBar; // ������ �� ��������-���

    private int currentLevel = 1; // ������� �������
    private int currentClicks = 0; // ���������� ������� ������
    public int clicksForNextLevel = 10; // ���������� ������ ��� �������� �� ��������� �������
    public int levelToLoadNextScene = 5; // ������� ��� �������� �� ��������� �����

    // �����, ���������� ��� ����� �� ������
    public void OnObjectClick()
    {
        currentClicks++; // ����������� ���������� ������
        UpdateProgressBar(); // ��������� ��������-���

        if (currentClicks >= clicksForNextLevel)
        {
            LevelUp(); // ��������� �� ��������� �������
        }
    }

    // ����� ��� ���������� ��������-����
    void UpdateProgressBar()
    {
        float progress = (float)currentClicks / clicksForNextLevel;
        progressBar.value = progress; // ��������� �������� ��������
    }

    // ����� ��� �������� �� ��������� �������
    void LevelUp()
    {
        currentLevel++; // ����������� �������
        levelText.text = "LEVEL: " + currentLevel; // ��������� ����� ������
        currentClicks = 0; // ���������� ���������� ������
        clicksForNextLevel += 10; // ����������� ���������� ������ ��� ���������� ������

        UpdateProgressBar(); // ���������� ��������-���

        // ���������, ��������� �� ����������� ������� ��� �������� �� ��������� �����
        if (currentLevel >= levelToLoadNextScene)
        {
            ShowLevelUpPopup(); // ���������� �����
        }
    }
    // ����� ��� ������ ������ � �������� ��������� �����
    void ShowLevelUpPopup()
    {
        Popup.Show("�����", "�� �������� ������ " + levelToLoadNextScene + ".", "OK", PopupColor.Green, OnPopupClose);
    }


    // �����, ���������� ��� �������� ������
    private void OnPopupClose()
    {
        LoadNextScene(); // �������� ��������� �����
    }

    // ����� ��� �������� ��������� �����
    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // ��������� ��������� ����� �� �������
    }
}
