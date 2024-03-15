using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShopState
{
    None,
    PLAYER,
    TERRAIN,
    ENEMIES,
}
public class ShopManager : MonoBehaviour
{

    private ShopState state = ShopState.None;
    public ShopState State
    { 
        get { return state; } 
        set
        {
            if (value == state) return;
            state = value;
            SpriteIndex = 0;
            switch (state)
            {
                case ShopState.PLAYER:
                    targetSprite.sprite = DataManager.Instance.characterUnlockData[spriteIndex].shopIcon;
                    break;
                case ShopState.TERRAIN:
                    targetSprite.sprite = DataManager.Instance.terrainUnlockData[spriteIndex].shopIcon;
                    break;
                case ShopState.ENEMIES:
                    targetSprite.sprite = DataManager.Instance.enemyUnlockData[spriteIndex].shopIcon;
                    break;
                default:
                    break;
            }
            DetermineMaxIndex();
            OnShopStateChange?.Invoke();
        }
    }

    private int spriteIndex = 0;
    public int SpriteIndex
    {
        get { return spriteIndex; }
        set
        {
            if (value == spriteIndex) return;
            if (value < 0)
            {
                spriteIndex = maxIndex;
            }
            else if (value > maxIndex)
            {
                spriteIndex = 0;
            }
            else
            {
                spriteIndex = value;
            }
            SetGameDataIndex();
            ChangeIconFromIndex();
            DetermineLockState();
            OnSpriteIndexChange?.Invoke();
        }
    }

    public event Action OnSpriteIndexChange;
    public event Action OnShopStateChange;

    int maxIndex = 0;

    public RectTransform target;
    public Menu menu;

    [Header("Menu Transition Properties")]
    public float transitionSpeed = 1f;
    public Vector3 menuPosition = new Vector3(0, 320, 0);
    public Vector3 shopPosition = new Vector3(1920, 320, 0);
    private Vector3 position;
    private Vector3 targetPosition;

    public Image targetSprite;
    public Image lockImage;

    private void Start()
    {
        if (!target)
        {
            Debug.Log($"{name} is missing target reference - please assign.");
        }
        if (!menu)
        {
            Debug.Log($"{name} is missing menu reference - please assign.");
        }
        if (!targetSprite)
        {
            Debug.Log($"{name} is missing target sprite reference - please assign.");
        }
        menu.OnMenuStateChanged += TransitionMenu;

        state = ShopState.PLAYER;
        DetermineMaxIndex();
    }

    private void ChangeIconFromIndex()
    {
        switch (state)
        {
            case ShopState.PLAYER:
                targetSprite.sprite = DataManager.Instance.characterUnlockData[spriteIndex].shopIcon;
                break;
            case ShopState.TERRAIN:
                targetSprite.sprite = DataManager.Instance.terrainUnlockData[spriteIndex].shopIcon;
                break;
            case ShopState.ENEMIES:
                targetSprite.sprite = DataManager.Instance.enemyUnlockData[spriteIndex].shopIcon;
                break;
            default:
                break;
        }
    }

    private void DetermineMaxIndex()
    {
        switch (state)
        {
            case ShopState.PLAYER:
                maxIndex = DataManager.Instance.characterUnlockData.Count - 1;
                break;
            case ShopState.TERRAIN:
                maxIndex = DataManager.Instance.terrainUnlockData.Count - 1;
                break;
            case ShopState.ENEMIES:
                maxIndex = DataManager.Instance.enemyUnlockData.Count - 1;
                break;
            default:
                break;
        }
    }

    private void SetGameDataIndex()
    {
        switch (state)
        {
            case ShopState.PLAYER:
                GameData.PlayerIndex = spriteIndex;
                break;
            case ShopState.TERRAIN:
                GameData.TerrainIndex = spriteIndex;
                break;
            case ShopState.ENEMIES:
                GameData.EnemyIndex = spriteIndex;
                break;
            default:
                break;
        }
    }

    public void DetermineLockState()
    {
        switch (state)
        {
            case ShopState.PLAYER:
                lockImage.gameObject.SetActive(!DataManager.Instance.characterUnlockData[spriteIndex].isUnlocked);
                break;
            case ShopState.TERRAIN:
                lockImage.gameObject.SetActive(!DataManager.Instance.terrainUnlockData[spriteIndex].isUnlocked);
                break;
            case ShopState.ENEMIES:
                lockImage.gameObject.SetActive(!DataManager.Instance.enemyUnlockData[spriteIndex].isUnlocked);
                break;
            default:
                break;
        }
    }

    #region Menu Transition
    public void TransitionMenu()
    {
        if (menu.fade) return;
        if (menu.MenuState == MenuState.MAIN)
        {
            position = shopPosition;
            targetPosition = menuPosition;
        }
        if (menu.MenuState == MenuState.SHOP)
        {
            position = menuPosition;
            targetPosition = shopPosition;
        }
        StartCoroutine(MoveMenu());
    }

    IEnumerator MoveMenu()
    {
        Debug.Log($"Transition Menu to {menu.MenuState}");
        float timePassed = 0f;
        CanvasGroup tCanvas = target.GetComponent<CanvasGroup>();
        tCanvas.interactable = false;

        while (timePassed < transitionSpeed)
        {
            float t = timePassed / transitionSpeed;
            target.localPosition = Vector3.Lerp(position, targetPosition, t);

            timePassed += Time.deltaTime;

            yield return null;
        }

        target.localPosition = targetPosition;
        tCanvas.interactable = true;
    }
    #endregion
}
