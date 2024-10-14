using UnityEngine;
using System.Collections;

public class ScaleAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform targetTransform;
    [SerializeField] private Vector3 scaleIncrease = new Vector3(1.1f, 1.1f, 1f);
    [SerializeField] private Vector3 scaleDecrease = new Vector3(0.9f, 0.9f, 1f);
    [SerializeField] private float duration = 1f;

    private Coroutine scaleCoroutine;

    private void Start()
    {
        if (targetTransform != null)
        {
            StartScaling();
        }
    }

    private void StartScaling()
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }

        scaleCoroutine = StartCoroutine(AnimateScale());
    }


    private IEnumerator AnimateScale()
    {
        Vector3 originalScale = targetTransform.localScale;

        while (true)
        {
            yield return ScaleTo(Vector3.Scale(originalScale, scaleIncrease));
            yield return ScaleTo(Vector3.Scale(originalScale, scaleDecrease));
            yield return ScaleTo(originalScale);
        }
    }

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 initialScale = targetTransform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            targetTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            yield return null;
        }

        targetTransform.localScale = targetScale;
    }
}
