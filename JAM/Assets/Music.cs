using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private bool playOnStart;
    [SerializeField] private AudioSource audio;
    [SerializeField] private float loopStart;
    [SerializeField] private float loopEnd;

    public bool shouldReplay;

    // Start is called before the first frame update
    void Start()
    {
        shouldReplay = false;
        if (playOnStart)
        {
            audio.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (audio.time > loopStart)
        {
            shouldReplay = true;
        }
        if (!audio.isPlaying && shouldReplay)
        {
            audio.time = loopStart;
            audio.Play();
        }
    }
}
