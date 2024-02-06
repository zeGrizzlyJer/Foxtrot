using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerUnlockData))]
public class CustomUnlockDataEditor : Editor
{
    private const float MaxPreviewWidth = 150f;

    private void OnEnable()
    {
        serializedObject.Update();
    }

    public override void OnInspectorGUI()
    {
        PlayerUnlockData playerData = (PlayerUnlockData)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (playerData.shopIcon != null)
        {
            //serializedObject.Update();
            GUILayout.Label("Shop Icon Preview:");
            Rect rect = GUILayoutUtility.GetRect(MaxPreviewWidth, MaxPreviewWidth);

            Rect textureRect = playerData.shopIcon.textureRect;
            float aspectRatio = textureRect.width / textureRect.height;

            rect.width = MaxPreviewWidth;
            rect.height = MaxPreviewWidth / aspectRatio;

            Texture2D spriteTexture = playerData.shopIcon.texture;

            float normalizedX = textureRect.x / spriteTexture.width;
            float normalizedY = textureRect.y / spriteTexture.height;
            float normalizedWidth = textureRect.width / spriteTexture.width;
            float normalizedHeight = textureRect.height / spriteTexture.height;

            Rect normalizedTextureRect = new Rect(normalizedX, normalizedY, normalizedWidth, normalizedHeight);

            GUI.DrawTextureWithTexCoords(rect, spriteTexture, normalizedTextureRect);
            //EditorGUI.DrawPreviewTexture(rect, spriteTexture, null, ScaleMode.ScaleToFit, 1, 1, 1, 1, normalizedTextureRect);
            /*SerializedProperty spriteProperty = serializedObject.FindProperty("shopIcon");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(spriteProperty, new GUIContent("Select Sprite"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUIUtility.labelWidth);
            GUILayout.Box(playerData.shopIcon.texture, GUILayout.MaxHeight(100), GUILayout.MaxWidth(100));
            GUILayout.EndHorizontal();*/

        }
        serializedObject.ApplyModifiedProperties();
    }
}
