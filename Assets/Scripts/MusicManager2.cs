using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Rendering.Universal;

public class MusicManager2 : MonoBehaviour
{

    LinkedList<GameObject> panel = new LinkedList<GameObject>();


    public List<RectTransform> questPanelPosition = new List<RectTransform>();

    public GameObject panelPrefab;
    public GameObject QuestPanelList;
    private AudioSource songAudio;
    private int totalSongs;
    private bool isTweening;

    private string MusicDataFile;

    static int count = -2;

    public TextMeshProUGUI title;
    public TextMeshProUGUI artist;
    public TextMeshProUGUI score;
    public TextMeshProUGUI rank;

    SongData songData;

    [System.Serializable]
    public class SongData
    {
        public Song[] songs;
    }

    [System.Serializable]
    public class Song
    {
        public string title;
        public string artist;
        public string cover_image_path;
        public string audio_file_path;
        public string score;
        public string rank;
    }


    public void GameStart()
    {

        songAudio = GetComponent<AudioSource>();
        
        for(int i = 0; i < 5; i++)
        {
           panel.AddLast(Instantiate(panelPrefab, questPanelPosition[i].position, questPanelPosition[i].rotation, QuestPanelList.transform));

        }
        ReadJson("MusicData");
        UpdateSongInfo();
    }
    private void ReadJson(string json)
    {
        //json 파일 읽기
        TextAsset jsonFile = Resources.Load<TextAsset>(json);

        if (jsonFile != null)
        {
            // JSON 파일 내용을 문자열로 읽어옵니다.
            string jsonString = jsonFile.text;

            // JSON 문자열을 파싱하여 SongData 객체로 변환합니다.
            songData = JsonUtility.FromJson<SongData>(jsonString);

            // SongData 객체를 사용합니다.
            foreach (Song song in songData.songs)
            {
                Debug.Log("Title: " + song.title);
                Debug.Log("Artist: " + song.artist);
                Debug.Log("Cover Image Path: " + song.cover_image_path);
                Debug.Log("Audio File Path: " + song.audio_file_path);
            }
        }
        else
        {
            Debug.LogError("JSON 파일을 읽을 수 없습니다: " + MusicDataFile);
        }
    }
    public void UpdateSongInfo()
    {
        Debug.Log("*************************");
        Debug.Log("UpdateSongInfo");

        for (int i = 0; i < 5; i++)
        {

            if (count >= songData.songs.Length) count = 0;
            else if (count < 0) count = songData.songs.Length + count;
            Debug.Log("count : " + count + " i : " + i + " tltle : " + songData.songs[count].title);
            //이미지 변경
            Sprite coverImage = LoadSpriteFromPath(songData.songs[count].cover_image_path);
            QuestPanelList.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = coverImage;
            count = AddNumber(songData.songs.Length - 1, count);
        }
        count = 0;
        //곡 변환
        AudioClip musicClip = Resources.Load<AudioClip>(songData.songs[count].audio_file_path);
        songAudio.clip = musicClip;
        songAudio.Play();
        Debug.Log(songData.songs[count].title);
        title.text = songData.songs[count].title;
        artist.text = songData.songs[count].artist;
        score.text = songData.songs[count].score;
        rank.text = songData.songs[count].rank;

        totalSongs = songData.songs.Length - 1;

    }
    private int AddNumber(int max, int count)
    {
        if (count + 1 > max) count = 0;
        else count++;
        return count;
    }
    private int SubNumber(int max, int count)
    {
        if (count - 1 < 0) count = max;
        else count--;
        return count;
    }
    private Sprite LoadSpriteFromPath(string path)
    {
        // 이미지 파일을 로드
        Texture2D texture = Resources.Load<Texture2D>(path);
        if (!texture) Debug.Log("texture is null" + path);
        // Texture2D를 Sprite로 변환
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        return sprite;
    }
    public void NextSong()
    {
        //패널 이동
        if (isTweening) return;
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        foreach (GameObject obj in panel)
        {
            RectTransform rectTransform = obj.GetComponent<RectTransform>();//QuestPanelList.transform.GetChild(i).GetComponent<RectTransform>();
            //sequence.Join(sequence.Join(rectTransform.DOAnchorPos3D(new Vector3(rectTransform.anchoredPosition3D.x - 650, y: rectTransform.anchoredPosition3D.y, rectTransform.anchoredPosition3D.z), 0.5f)));
            DG.Tweening.Sequence sequence1 = sequence.Join(rectTransform.DOAnchorPos3D(new Vector3(rectTransform.anchoredPosition3D.x + 650, y: rectTransform.anchoredPosition3D.y, rectTransform.anchoredPosition3D.z), 0.5f)
              .OnComplete(() =>
              {
                  isTweening = false;

              }));
        }
        //현재 곡을 새로 업데이트
        count = SubNumber(totalSongs, count);
        //현재 곡 정보 출력
        title.text = songData.songs[count].title;
        artist.text = songData.songs[count].artist;
        score.text = songData.songs[count].score;
        rank.text = songData.songs[count].rank;


        AudioClip musicClip = Resources.Load<AudioClip>(songData.songs[count].audio_file_path);
        songAudio.clip = musicClip;
        songAudio.Play();

        //새로 생성된 패널에 패널에 넣을 곡 count update
        count = SubNumber(totalSongs, count);
        count = SubNumber(totalSongs, count);

        //child[2]번 위치에 패널 생성
        panel.AddFirst(Instantiate(panelPrefab, questPanelPosition[0].position, questPanelPosition[0].rotation, QuestPanelList.transform));

        //새로 만든 패널에 정보 넣기
        Sprite coverImage = LoadSpriteFromPath(songData.songs[count].cover_image_path);
        panel.First.Value.transform.GetChild(0).GetComponent<Image>().sprite = coverImage;
        //count를 현재 곡에 맞춰주기
        count = AddNumber(totalSongs, count);
        count = AddNumber(totalSongs, count);

        //패널 삭제
        Destroy(panel.Last.Value);
        panel.RemoveLast();

    }

    public void PrevSong()
    {
        //패널 이동
        if (isTweening) return;
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        foreach (GameObject obj in panel)
        {
            RectTransform rectTransform = obj.GetComponent<RectTransform>();//QuestPanelList.transform.GetChild(i).GetComponent<RectTransform>();
            DG.Tweening.Sequence sequence1 = sequence.Join(rectTransform.DOAnchorPos3D(new Vector3(rectTransform.anchoredPosition3D.x - 650, y: rectTransform.anchoredPosition3D.y, rectTransform.anchoredPosition3D.z), 0.5f)
              .OnComplete(() =>
              {
                  isTweening = false;

              }));
        }
       
        //현재 곡을 새로 업데이트
        count = AddNumber(totalSongs, count);

        //현재 곡 정보 출력
        title.text = songData.songs[count].title;
        artist.text = songData.songs[count].artist;
        score.text = songData.songs[count].score;
        rank.text = songData.songs[count].rank;


        AudioClip musicClip = Resources.Load<AudioClip>(songData.songs[count].audio_file_path);
        songAudio.clip = musicClip;
        songAudio.Play();

        //패널에 넣을 새로운 곡 count update
        count = AddNumber(totalSongs, count);
        count = AddNumber(totalSongs, count);

        //child[2]번 위치에 패널 생성
        panel.AddLast(Instantiate(panelPrefab, questPanelPosition[4].position, questPanelPosition[4].rotation, QuestPanelList.transform));

        //GameObject newPanel = Instantiate(panelPrefab, questPanelPosition[2].position, questPanelPosition[2].rotation, QuestPanelList.transform);

        //새로 만든 패널에 정보 넣기
        Sprite coverImage = LoadSpriteFromPath(songData.songs[count].cover_image_path);
        panel.Last.Value.transform.GetChild(0).GetComponent<Image>().sprite = coverImage;

        //count를 현재 곡에 맞춰주기
        count = SubNumber(totalSongs, count);
        count = SubNumber(totalSongs, count);


        //패널 삭제
        Destroy(panel.First.Value);
        panel.RemoveFirst();

    }

    public void NextScene()
    {
        GameManager.songTitle = songData.songs[count].title;
        Debug.Log(GameManager.songTitle + "현재 선택한 곡 이름 - MusicManager2");
        SceneManager.LoadScene(0);
    }
}