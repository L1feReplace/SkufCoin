using UnityEngine;
using TMPro;

public class CharacterClicker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private LevelProgressBar levelProgressBar;
    [SerializeField] private EnergyManager energyManager;

    private int score = 0;

    public void OnImageClick()
    {
        if (energyManager.HasEnergy())
        {
            AddScore(1);
            levelProgressBar.OnObjectClick();
            energyManager.OnObjectClick();
        }
        else
        {
            Debug.Log("������� �����������!");
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "���������� ���� \r\n������: " + score.ToString();
    }

}
