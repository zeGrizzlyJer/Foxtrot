using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource audioTemplate2D;
    [SerializeField] private AudioSource audioTemplate3D;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private AudioMixer mixer;

    private Queue<AudioSource> audioPool2D;
    private Queue<AudioSource> audioPool3D;

    [SerializeField] private float audioDelay = 0.3f;
    private float currentDelay = 0f;
    private bool isAudioDelayed = false;

    private void Start()
    {
        audioPool2D = new Queue<AudioSource>();
        audioPool3D = new Queue<AudioSource>();

        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source2D = Instantiate(audioTemplate2D, transform);
            audioPool2D.Enqueue(source2D);

            AudioSource source3D = Instantiate(audioTemplate3D, transform);
            audioPool3D.Enqueue(source3D);
        }
    }

    private void Update()
    {
        if (!isAudioDelayed) return;
        if (currentDelay >= audioDelay)
        {
            isAudioDelayed = false;
            currentDelay = 0f;
            return;
        }
        currentDelay += Time.deltaTime;
    }

    private AudioSource Get2DSource()
    {
        AudioSource source = audioPool2D.Dequeue();
        audioPool2D.Enqueue(source);
        return source;
    }

    public void Play2DSFX(AudioClip clip)
    {
        if (isAudioDelayed) return;
        isAudioDelayed = true;
        AudioSource source = Get2DSource();
        source.volume = 1f;
        source.clip = clip;
        source.Play();
    }

    private AudioSource Get3DSource(Vector3 origin)
    {
        AudioSource source = audioPool3D.Dequeue();
        source.transform.position = origin;
        return source;
    }

    public void Play3DAudio(AudioClip clip, Vector3 origin)
    {
        AudioSource source = Get3DSource(origin);
        source.volume = 1f;
        source.clip = clip;
        float duration = clip.length;
        source.Play();

        StartCoroutine(ReturnSound(duration, source));
    }

    private IEnumerator ReturnSound(float duration, AudioSource source)
    {
        yield return new WaitForSeconds(duration);
        source.transform.SetParent(transform);
        source.transform.localPosition = Vector3.zero;

        audioPool3D.Enqueue(source);
    }

    public void SetMixerVolume(string name, float value)
    {
        PlayerPrefs.SetFloat(name, value);
        mixer.SetFloat(name, Mathf.Log10(value) * 20);
    }
}
