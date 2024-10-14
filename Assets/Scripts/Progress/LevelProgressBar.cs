using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EasyUI.Popup;

public class LevelProgressBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider progressBar;

    private int currentLevel = 1;
    private int currentClicks = 0;
    [SerializeField] private int clicksForNextLevel = 10;
    [SerializeField] private int levelToLoadNextScene = 5;

    public void OnObjectClick()
    {
        currentClicks++;
        UpdateProgressBar();

        if (currentClicks >= clicksForNextLevel)
        {
            LevelUp();
        }
    }

    private void UpdateProgressBar()
    {
        float progress = (float)currentClicks / clicksForNextLevel;
        progressBar.value = progress;
    }

    private void LevelUp()
    {
        currentLevel++;
        levelText.text = "LEVEL: " + currentLevel;
        currentClicks = 0;
        clicksForNextLevel += 10;

        UpdateProgressBar();

        if (currentLevel >= levelToLoadNextScene)
        {
            ShowLevelUpPopup();
        }
    }

    private void ShowLevelUpPopup()
    {
        Popup.Show("Успех", "Вы достигли уровня " + levelToLoadNextScene + ".", "OK", PopupColor.Green, OnPopupClose);
    }

    private void OnPopupClose()
    {
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Загружаем следующую сцену по индексу
    }
}
