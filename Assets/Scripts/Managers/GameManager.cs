using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    PLAY,
    PAUSE,
    END,
    MAINMENU,
    GAMESTART,
    RESPAWN,
}

public class GameManager : Singleton<GameManager>
{
    #region Statics
    public static float START_SPEED = 5f;
    public static float GAME_ACCEL = 0.1f;
    public static int SCORE_RATE = 2;
    #endregion

    #region Properties
    private GameState gameState = GameState.MAINMENU;
    public GameState GameState
    {
        get
        {
            return gameState;
        }
        set
        {
            if (gameState == value) return;
            gameState = value;
            Debug.Log("Gamestate: " + gameState);
            OnGameStateChanged?.Invoke();
        }
    }

    private int lives = 3;
    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {
            if (lives == value) return;
            lives = value;
            OnLivesChanged?.Invoke();
        }
    }

    private int hiScore;
    public int HiScore
    {
        get
        {
            return hiScore;
        }
        set
        {
            if (hiScore >= value) return;
            hiScore = value;
            PlayerPrefs.SetInt("Hiscore", hiScore);
            OnHiScoreChanged?.Invoke();
        }
    }
    private int score = 0;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            if (score == value) return;
            score = value;
            OnScoreChanged?.Invoke();
        }
    }

    public float gameSpeed = START_SPEED;
    #endregion

    #region Cleanup
    static public bool cleanedUp = false;
    public event Action OnApplicationCleanup;

    private void OnApplicationQuit()
    {
        OnApplicationCleanup?.Invoke();
        cleanedUp = true;
    }
    #endregion

    public event Action OnGameStateChanged;
    public event Action OnLivesChanged;
    public event Action OnScoreChanged;
    public event Action OnHiScoreChanged;

    protected override void Awake()
    {
        base.Awake();
        hiScore = PlayerPrefs.GetInt("Hiscore", 0);
        OnGameStateChanged += DetermineCursorState;
        OnGameStateChanged += SetTimeScale;
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.PLAY:
                gameSpeed += GAME_ACCEL * Time.deltaTime;
                Score += SCORE_RATE;
                break;
            default:
                break;
        }
    }

    #region GameStateChangeEffects
    private void SetTimeScale()
    {
        if (GameState != GameState.PAUSE) Time.timeScale = 1f;
    }

    private void DetermineCursorState()
    {
        if (gameState == GameState.PLAY)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    #endregion
}