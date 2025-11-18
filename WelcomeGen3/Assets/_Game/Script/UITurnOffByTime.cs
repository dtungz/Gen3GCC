using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITurnOffByTime : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;

    [SerializeField] private GameObject target;
   
    public void OnEnable()
    {
        image.color = Color.white;
        text.color = Color.black;
        StartCoroutine(OnBlur());

    }
    public void OnDisable()
    {
        StopAllCoroutines();
    }
    public void OnDestroy()
    {
        StopAllCoroutines();
    }
    private IEnumerator OnBlur()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(Blur());
    }
    private IEnumerator Blur()
    {
        float start = 1f;
        float end = 0f;
        float time = 0, duration = 0.5f;
        while (time < duration)
        {
            time += Time.deltaTime;
            image.color = new Color(1,1,1, Mathf.Lerp(start, end, curve.Evaluate(time / (duration))));
            text.color = new Color(0, 0, 0, Mathf.Lerp(start, end, curve.Evaluate(time / (duration))));
            yield return null;
        }
        target.SetActive(false);
    }
}
