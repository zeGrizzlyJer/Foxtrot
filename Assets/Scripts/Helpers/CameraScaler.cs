using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    [SerializeField] private float sceneWidth = 20f;

    private void Awake()
    {
        ScaleSize();
    }

    [ContextMenu("Scale Screen Size")]
    private void ScaleSize()
    {
        float unitsPerPixel = sceneWidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
        GetComponent<Camera>().orthographicSize = Mathf.Ceil(desiredHalfHeight);
        Debug.Log($"Camera size was changed to: {desiredHalfHeight}");
    }
}
