using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI locationText;
    public TextMeshProUGUI levelTitleText;

    public float fadeDuration = 1f;
    public float displayDuration = 2f;

    void Start()
    {
        StartCoroutine(LevelIntro());
    }

    IEnumerator LevelIntro()
    {
        // start invisible
        SetAlpha(locationText, 0f);
        SetAlpha(levelTitleText, 0f);

        // locationText
        yield return StartCoroutine(FadeText(locationText, 0f, 1f));
        yield return new WaitForSeconds(displayDuration);
        yield return StartCoroutine(FadeText(locationText, 1f, 0f));

        // levelTitleText
        yield return StartCoroutine(FadeText(levelTitleText, 0f, 1f));
        yield return new WaitForSeconds(displayDuration);
        yield return StartCoroutine(FadeText(levelTitleText, 1f, 0f));
    }

    void SetAlpha(TextMeshProUGUI text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }

    IEnumerator FadeText(TextMeshProUGUI text, float startAlpha, float endAlpha)
    {
        float time = 0f;
        Color color = text.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            text.color = color;

            yield return null;
        }

        color.a = endAlpha;
        text.color = color;
    }
}
