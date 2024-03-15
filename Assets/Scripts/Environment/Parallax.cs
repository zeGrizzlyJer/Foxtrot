using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Parallax : MonoBehaviour
{
    public enum Mode
    {
        TIME,
        SPEED,
    }

    public bool isSprite = false;
    private Material parallaxMat;

    public Mode mode = Mode.TIME;

    private float time = 0f;
    public float horizontalSpeed = 1f;
    public float verticalSpeed = 1f;
    public bool goingRight = true;
    public bool goingUp = true;

    private void Start()
    {
        if (GetComponent<SpriteRenderer>() != null) isSprite = true;
        if (isSprite) parallaxMat = GetComponent<SpriteRenderer>().material;
        if (!isSprite) parallaxMat = GetComponent<Image>().material;
    }

    private void FixedUpdate()
    {
        switch(mode)
        {
            case Mode.TIME:
                TimeMethod();
                break;
            default:
                break;
        }
    }

    private void TimeMethod()
    {
        float offsetX = (goingRight ? 1f : -1f) * time * horizontalSpeed;
        float offsetY = (goingUp ? 1f : -1f) * time * verticalSpeed;
        time += Time.deltaTime;

        parallaxMat.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}
