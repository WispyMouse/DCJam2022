using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseWhenSoundFinished : MonoBehaviour
{
    public AudioSource AttachedSource;

    private void OnEnable()
    {
        StartCoroutine(DisableAfterSoundDone());
    }

    IEnumerator DisableAfterSoundDone()
    {
        // Wait for the sound to play, then wait for it to finish
        yield return new WaitUntil(() => AttachedSource.isPlaying);
        yield return new WaitUntil(() => !AttachedSource.isPlaying);

        gameObject.SetActive(false);
    }
}