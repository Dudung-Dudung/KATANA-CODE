using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicData
{
    public AudioClip audioClip;
    public string title;
    public string artist;
    public Sprite album;
    public int level;

    public MusicData(AudioClip Clip, string songTitle, string songArtist, Sprite songAlbum, int songLevel)
    {
        audioClip = Clip;
        title = songTitle;
        this.artist = songArtist;
       this.album = songAlbum;
        //this.album.sprite = songAlbum;
        level = songLevel;
    }
}
