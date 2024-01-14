using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleAnim : MonoBehaviour
{
    public enum Animations
    { 
        Run,
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
        anim.speed = 0f;
    }

    public void Run()
    {
        anim.speed = 1f;
        if (animDict.ContainsKey(Animations.Run)) anim.Play(animDict[Animations.Run]);
        else
            Debug.Log($"{name}.{animDict} needs definition for {Animations.Run}");
    }

    public void Injured()
    {
        anim.speed = 1f;
        if (animDict.ContainsKey(Animations.Injured)) anim.Play(animDict[Animations.Injured]);
        else
            Debug.Log($"{name}.{animDict} needs definition for {Animations.Injured}");
    }
    #endregion
}
