using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MenuState
{
    MAIN,
    SETTINGS,
    SHOP,
}

public class Menu : MonoBehaviour, IRequireCleanup
{
    private MenuState menuState = MenuState.MAIN;

    public MenuState MenuState
    {
        get
        {
            return menuState;
        }
        set
        {
            if (menuState == value) return;
            if (value == MenuState.SETTINGS || menuState == MenuState.SETTINGS) fade = true;
            menuState = value;
            OnMenuStateChanged?.Invoke();
        }
    }

    [SerializeField] private AudioClip pauseSound;
    [SerializeField] private AudioClip unpauseSound;

    [SerializeField] private float fadeTime = 1f;

    [SerializeField] private CanvasGroup primaryMenu;
    [SerializeField] private CanvasGroup settingsMenu;

    public event Action OnMenuStateChanged;
    [HideInInspector] public bool fade = false;

    private void Awake()
    {
        OnMenuStateChanged += SwapMenu;
        GameManager.Instance.OnGameStateChanged += Pause;
        GameManager.Instance.OnApplicationCleanup += OnCleanup;
        SceneTransitionManager.Instance.BeforeSceneChange += OnCleanup;
    }

    #region Cleanup
    public void OnDisable()
    {
        if (!GameManager.cleanedUp) OnCleanup();
    }

    public void OnCleanup()
    {
        Debug.Log($"{name}: Unsubscribing in progress...");
        GameManager.Instance.OnGameStateChanged -= Pause;
        GameManager.Instance.OnApplicationCleanup -= OnCleanup;
        SceneTransitionManager.Instance.BeforeSceneChange -= OnCleanup;
    }
    #endregion

    private void SwapMenu()
    {
        if (!fade) return; 
        if (MenuState == MenuState.MAIN)
        {
            StartCoroutine(FadeBetween(settingsMenu, primaryMenu));
        }
        else if (MenuState == MenuState.SETTINGS)
        {
            StartCoroutine(FadeBetween(primaryMenu, settingsMenu));
        }
    }

    private IEnumerator FadeBetween(CanvasGroup previous, CanvasGroup next)
    {
        previous.interactable = false;
        float timer = 0f;
        previous.alpha = 1f;
        while (timer < fadeTime)
        {
            float alpha = 1f - (timer / fadeTime);
            previous.alpha = alpha;
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        previous.alpha = 0;
        previous.gameObject.SetActive(false);
        next.gameObject.SetActive(true);
        next.interactable = false;
        next.alpha = 0f;

        timer = 0f;

        while (timer < fadeTime)
        {
            float alpha = timer / fadeTime;
            next.alpha = alpha;
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        next.alpha = 1f;
        next.interactable = true;
        fade = false;
    }

    private void Pause()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1) return;

        if (GameManager.Instance.GameState == GameState.PAUSE)
        {
            if (pauseSound) AudioManager.Instance.Play2DSFX(pauseSound);
            primaryMenu.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            if (unpauseSound && GameManager.Instance.GameState == GameState.PLAY) AudioManager.Instance.Play2DSFX(unpauseSound);
            primaryMenu.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
