using System.Collections;
using UnityEngine;

/// <summary>
/// Global entry point for playing sound effects and music.
/// </summary>
public class AudioManager : MonoBehaviour
{
    const float FadeInTime = 4f;
    const float MinRandomPitch = .6f;
    const float MaxRandomPitch = 1.2f;

    public static AudioManager Instance { get; set; }

    [SerializeField]
    AudioSource CurrentMusic;
    [SerializeField]
    AudioSource IncomingMusic;

    [SerializeField]
    ReleaseWhenSoundFinished soundPlayerPF;

    Coroutine musicFadeInCoroutine { get; set; }
    ObjectPool<ReleaseWhenSoundFinished> audioSourcePoolPointer { get; set; }
    float curFadeInTime { get; set; } = 0f;

    void Awake()
    {
        // If this is loaded and there's another AudioManager, destroy this one
        // otherwise, mark this as don't destroy on load so the music persists between scenes
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        audioSourcePoolPointer = GlobalObjectPool.GetObjectPool<ReleaseWhenSoundFinished>(soundPlayerPF, 30);
    }

    public void PlaySfx(AudioClip toPlay, float minRandomPitch = MinRandomPitch, float maxRandomPitch = MaxRandomPitch, float volume = 1)
    {
        ReleaseWhenSoundFinished player = Instance.audioSourcePoolPointer.GetObject();
        player.AttachedSource.pitch = Random.Range(MinRandomPitch, MaxRandomPitch);
        player.AttachedSource.volume = volume;
        player.AttachedSource.clip = toPlay;
        player.AttachedSource.Play();
    }

    public void PlayMusic(AudioClip music)
    {
        // If we're asked to play music already being played, and we're not mid transition,
        // take no action rather than stopping and starting it
        if (CurrentMusic.clip == music && musicFadeInCoroutine == null)
        {
            return;
        }

        // If we are mid transition, but the incoming clip is the same as parameter, then take no action
        if (musicFadeInCoroutine != null && IncomingMusic.clip == music)
        {
            return;
        }

        // If we're not playing any music, then start playing this immediately
        if (CurrentMusic.clip == null)
        {
            CurrentMusic.clip = music;
            CurrentMusic.Play();
            return;
        }

        // Stop the previous fade in coroutine and start a new one
        if (musicFadeInCoroutine != null)
        {
            StopCoroutine(musicFadeInCoroutine);
            AudioSource swap = CurrentMusic;
            CurrentMusic = IncomingMusic;
            IncomingMusic = swap;
        }

        musicFadeInCoroutine = StartCoroutine(FadeInMusic(music));
    }

    IEnumerator FadeInMusic(AudioClip newMusic)
    {
        // if the fade in process was interrupted, set the volume of the music playing to where it was when interrupted
        // use this value instead of full volume so that interrupting a fade in at the start doesn't make the
        // volume jump to full
        float maxCurrentMusicVolume = Mathf.Lerp(1f, 0f, curFadeInTime / FadeInTime);
        curFadeInTime = 0;

        IncomingMusic.clip = newMusic;

        IncomingMusic.volume = 0f;
        CurrentMusic.volume = 1f;

        IncomingMusic.Play();

        WaitForEndOfFrame yieldedWait = new WaitForEndOfFrame();

        while (curFadeInTime < FadeInTime)
        {
            curFadeInTime += Time.deltaTime;

            IncomingMusic.volume = Mathf.Lerp(0, 1f, curFadeInTime / FadeInTime);
            CurrentMusic.volume = Mathf.Lerp(maxCurrentMusicVolume, 0, curFadeInTime / FadeInTime);

            yield return yieldedWait;
        }

        AudioSource swap = CurrentMusic;
        CurrentMusic.Stop();
        CurrentMusic = IncomingMusic;
        IncomingMusic = swap;
        IncomingMusic.clip = null;
        curFadeInTime = 0;

        Instance.musicFadeInCoroutine = null;
    }
}