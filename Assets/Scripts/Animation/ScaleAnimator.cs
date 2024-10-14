using UnityEngine;
using System.Collections;

public class ScaleAnimator : MonoBehaviour
{
    public RectTransform targetTransform; // Ссылка на RectTransform изображения
    public Vector3 scaleIncrease = new Vector3(1.1f, 1.1f, 1f); // Во сколько раз увеличить масштаб
    public Vector3 scaleDecrease = new Vector3(0.9f, 0.9f, 1f); // Во сколько раз уменьшить масштаб
    public float duration = 1f; // Длительность одного цикла анимации (увеличение и уменьшение)

    private Coroutine scaleCoroutine; // Хранит ссылку на текущую корутину

    private void Start()
    {
        if (targetTransform != null)
        {
            StartScaling();
        }
    }

    public void StartScaling()
    {
        // Если анимация уже запущена, останавливаем её
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
            scaleCoroutine = null; // Освобождаем ссылку после остановки
            targetTransform.localScale = Vector3.one; // Возвращаем к исходному масштабу
        }
    }

    private IEnumerator AnimateScale()
    {
        Vector3 originalScale = targetTransform.localScale;

        while (true)
        {
            // Увеличение масштаба
            yield return ScaleTo(Vector3.Scale(originalScale, scaleIncrease), duration);

            // Уменьшение масштаба
            yield return ScaleTo(Vector3.Scale(originalScale, scaleDecrease), duration);

            // Возвращение к исходному масштабу
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
            yield return null; // Уменьшаем количество итераций за счет ожидания
        }

        targetTransform.localScale = targetScale; // Устанавливаем конечный масштаб
    }
}
