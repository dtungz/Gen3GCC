using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopUp : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    public void PopUp()
    {
        StartCoroutine(Pop());
    }
    public void OnEnable()
    {
        PopUp();
    }
    public void OnDestroy()
    {
        StopAllCoroutines();
    }
    private IEnumerator Pop()
    {
        Vector2 startScale = Vector2.one;
        Vector2 bufferScale = Vector2.one*2;
        float time = 0,duration=0.25f;
        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, bufferScale, curve.Evaluate(time / (duration)));
            yield return null;
        }
        transform.localScale = startScale;
        yield return null;
    }
    public IEnumerator Float(float duration, Vector3 startScale, Vector3 targetScale, AnimationCurve curve)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, curve.Evaluate(time / (duration)));
            yield return null;
        }
        transform.localScale = targetScale;
    }
}
