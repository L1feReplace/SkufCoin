using UnityEngine;

public class UIController : MonoBehaviour
{
    // ������ �� ������
    public GameObject _dailiesPanel;

    // ����� ��� ������������ ��������� ������
    public void TogglePanel()
    {
        if (_dailiesPanel != null)
        {
            // ����������� ��������� ������
            _dailiesPanel.SetActive(!_dailiesPanel.activeSelf);
        }
    }
}
