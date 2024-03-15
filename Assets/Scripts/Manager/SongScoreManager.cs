using UnityEngine;
using System.Collections.Generic;
using System.IO;


// JSON �����͸� ���� Ŭ����
[System.Serializable]
public class SongData
{
    public string title;
    public string artist;
    public string cover_image_path;
    public string audio_file_path;
    public float score;
}

// JSON�� ���� ����ȭ�Ǵ� �� ��� Ŭ����
[System.Serializable]
public class SongList
{
    public List<SongData> songs;
}

public class SongScoreManager : MonoBehaviour
{
    // JSON ������ ���
    public string songsJsonPath = "Assets/Resources/MusicData.json";

    // �� ���
    public List<SongData> songs;

    // Start �Լ����� ȣ���Ͽ� ����
    private void Start()
    {
        // �� ��� �ε�
        LoadSongs();
    }

    // �� ��� �ε�
    private void LoadSongs()
    {
        // JSON ���Ͽ��� ������ �б�
        string json = File.ReadAllText(songsJsonPath);

        // JSON �����͸� SongList ��ü�� ������ȭ
        SongList songList = JsonUtility.FromJson<SongList>(json);

        // �� ��� ����
        songs = songList.songs;
    }

    // Ư�� ���� ���� ����
    public void UpdateScore(string songTitle, float newScore)
    {
        // �� ��Ͽ��� �ش� �� ã��
        SongData songToUpdate = songs.Find(song => song.title == songTitle);

        // �ش� ���� �����ϴ��� Ȯ��
        if (songToUpdate != null)
        {
            // ���� ����
            songToUpdate.score = newScore;

            // JSON ���Ͽ� ����� ���� ����
            SaveSongs();
        }
        else
        {
            Debug.LogWarning("Song not found: " + songTitle);
        }
    }

    // JSON ���Ͽ� �� ��� ����
    private void SaveSongs()
    {
        // SongList ��ü ���� �� �� ��� ����
        SongList songList = new SongList();
        songList.songs = songs;

        // SongList ��ü�� JSON �������� ����ȭ
        string json = JsonUtility.ToJson(songList);

        // JSON ���Ͽ� ����
        File.WriteAllText(songsJsonPath, json);
    }
}


