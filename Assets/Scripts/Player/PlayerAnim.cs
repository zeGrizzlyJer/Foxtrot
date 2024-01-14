using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public enum Animations
    {
        Idle,
        Run,
        Jump,
        Fall,
        Injured
    }

    [System.Serializable]
    public struct AnimString
    {
        public Animations anim;
        public string name;
    }

    Animator anim;
    [SerializeField] List<AnimString> animations;
    Dictionary<Animations, string> animDict = new Dictionary<Animations, string>();

    private void Start()
    {
        anim = GetComponent<Animator>();
        foreach (AnimString a in animations)
        {
            animDict.Add(a.anim, a.name);
        }
    }
    #region AnimationFunctions
    public void Idle()
    {
        if (animDict.ContainsKey(Animations.Idle)) anim.Play(animDict[Animations.Idle]);
        else
            Debug.Log($"{name}.{animDict} needs definition for {Animations.Idle}");
    }

    public void Run()
    {
        if (animDict.ContainsKey(Animations.Run)) anim.Play(animDict[Animations.Run]);
        else
            Debug.Log($"{name}.{animDict} needs definition for {Animations.Run}");
    }

    public void Jump()
    {
        if (animDict.ContainsKey(Animations.Jump)) anim.Play(animDict[Animations.Jump]);
        else
            Debug.Log($"{name}.{animDict} needs definition for {Animations.Jump}");
    }

    public void Fall()
    {
        if (animDict.ContainsKey(Animations.Fall)) anim.Play(animDict[Animations.Fall]);
        else
            Debug.Log($"{name}.{animDict} needs definition for {Animations.Fall}");
    }

    public void Injured()
    {
        if (animDict.ContainsKey(Animations.Injured)) anim.Play(animDict[Animations.Injured]);
        else
            Debug.Log($"{name}.{animDict} needs definition for {Animations.Injured}");
    }
    #endregion
}
