using UnityEngine;
using TMPro;

public class CharacterClicker : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // ���������� TextMeshPro ��� UI ������
    private int score = 0;

    public LevelProgressBar levelProgressBar;
    public EnergyManager energyManager; // ������ �� EnergyManager

    public void OnImageClick() // ���� ����� ����� ��������� ����� Inspector
    {
        // ���������, ���� �� ������� ����� ����������� ��������
        if (energyManager.HasEnergy())
        {
            AddScore(1); // ��������� 1 � ����� ��� �����
            levelProgressBar.OnObjectClick();
            energyManager.OnObjectClick(); // ��������� ������� ��� �����
        }
        else
        {
            Debug.Log("������� �����������!"); // ������� ���������, ���� ������� �����������
        }
    }

    public void AddScore(int amount) // ����� ������ ��������� ���������� �����
    {
        score += amount; // ����������� ���� �� �������� ����������
        scoreText.text = "���������� ���� \r\n������: " + score.ToString(); // ��������� �����
    }
}
