using System.Collections;
using TMPro;
using UnityEngine;

public class SpeechController : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    private Coroutine revealCoroutine;
    private Coroutine clearCoroutine;
    public bool isRevealing = false;
    private bool isClearing = false;
    string currentText;

    public void RevealText(string text)
    {
        if (!isRevealing && !isClearing)
        {
            revealCoroutine = StartCoroutine(RevealRoutine(text));
        }
    }

    public void SkipText()
    {
        isRevealing = false;
        isClearing = false;
        textUI.text = currentText;

        if (revealCoroutine != null)
        {
            StopCoroutine(revealCoroutine);
        }
        if (clearCoroutine != null)
        {
            StopCoroutine(clearCoroutine);
        }

        ClearText(1f);
    }

    private IEnumerator RevealRoutine(string text)
    {
        isRevealing = true;
        textUI.text = "";
        currentText = text;

        float interval = 1.0f / text.Length;
        for (int i = 0; i < text.Length; i++)
        {
            textUI.text += text[i];
            yield return new WaitForSeconds(interval);
        }

        isRevealing = false;
        clearCoroutine = StartCoroutine(ClearText(1f));
    }

    private IEnumerator ClearText(float seconds)
    {
        isClearing = true;
        yield return new WaitForSeconds(seconds);
        textUI.text = "";
        isClearing = false;
    }
}
