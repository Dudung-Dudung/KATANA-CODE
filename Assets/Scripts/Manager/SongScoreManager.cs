using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

// 곡 정보들 들고 잇는 MusicData JSON 데이터를 담을 클래스
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

// JSON에 의해 직렬화되는 곡 목록 클래스
[System.Serializable]
public class SongList
{
    public List<SongData> songs;
}

public class SongScoreManager : MonoBehaviour
{
    // JSON 파일의 경로
    public string songsJsonPath;

    // 곡 목록
    public List<SongData> songs;

    private void Awake()
    {
        CopyJsonFromStreamingAssetsToPersistentDataPath("MusicData.json");
    }

    // Start 함수에서 호출하여 실행
    private void Start()
    {
        CopyJsonFromStreamingAssetsToPersistentDataPath("MusicData.json");
        // Android에서 JSON 파일 경로 설정
        songsJsonPath = Path.Combine(Application.persistentDataPath, "MusicData.json");
        // 곡 목록 로드
        LoadSongs();
    }

    // 곡 목록 로드
    private void LoadSongs()
    {
        // JSON 파일에서 데이터 읽기
        string json = File.ReadAllText(songsJsonPath);

        // JSON 데이터를 SongList 객체로 역직렬화
        SongList songList = JsonUtility.FromJson<SongList>(json);

        // 곡 목록 설정
        songs = songList.songs;
    }

    // 특정 곡의 점수 갱신
    public void UpdateSongState(string songTitle, float newScore, string newRank, float percentage)
    {
        Debug.Log(songTitle + "곡 점수 갱신");
        // 곡 목록에서 해당 곡 찾기
        SongData songToUpdate = songs.Find(song => song.title == songTitle);

        // 해당 곡이 존재하는지 확인
        if (songToUpdate != null)
        {
            // 점수 갱신
            songToUpdate.score = newScore;

            songToUpdate.rank = newRank;
            songToUpdate.percentage = percentage;

            // JSON 파일에 변경된 내용 저장
            SaveSongs();
        }
        else
        {
            Debug.LogWarning("Song not found: " + songTitle);
        }
    }

    // JSON 파일에 곡 목록 저장
    private void SaveSongs()
    {
        // SongList 객체 생성 및 곡 목록 설정
        SongList songList = new SongList();
        songList.songs = songs;

        // SongList 객체를 JSON 형식으로 직렬화
        string json = JsonUtility.ToJson(songList);

        // JSON 파일에 쓰기
        File.WriteAllText(songsJsonPath, json);
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
                File.WriteAllBytes(destinationPath, request.downloadHandler.data);
                Debug.Log(fileName + " 파일이 복사되었습니다.");
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
