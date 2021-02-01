using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSFX : MonoBehaviour
{
    [SerializeField] AudioClip[] clips = default;
    [SerializeField] float minTime = 15f;
    [SerializeField] float maxTime = 40f;

    private float timeOfLastPlay;
    private float timeNextPlay;

    private void Start()
    {
        timeOfLastPlay = Time.time + Random.Range(1f, 3f);
    }

    private void Update()
    {
        if(Time.time > timeOfLastPlay + timeNextPlay)
        {
            PlayRandomSFX();
        }
    }

    private void PlayRandomSFX()
    {
        timeNextPlay = Random.Range(minTime, maxTime);
        timeOfLastPlay = Time.time;

        int random = Random.Range(0, clips.Length);
        AudioSource.PlayClipAtPoint(clips[random], Camera.main.transform.position);

    }
}
