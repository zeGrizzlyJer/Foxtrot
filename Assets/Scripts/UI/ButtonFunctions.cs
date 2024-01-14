using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    private bool hasGameStarted = false;

    #region UI Elements
    [SerializeField] Menu menu;
    [SerializeField] TextMeshProUGUI scoreTxt;
    [SerializeField] TextMeshProUGUI hiScoreTxt;
    [SerializeField] Button startBtn;
    [SerializeField] Button settingsBtn;
    [SerializeField] Button backBtn;
    [SerializeField] Button quitBtn;
    [SerializeField] Button resumeBtn;
    [SerializeField] Button menuBtn;
    [SerializeField] Button pauseBtn;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    #endregion
    #region SFX
    [SerializeField] AudioClip startGameSound;
    [SerializeField] AudioClip clickSound;
    [SerializeField] AudioClip hoverSound;
    [SerializeField] AudioClip returnSound;
    [SerializeField] AudioClip resumeSound;
    #endregion

    private void Start()
    {
        List<Button> buttons = new List<Button>();

        if (scoreTxt)
        {
            scoreTxt.text = "SCORE: " + GameManager.Instance.Score;
        }
        if (hiScoreTxt)
        {
            hiScoreTxt.text = "HIGH SCORE: " + GameManager.Instance.HiScore;
        }
        if (startBtn)
        {
            startBtn.onClick.AddListener(StartGame);
            buttons.Add(startBtn);
        }
        if (settingsBtn)
        {
            settingsBtn.onClick.AddListener(OpenSettings);
            buttons.Add(settingsBtn);
        }
        if (backBtn)
        {
            backBtn.onClick.AddListener(OpenMenu);
            buttons.Add(backBtn);
        }
        if (quitBtn)
        {
            quitBtn.onClick.AddListener(QuitGame);
            buttons.Add(quitBtn);
        }
        if (resumeBtn)
        {
            resumeBtn.onClick.AddListener(ResumeGame);
            buttons.Add(resumeBtn);
        }
        if (menuBtn)
        {
            menuBtn.onClick.AddListener(GoToMenu);
            buttons.Add(menuBtn);
        }
        if (pauseBtn)
        {
            pauseBtn.onClick.AddListener(PauseGame);
            buttons.Add(pauseBtn);
        }
        if (masterSlider)
        {
            masterSlider.onValueChanged.AddListener(MasterVolumeCallback);
            masterSlider.value = PlayerPrefs.GetFloat("MasterVol", 0.75f);
            SFXVolumeCallback(masterSlider.value);
        }
        if (musicSlider)
        {
            musicSlider.onValueChanged.AddListener(MusicVolumeCallback);
            musicSlider.value = PlayerPrefs.GetFloat("MusicVol", 0.75f);
            SFXVolumeCallback(musicSlider.value);
        }
        if (sfxSlider)
        {
            sfxSlider.onValueChanged.AddListener(SFXVolumeCallback);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVol", 0.75f);
            SFXVolumeCallback(sfxSlider.value);
        }

        foreach (Button button in buttons)
        {
            EventTrigger trigger = button.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = button.gameObject.AddComponent<EventTrigger>();
            }
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            entry.callback.AddListener((data) => { OnButtonHovered(); });
            trigger.triggers.Add(entry);
        }
    }

    public void OnButtonHovered()
    {
        if (hoverSound) AudioManager.Instance.Play2DSFX(hoverSound);
    }
    
    private void PauseGame()
    {
        if (GameManager.Instance.GameState == GameState.PAUSE)
        {
            ResumeGame();
        }
        else
        {
            hasGameStarted = (GameManager.Instance.GameState == GameState.GAMESTART ? true : false);
            GameManager.Instance.GameState = GameState.PAUSE;
        }
    }

    private void GoToMenu()
    {
        if (clickSound) AudioManager.Instance.Play2DSFX(clickSound);
        SceneTransitionManager.Instance.LoadScene(0);
    }

    private void SFXVolumeCallback(float value)
    {
        AudioManager.Instance.SetMixerVolume("SFXVol", value);
    }

    private void MusicVolumeCallback(float value)
    {
        AudioManager.Instance.SetMixerVolume("MusicVol", value);
    }

    private void MasterVolumeCallback(float value)
    {
        AudioManager.Instance.SetMixerVolume("MasterVol", value);
    }

    private void ResumeGame()
    {
        if (resumeSound) AudioManager.Instance.Play2DSFX(resumeSound);
        GameManager.Instance.GameState = (hasGameStarted ? GameState.GAMESTART : GameState.PLAY);
    }

    private void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    private void OpenMenu()
    {
        if (returnSound) AudioManager.Instance.Play2DSFX(returnSound);
        menu.MenuState = MenuState.MAIN;
    }

    private void OpenSettings()
    {
        if (clickSound) AudioManager.Instance.Play2DSFX(clickSound);
        menu.MenuState = MenuState.SETTINGS;
    }

    private void StartGame()
    {
        if (startGameSound) AudioManager.Instance.Play2DSFX(startGameSound);
        SceneTransitionManager.Instance.LoadScene(1);
    }
}
