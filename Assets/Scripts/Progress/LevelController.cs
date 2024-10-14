using UnityEngine;
using UnityEngine.SceneManagement; // ��� �������� ����
using EasyUI.Popup; // ���������� ���������� ��� ������ � Popup

public class LevelController : MonoBehaviour
{
    public int[] levelThresholds; // ������ ��������� �������� ��� �������� �� ����� �������
    private int currentLevel = 0; // ������� �������

    // ����� ��� �������� ������
    public void CheckLevelUp(int score)
    {
        // ���������, ������ �� ����� ������ ��� �������� �� ��������� �������
        if (currentLevel < levelThresholds.Length && score >= levelThresholds[currentLevel])
        {
            LevelUp(); // ������� �� ����� �������
        }
    }

    private void LevelUp()
    {
        Popup.Show("Success", "Your account updated successfully.", "OK", PopupColor.Green, OnPopupClose);
    }

    private void OnPopupClose()
    {
        currentLevel++; // ����������� �������

        // ���������, �������� �� �� ���������� ������
        if (currentLevel < levelThresholds.Length)
        {
            Debug.Log($"������� �� ������� {currentLevel + 1}");
            SceneManager.LoadScene(currentLevel); // ��������� ����� �����
        }
        else
        {
            Debug.Log("��� ������ ��������!");
            // ����� ����� �������� �������������� ������, ���� ��� ������ ��������
        }
    }
}
