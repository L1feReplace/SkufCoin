using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EasyUI.Popup;

public class EnergyManager : MonoBehaviour
{
    public Image[] energyIcons; // ������ ������ �������
    public Sprite fullEnergyIcon; // ������ ��� ������ �������
    public Sprite emptyEnergyIcon; // ������ ��� ������ �������
    public int clicksPerEnergy = 10; // ���������� ������ ��� ������� ����� �������
    public float energyRestoreTime = 5f; // ����� ��� �������������� ����� �������

    private int currentEnergy; // ������� ������� �������
    private int currentClicks = 0; // ������� ���������� ������
    private Coroutine restoreCoroutine; // ��� ���������� ��������� �������������� �������

    void Start()
    {
        currentEnergy = energyIcons.Length; // ��������� ������� ����� ���������� ������
        for (int i = 0; i < currentEnergy; i++)
        {
            energyIcons[i].sprite = fullEnergyIcon; // ������������� ������ ������ ��� ������
        }
    }

    public void OnObjectClick()
    {
        if (currentEnergy > 0)
        {
            currentClicks++; // ����������� ���������� ������

            if (currentClicks >= clicksPerEnergy)
            {
                UseEnergy(); // ���������� ���� ������� �������
                currentClicks = 0; // ���������� ������� ������
            }
        }
    }

    void UseEnergy()
    {
        currentEnergy--; // ��������� ���������� �������
        energyIcons[currentEnergy].sprite = emptyEnergyIcon; // �������� ������ �� ������

        // ��������� �������� �������������� �������, ���� ��� ���������
        if (currentEnergy == 0)
        {
            Debug.Log("Energy depleted!"); // ������� ���������, ����� ������� �����������
            Popup.Show("������� �����������, ������������� ����� ��������� �����");
            if (restoreCoroutine == null) // ���������, �������� �� ��������
            {
                restoreCoroutine = StartCoroutine(RestoreEnergy()); // ��������� �������������� �������
            }
        }
        else if (restoreCoroutine == null) // ���������, ����� �� ��������� �������������� �������
        {
            restoreCoroutine = StartCoroutine(RestoreEnergy()); // ��������� �������������� �������
        }
    }

    // ����� ��� ��������, ���� �� ��� �������
    public bool HasEnergy()
    {
        return currentEnergy > 0;
    }

    void Update()
    {
        // ��������������� �������, ���� ������� ������� ������ ��������� � �������� �� ��������
        if (currentEnergy < energyIcons.Length && restoreCoroutine == null)
        {
            restoreCoroutine = StartCoroutine(RestoreEnergy());
        }
    }

    IEnumerator RestoreEnergy()
    {
        while (currentEnergy < energyIcons.Length)
        {
            yield return new WaitForSeconds(energyRestoreTime); // ���� ������������ �����

            // ��������������� ������, ������ ���� ���� �����������
            energyIcons[currentEnergy].sprite = fullEnergyIcon; // ��������������� ������
            currentEnergy++; // ����������� ���������� �������
        }

        restoreCoroutine = null; // ����������� ��������, ����� �������������� ���������
    }
}
