using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour, IRequireCleanup
{
    public RectTransform livesIcon;
    public Button gameStartPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiScoreText;
    public TextMeshProUGUI moneyText;
    public float livesIconSize = 112f;

    private void Awake()
    {
        GameManager.Instance.OnApplicationCleanup += OnCleanup;
        SceneTransitionManager.Instance.BeforeSceneChange += OnCleanup;
        if (livesIcon)
        {
            livesIconSize = livesIcon.rect.height;
            GameManager.Instance.OnLivesChanged += AdjustLives;
            AdjustLives();
        }
        if (gameStartPanel)
        {
            gameStartPanel.onClick.AddListener(StartGame);
            GameManager.Instance.OnGameStateChanged += EnablePanel;
        }
        if (scoreText)
        {
            scoreText.text = "0";
            GameManager.Instance.OnScoreChanged += UpdateScoreText;
        }
        if (hiScoreText)
        {
            hiScoreText.text = "HIGH SCORE: " + GameManager.Instance.HiScore;
            GameManager.Instance.OnHiScoreChanged += UpdateHiScoreText;
        }
        if (moneyText)
        {
            moneyText.text = GameData.Money.ToString(); ;
            GameManager.Instance.OnCurrencyChanged += UpdateMoneyText;
        }
    }

    #region Cleanup
    public void OnDisable()
    {
        if (!GameManager.cleanedUp) OnCleanup();
    }

    public void OnCleanup()
    {
        Debug.Log($"{name}: Unsubscribing in progress...");
        GameManager.Instance.OnApplicationCleanup -= OnCleanup;
        SceneTransitionManager.Instance.BeforeSceneChange -= OnCleanup;
        if (livesIcon) GameManager.Instance.OnLivesChanged -= AdjustLives;
        if (gameStartPanel) GameManager.Instance.OnGameStateChanged -= EnablePanel;
        if (scoreText) GameManager.Instance.OnScoreChanged -= UpdateScoreText;
        if (hiScoreText) GameManager.Instance.OnHiScoreChanged -= UpdateHiScoreText;
        if (moneyText) GameManager.Instance.OnCurrencyChanged -= UpdateMoneyText;
    }
    #endregion

    private void AdjustLives()
    { 
        livesIcon.sizeDelta = new Vector2(livesIconSize * GameManager.Instance.Lives, livesIconSize);
    }

    private void StartGame()
    {
        gameStartPanel.gameObject.SetActive(false);
        GameManager.Instance.GameState = GameState.PLAY;
    }

    private void EnablePanel()
    {
        if (GameManager.Instance.GameState == GameState.GAMESTART)
        {
            gameStartPanel.gameObject.SetActive(true);
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = GameManager.Instance.Score.ToString();
    }
    private void UpdateHiScoreText()
    {
        hiScoreText.text = "HIGH SCORE: " + GameManager.Instance.HiScore;
    }

    private void UpdateMoneyText()
    {
        moneyText.text = GameData.Money.ToString();
    }
}
