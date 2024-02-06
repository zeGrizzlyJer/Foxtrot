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
            spriteIndex = 0;
            switch (state)
            {
                case ShopState.PLAYER:
                    targetSprite.sprite = playerSprites[spriteIndex];
                    break;
                case ShopState.TERRAIN:
                    targetSprite.sprite = terrainSprites[spriteIndex];
                    break;
                case ShopState.ENEMIES:
                    targetSprite.sprite = enemySprites[spriteIndex];
                    break;
                default:
                    break;
            }
            DetermineMaxIndex();
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
            ChangeIconFromIndex();
        }
    }

    int maxIndex = 0;

    public RectTransform target;
    public Menu menu;

    [Header("Menu Transition Properties")]
    public float transitionSpeed = 1f;
    public Vector3 menuPosition = new Vector3(0, 320, 0);
    public Vector3 shopPosition = new Vector3(1920, 320, 0);
    private Vector3 position;
    private Vector3 targetPosition;

    [Header("Sprite Lists")]
    public Image targetSprite;
    public List<Sprite> playerSprites = new List<Sprite>();
    public List<Sprite> terrainSprites = new List<Sprite>();
    public List<Sprite> enemySprites = new List<Sprite>();


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
                targetSprite.sprite = playerSprites[spriteIndex];
                break;
            case ShopState.TERRAIN:
                targetSprite.sprite = terrainSprites[spriteIndex];
                break;
            case ShopState.ENEMIES:
                targetSprite.sprite = enemySprites[spriteIndex];
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
                maxIndex = playerSprites.Count - 1;
                break;
            case ShopState.TERRAIN:
                maxIndex = terrainSprites.Count - 1;
                break;
            case ShopState.ENEMIES:
                maxIndex = enemySprites.Count - 1;
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
