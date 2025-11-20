using System.Collections;
using UnityEngine;
using TMPro; // Nếu bạn dùng TextMeshPro

public class UIEnvelopeOpen : MonoBehaviour
{
    [Header("Open Animation")]
    [SerializeField] private AnimationCurve openCurve;
    [SerializeField] private float openDuration = 0.35f;

    [Header("Text Reveal")]
    [SerializeField] private TMP_Text textUI;  // hoặc Text nếu bạn dùng Text thường
    [SerializeField] private float charDelay = 0.03f;

    [TextArea]
    [SerializeField] private string fullText; // Nội dung thư
    [SerializeField] private GameObject clickArea;
    public void SetText(string text)
    {
        fullText = text;
    }
    public void FullText()
    {
        clickArea.SetActive(false);
        StopAllCoroutines();
        transform.localScale = Vector2.one;
        textUI.text = fullText;
    }
    private void OnEnable()
    {
        clickArea.SetActive(true);
        StartCoroutine(EnvelopeSequence());
    }

    private IEnumerator EnvelopeSequence()
    {
        yield return StartCoroutine(EnvelopeOpen());
        yield return StartCoroutine(RevealText());
    }

    private IEnumerator EnvelopeOpen()
    {
        Vector3 startScale = new Vector3(1, 0, 1);
        Vector3 targetScale = Vector3.one;

        float time = 0;
        transform.localScale = startScale;

        while (time < openDuration)
        {
            time += Time.deltaTime;
            float t = openCurve.Evaluate(time / openDuration);

            transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }

        transform.localScale = targetScale;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    private IEnumerator RevealText()
    {
        textUI.text = fullText;
        textUI.ForceMeshUpdate();

        float startFontSize = textUI.fontSize;

        textUI.enableAutoSizing = false;
        textUI.fontSize = startFontSize;
        textUI.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            textUI.text += fullText[i];
            yield return new WaitForSeconds(charDelay);
        }
        clickArea.SetActive(false);
    }
}
