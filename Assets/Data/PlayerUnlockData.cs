using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUnlockData", menuName = "Game/Unlock Data")]
public class PlayerUnlockData : ScriptableObject
{
    // Future proofing, in case there might be other conditions - though, unsure what they would be
    [Flags]
    public enum UnlockCondition
    {
        Default = 0,
        Achievement = 1 << 0,
        Purchase = 1 << 1,
        NoCondition = 1 << 2,
    }

    public int unlockID;
    public string unlockName;
    public string description;
    public UnlockCondition condition;
    public bool isUnlocked = false;
    public Sprite shopIcon;
}