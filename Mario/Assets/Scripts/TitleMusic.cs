using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMusic : MonoBehaviour
{
    public AudioSource titleMusic;
    public AudioClip winMusic;

    public PlayerContoller player;

    private int count = 0;
    //public AudioClip deadMusic;
    // Start is called before the first frame update
    void Start()
    {
        titleMusic = GetComponent<AudioSource>();
        titleMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDead)
        {
            titleMusic.Stop();
        }
        if (player.isWin && count == 0)
        {
            count++;
            playWinMusic();
        }
    }

    private void playWinMusic()
    {
        titleMusic.Stop();
        titleMusic.PlayOneShot(winMusic);
    }
}
