using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class MusicManager2 : MonoBehaviour
{

    private List<MusicData> musicList = new List<MusicData>();

    public List<RectTransform> questPanelPosition = new List<RectTransform>();

    public GameObject panelPrefab;
    public GameObject QuestPanelList;
    private AudioSource songAudio;

    private string MusicDataFile;

    static int count = -1;
    
    bool isTweening = false;
    bool isNext = false;
    bool isPre = true;
    bool isPreSecond = true;
    bool isNextSecond = false;
    bool isSecond = false;

    public TextMeshProUGUI title;
    public TextMeshProUGUI artist;

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
    }


    public void GameStart()
    {
        songAudio = GetComponent<AudioSource>();

        //�г� ����
        Instantiate(panelPrefab, questPanelPosition[0].position, questPanelPosition[0].rotation, QuestPanelList.transform);
        Instantiate(panelPrefab, questPanelPosition[1].position, questPanelPosition[1].rotation, QuestPanelList.transform);
        Instantiate(panelPrefab, questPanelPosition[2].position, questPanelPosition[2].rotation, QuestPanelList.transform);

        ReadJson("MusicData");
        UpdateSongInfo();
    }
    private void ReadJson(string json)
    {
        //json ���� �б�
        TextAsset jsonFile = Resources.Load<TextAsset>(json);

        if (jsonFile != null)
        {
            // JSON ���� ������ ���ڿ��� �о�ɴϴ�.
            string jsonString = jsonFile.text;

            // JSON ���ڿ��� �Ľ��Ͽ� SongData ��ü�� ��ȯ�մϴ�.
            songData = JsonUtility.FromJson<SongData>(jsonString);

            // SongData ��ü�� ����մϴ�.
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
            Debug.LogError("JSON ������ ���� �� �����ϴ�: " + MusicDataFile);
        }
    }
    public void UpdateSongInfo()
    {
        Debug.Log("*************************");
        Debug.Log("UpdateSongInfo");

        for (int i = 0; i < 3; i++)
        {

            if (count >= songData.songs.Length) count = 0;
            else if (count < 0) count = songData.songs.Length + count;
            Debug.Log("count : " + count + " i : " + i + " tltle : " + songData.songs[count].title);
            //�̹��� ����
            Sprite coverImage = LoadSpriteFromPath(songData.songs[count].cover_image_path);
            QuestPanelList.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = coverImage;
            count = AddNumber(count);
        }
        count = 0;
        //�� ��ȯ
        AudioClip musicClip = Resources.Load<AudioClip>(songData.songs[count].audio_file_path);
        songAudio.clip = musicClip;
        songAudio.Play();
        Debug.Log(songData.songs[count].title);
        title.text = songData.songs[count].title;
        artist.text = songData.songs[count].artist;
    }
    private int AddNumber(int count)
    {
        if (count + 1 > songData.songs.Length - 1) count = 0;
        else count++;
        return count;
    }
    private int SubNumber(int count)
    {
        if (count - 1 < 0) count = songData.songs.Length - 1;
        else count--;
        return count;
    }
    private Sprite LoadSpriteFromPath(string path)
    {
        // �̹��� ������ �ε�
        Texture2D texture = Resources.Load<Texture2D>(path);
        if (!texture) Debug.Log("texture is null" + path);
        // Texture2D�� Sprite�� ��ȯ
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        return sprite;
    }
    public void SongInfoNext() //�гε��� ���������� �̵�
    {
        if (isTweening) return;
   //     Debug.Log("SongInfoNext");
        //���� ���� ���� ������Ʈ
        count = SubNumber(count);
        //���� �� ���� ���
        title.text = songData.songs[count].title;
        artist.text = songData.songs[count].artist;
        AudioClip musicClip = Resources.Load<AudioClip>(songData.songs[count].audio_file_path);
        songAudio.clip = musicClip;
        songAudio.Play();

        //���� ������ �гο� �гο� ���� �� count update
        count = SubNumber(count);

        //child[2]�� ��ġ�� �г� ����
        GameObject newPanel = Instantiate(panelPrefab, questPanelPosition[0].position, questPanelPosition[0].rotation, QuestPanelList.transform);

        //���� ���� �гο� ���� �ֱ�
        Sprite coverImage = LoadSpriteFromPath(songData.songs[count].cover_image_path);
        newPanel.transform.GetChild(0).GetComponent<Image>().sprite = coverImage;
        //count�� ���� � �����ֱ�
        count = AddNumber(count);
        //������ �ٸ� �������� �̵��� ���
        int destroy = 0;

        if (isPre) 
        { 
            destroy = 2;
            isPre = false;
            isNextSecond = false;
        } 
        else if(isPreSecond)
        {
            destroy = 1;
            isNextSecond = true;
            isPreSecond = false;
        }
            
        
         Debug.Log("destroy : " + destroy + "isPre : " + isPre + "isNextSecond : " + isNextSecond);
        //�г� destroy
        Destroy(QuestPanelList.transform.GetChild(destroy).gameObject);

        //�г� �̵�
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < 3; i++)
        {
            RectTransform rectTransform = QuestPanelList.transform.GetChild(i).GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                sequence.Join(rectTransform.DOAnchorPos3D(new Vector3(rectTransform.anchoredPosition3D.x + 650, y: rectTransform.anchoredPosition3D.y, rectTransform.anchoredPosition3D.z), 0.5f)
                    .OnComplete(() =>
                    {
                        isTweening = false;
                    }));
            }
        }
        isNext = true;
    }
    public void SongInfoPre() //�гε��� �������� �̵�
    {
        if (isTweening) return;
  //      Debug.Log("SongInfoPre");

        //���� ���� ���� ������Ʈ
        count = AddNumber(count);

        //���� �� ���� ���
        title.text = songData.songs[count].title;
        artist.text = songData.songs[count].artist;
        AudioClip musicClip = Resources.Load<AudioClip>(songData.songs[count].audio_file_path);
        songAudio.clip = musicClip;
        songAudio.Play();

        //�гο� ���� ���ο� �� count update
        count = AddNumber(count);

        //child[2]�� ��ġ�� �г� ����
        GameObject newPanel = Instantiate(panelPrefab, questPanelPosition[2].position, questPanelPosition[2].rotation, QuestPanelList.transform);

        //���� ���� �гο� ���� �ֱ�
        Sprite coverImage = LoadSpriteFromPath(songData.songs[count].cover_image_path);
        newPanel.transform.GetChild(0).GetComponent<Image>().sprite = coverImage;

        //count�� ���� � �����ֱ�
        count = SubNumber(count);
        //������ �ٸ� �������� �̵��� ���
        int destroy = 0;
        if (isNext)
        {
            destroy = 2;
            isNext = false;
            isPreSecond = false;
        }
        else if(isNextSecond)
        {
            destroy = 1;
            isPreSecond = true;
            isNextSecond = false;

        }
        
        Debug.Log("destroy : " + destroy + "isNext : " + isNext + "isNextSecond : " + isNextSecond);

        //�г� destroy
        Destroy(QuestPanelList.transform.GetChild(destroy).gameObject);
        //�г� �̵�
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < 3; i++)
        {
            RectTransform rectTransform = QuestPanelList.transform.GetChild(i).GetComponent<RectTransform>();
            //sequence.Join(sequence.Join(rectTransform.DOAnchorPos3D(new Vector3(rectTransform.anchoredPosition3D.x - 650, y: rectTransform.anchoredPosition3D.y, rectTransform.anchoredPosition3D.z), 0.5f)));
            DG.Tweening.Sequence sequence1 = sequence.Join(rectTransform.DOAnchorPos3D(new Vector3(rectTransform.anchoredPosition3D.x - 650, y: rectTransform.anchoredPosition3D.y, rectTransform.anchoredPosition3D.z), 0.5f)
              .OnComplete(() =>
              {
                  isTweening = false;

              }));
        }
        isPre = true;
    }

    public void NextScene()
    {
        SceneManager.LoadScene(0);
    }
}