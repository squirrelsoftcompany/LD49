using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    [SerializeField] private AudioSource audioSourceFuelFill;
    [SerializeField] private AudioSource audioSourceAmbient;
    [SerializeField] private AudioSource audioSourceFuelOverflow;
    [SerializeField] private AudioSource audioSourceOverPressure;
    [SerializeField] private AudioSource audioSourceOverTemp;

    [SerializeField] private AudioSource audioSourcePipeClip;
    [SerializeField] private AudioSource audioSourcePressurePurge;
    [SerializeField] private AudioSource audioSourceFuelPurge;
    [SerializeField] private AudioSource audioSourceFreeze;

    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioClip[] audioClipsMusic;

    private int indexMusic;
    private bool shouldPlayBubbles;
    [CanBeNull] private IEnumerator bubbleLoop;
    [CanBeNull] private IEnumerator fadeOutCoroutine, fadeInCoroutine;

    private bool musicOn;

    // Start is called before the first frame update
    private void Start() {
        indexMusic = 0;
        musicOn = true;
    }

    private IEnumerator playNextMusic() {
        if (audioClipsMusic.Length > 0)
        {
            while (musicOn)
            {
                audioSourceMusic.clip = audioClipsMusic[indexMusic];
                audioSourceMusic.Play();
                yield return new WaitForSeconds(audioClipsMusic[indexMusic].length + 2);
                indexMusic = (indexMusic + 1) % audioClipsMusic.Length;
            }

        }
    }

    private void OnEnable() {
        musicOn = true;
    }

    private void OnDisable() {
        musicOn = false;
    }

    public void playFuelFill(bool start)
    {
        if(start)
        {
            audioSourceFuelFill.loop = true;
            audioSourceFuelFill.Play();
        }
        else
        {
            audioSourceFuelFill.Stop();
        }
    }

    public void playOverPressure(bool start)
    {
        if (start)
        {
            audioSourceOverPressure.loop = true;
            audioSourceOverPressure.Play();
        }
        else
        {
            audioSourceOverPressure.Stop();
        }
    }
    public void playOverTemp(bool start)
    {
        if (start)
        {
            audioSourceOverTemp.loop = true;
            audioSourceOverTemp.Play();
        }
        else
        {
            audioSourceOverTemp.Stop();
        }
    }

    public void playPressurePurge(bool start)
    {
        if (start)
        {
            audioSourcePressurePurge.loop = true;
            audioSourcePressurePurge.Play();
        }
        else
        {
            audioSourcePressurePurge.Stop();
        }
    }
    public void playFuelPurge(bool start)
    {
        if (start)
        {
            audioSourceFuelPurge.loop = true;
            audioSourceFuelPurge.Play();
        }
        else
        {
            audioSourceFuelPurge.Stop();
        }
    }

    public void playFuelOverflow(bool start)
    {
        if (start)
        {
            audioSourceFuelOverflow.loop = true;
            audioSourceFuelOverflow.Play();
        }
        else
        {
            audioSourceFuelOverflow.Stop();
        }
    }

    public void playFreeze()
    {
        audioSourceFreeze.Play();
    }

    public void playPipeClip() {
        audioSourcePipeClip.Play();
    }



    private void killAllCoroutines() {
        if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
        if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
    }

    private static IEnumerator fadeOut(AudioSource audioSource, float fadeTime, float threshold) {
        while (audioSource.volume > threshold) {
            audioSource.volume -= Time.deltaTime / fadeTime;
            yield return null;
        }
    }

    private static IEnumerator fadeIn(AudioSource audioSource, float fadeTime, float threshold) {
        while (audioSource.volume < threshold) {
            audioSource.volume += Time.deltaTime / fadeTime;
            yield return null;
        }
    }
}