using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundScaler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ScaleToScreen();
    }

    [ContextMenu("Scale Background Manually")]
    private void ScaleToScreen()
    {
        Camera mainCamera = Camera.main;

        float height = 2f * mainCamera.orthographicSize;
        float width = height * mainCamera.aspect;

        Vector2 spriteSize = new Vector2(width, height);
        sr.size = spriteSize;
    }
}
