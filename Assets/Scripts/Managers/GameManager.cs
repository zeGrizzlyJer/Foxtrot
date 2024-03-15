using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public static float START_SPEED = 8f;
    public static float GAME_ACCEL = 0.025f;
    public static int SCORE_RATE = 1;
    public static int START_LIVES = 1;
    public static int REWARDS_MINIMUM = 2;
    public static int REWARDS_MAXIMUM = 10;
    public static int REWARDS_MULTIPLIER = 3;
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

    private int lives = START_LIVES;
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
            GameData.HiScore = value;
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

    private int money;

    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            if (money == value) return;
            money = value;
            GameData.Money = value;
            OnCurrencyChanged?.Invoke();
        }
    }

    public float gameSpeed = START_SPEED;
    [HideInInspector] public float timer = 0f;
    #endregion

    #region Cleanup
    static public bool cleanedUp = false;
    public event Action OnApplicationCleanup;

    private void OnApplicationQuit()
    {
        OnApplicationCleanup?.Invoke();
        Debug.Log("Cleaning Up");
        cleanedUp = true;
    }
    #endregion

    public event Action OnGameStateChanged;
    public event Action OnLivesChanged;
    public event Action OnScoreChanged;
    public event Action OnHiScoreChanged;
    public event Action OnCurrencyChanged;

    protected override void Awake()
    {
        base.Awake();
        HiScore = GameData.HiScore;
        Money = GameData.Money;
        OnGameStateChanged += DetermineCursorState;
        OnGameStateChanged += SetTimeScale;
    }

    private void Start()
    {
        DetermineGameState();
        SceneTransitionManager.Instance.BeforeSceneChange += IncreaseRewards;
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.PLAY:
                gameSpeed += GAME_ACCEL * Time.deltaTime;
                timer += Time.deltaTime;
                if (timer > 1f)
                {
                    timer -= 1f;
                    Score += SCORE_RATE;
                }
                break;
            default:
                break;
        }
    }

    #region GameStateChangeEffects
    public void DetermineGameState()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                GameState = GameState.MAINMENU;
                break;
            case 1:
                GameState = GameState.GAMESTART;
                break;
            case 2:
                GameState = GameState.END;
                break;
            default:
                break;
        }
    }

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

    private void IncreaseRewards()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && Score >= 10)
        {
            GameData.Rewards += 1;
            Debug.Log("Rewards Count: " + GameData.Rewards);
            return;
        }
        Debug.Log("Rewards Count not increased");
    }
}