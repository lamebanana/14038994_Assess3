using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    public AudioClip dead_state;
    public AudioClip scared_state;
    public AudioClip normal_state;
    public AudioClip intro;

    public AudioSource music;
    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();
        music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!music.isPlaying)
        {
            music.clip = normal_state;
            music.Play();
        }
    }
}
