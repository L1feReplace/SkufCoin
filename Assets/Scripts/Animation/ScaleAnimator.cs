using UnityEngine;
using System.Collections;

public class ScaleAnimator : MonoBehaviour
{
    public RectTransform targetTransform; // ������ �� RectTransform �����������
    public Vector3 scaleIncrease = new Vector3(1.1f, 1.1f, 1f); // �� ������� ��� ��������� �������
    public Vector3 scaleDecrease = new Vector3(0.9f, 0.9f, 1f); // �� ������� ��� ��������� �������
    public float duration = 1f; // ������������ ������ ����� �������� (���������� � ����������)

    private Coroutine scaleCoroutine; // ������ ������ �� ������� ��������

    private void Start()
    {
        if (targetTransform != null)
        {
            StartScaling();
        }
    }

    public void StartScaling()
    {
        // ���� �������� ��� ��������, ������������� �
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }

        scaleCoroutine = StartCoroutine(AnimateScale());
    }

    public void StopScaling()
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
            scaleCoroutine = null; // ����������� ������ ����� ���������
            targetTransform.localScale = Vector3.one; // ���������� � ��������� ��������
        }
    }

    private IEnumerator AnimateScale()
    {
        Vector3 originalScale = targetTransform.localScale;

        while (true)
        {
            // ���������� ��������
            yield return ScaleTo(Vector3.Scale(originalScale, scaleIncrease), duration);

            // ���������� ��������
            yield return ScaleTo(Vector3.Scale(originalScale, scaleDecrease), duration);

            // ����������� � ��������� ��������
            yield return ScaleTo(originalScale, duration);
        }
    }

    private IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = targetTransform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            targetTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null; // ��������� ���������� �������� �� ���� ��������
        }

        targetTransform.localScale = targetScale; // ������������� �������� �������
    }
}
