using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class SongData
{
    public string title;
    public string artist;
    public string cover_image_path;
    public string audio_file_path;
    public float score;
    public string rank;
    public float percentage;
}

[System.Serializable]
public class SongList
{
    public List<SongData> songs;
}

public class SongScoreManager : MonoBehaviour
{
    public string songsJsonPath;
    public List<SongData> songs;

    private void Awake()
    {
        StartCoroutine(CopyJsonFromStreamingAssetsToPersistentDataPath("MusicData.json"));
    }

    private void Start()
    {
        songsJsonPath = Path.Combine(Application.persistentDataPath, "MusicData.json");
        LoadSongs();
    }

    public void LoadSongs()
    {
        try
        {
            if (File.Exists(songsJsonPath))
            {
                string json = File.ReadAllText(songsJsonPath);
                SongList songList = JsonUtility.FromJson<SongList>(json);
                songs = songList.songs;
                Debug.Log("Songs loaded successfully.");
            }
            else
            {
                Debug.LogWarning("Songs JSON file not found.");
            }
        }
        catch (IOException e)
        {
            Debug.LogError("Failed to load songs: " + e.Message);
        }
    }

    public void SaveSongs()
    {
        try
        {
            SongList songList = new SongList { songs = songs };
            string json = JsonUtility.ToJson(songList);
            File.WriteAllText(songsJsonPath, json);
            Debug.Log("Songs saved successfully.");
        }
        catch (IOException e)
        {
            Debug.LogError("Failed to save songs: " + e.Message);
        }
    }

    public void UpdateSongState(string songTitle, float newScore, string newRank, float percentage)
    {
        Debug.Log(songTitle + "곡 점수 갱신");
        SongData songToUpdate = songs.Find(song => song.title == songTitle);

        if (songToUpdate != null)
        {
            songToUpdate.score = newScore;
            songToUpdate.rank = newRank;
            songToUpdate.percentage = percentage;

            SaveSongs();
        }
        else
        {
            Debug.LogWarning("Song not found: " + songTitle);
        }
    }

    private IEnumerator CopyJsonFromStreamingAssetsToPersistentDataPath(string fileName)
    {
        string streamingAssetsPath = Path.Combine(Application.streamingAssetsPath, fileName);
        string destinationPath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(destinationPath))
        {
            UnityWebRequest request = UnityWebRequest.Get(streamingAssetsPath);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    File.WriteAllBytes(destinationPath, request.downloadHandler.data);
                    Debug.Log(fileName + " 파일이 복사되었습니다.");
                }
                catch (IOException e)
                {
                    Debug.LogError("Failed to write file: " + e.Message);
                }
            }
            else
            {
                Debug.LogError("StreamingAssets에서 " + fileName + " 파일을 가져오는 데 실패했습니다: " + request.error);
            }
        }
        else
        {
            Debug.Log(fileName + " 파일이 이미 존재합니다.");
        }
    }
}
