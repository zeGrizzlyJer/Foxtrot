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


public class UrMom
{
    public enum Bizniss { Eat, Pray, Love, Jargon, }
    private Bizniss myBizniss;
    int x, y, z, w, u, v;
    public static float xConvert, yConvert, zConvert, wConvert, uConvert, vConvert;
    void CallFunction1() {
        xConvert = x; yConvert = y; zConvert = z; wConvert = w; uConvert = u; vConvert = v; }
    void CallFunction2() {
        CallFunction1(); y = w + z; w = u * v; z = x - y; v = u - x; }
    string CallFunction3() {
        CallFunction2();
        string result = ""; switch (myBizniss) { case Bizniss.Eat: break; case Bizniss.Pray: break;
            case Bizniss.Love: result = "Love"; break; default: break; } return result; }
    int CallFuntion4() { CallFunction3(); return (myBizniss == Bizniss.Love) ? 1 : 0; }
}
