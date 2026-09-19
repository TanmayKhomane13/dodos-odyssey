using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Intro
    public TextMeshProUGUI locationText;
    public TextMeshProUGUI levelTitleText;

    public float fadeDuration = 1f;
    public float displayDuration = 2f;

    // Pause UI
    public GameObject pausePanel;
    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(LevelIntro());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // --------- Main Menu -------------
    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level 1");
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level 1");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
    // ---------------------------------

    // --------- Pause System ----------
    void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // ---------------------------------

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
