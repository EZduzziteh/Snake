using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> tracks = new List<AudioClip>();
    AudioSource aud;

    private void Start()
    {
        aud = GetComponent<AudioSource>();
        int randomNum = Random.Range(0, tracks.Count - 1);
        aud.clip = tracks[randomNum];
        aud.Play();
        aud.loop = true;
    }
}
