using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopOut : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;

    [SerializeField] private GameObject target;
   
    public void OnEnable()
    {
        image.color = Color.white;
        text.color = Color.black;
    }
    public void OnDisable()
    {
        StopAllCoroutines();
    }
    public void OnDestroy()
    {
        StopAllCoroutines();
    }
    public void OnOut()
    {
        StartCoroutine(Blur());
        StartCoroutine(PopOut());
    }
    private IEnumerator Blur()
    {
        float start = 1f;
        float end = 0f;
        float time = 0, duration = 0.25f;
        while (time < duration)
        {
            time += Time.deltaTime;
            image.color = new Color(1,1,1, Mathf.Lerp(start, end, curve.Evaluate(time / (duration))));
            text.color = new Color(0, 0, 0, Mathf.Lerp(start, end, curve.Evaluate(time / (duration))));
            yield return null;
        }
        target.SetActive(false);
    }
    private IEnumerator PopOut()
    {
        Vector2 startScale = Vector2.one;
        Vector2 bufferScale = Vector2.one * 0.5f;
        float time = 0, duration = 0.25f;
        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, bufferScale, curve.Evaluate(time / (duration)));
            yield return null;
        }
        transform.localScale = startScale;
        yield return null;
    }
}
