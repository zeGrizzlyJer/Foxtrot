using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    [Header("Components")]
    [SerializeField] private CanvasGroup fadeImg;
    [SerializeField] private float fadeTime = 0.3f;

    private bool inTransition = false;

    public event Action BeforeSceneChange;

    private void Start()
    {
        StartCoroutine(FadeOut());
        DetermineGameState();
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        GameManager.Instance.HiScore = GameManager.Instance.Score;
        StartCoroutine(FadeOut());
        DetermineGameState();
    }

    public void LoadScene(int scene)
    {
        if (inTransition) return;

        inTransition = true;
        StartCoroutine(FadeIn(scene));
    }

    public void RestartScene()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        LoadScene(scene);
    }

    public void DetermineGameState()
    {
        GameManager.Instance.DetermineGameState();
    }

    private IEnumerator FadeIn(int scene)
    {
        float timer = 0f;
        while (timer < fadeTime)
        {
            fadeImg.alpha = timer / fadeTime;
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        fadeImg.alpha = 1f;

        BeforeSceneChange?.Invoke();
        SceneManager.LoadScene(scene);
    }

    private IEnumerator FadeOut()
    {
        inTransition = true;

        float timer = 0f;

        fadeImg.alpha = 1f;
        while (timer < fadeTime)
        {
            fadeImg.alpha = 1 - (timer / fadeTime);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        fadeImg.alpha = 0f;

        inTransition = false;

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            GameManager.Instance.Score = 0;
            GameManager.Instance.Lives = GameManager.START_LIVES;
            GameManager.Instance.gameSpeed = GameManager.START_SPEED;
            GameManager.Instance.timer = 0;
            DetermineGameState();
        }
    }
}
