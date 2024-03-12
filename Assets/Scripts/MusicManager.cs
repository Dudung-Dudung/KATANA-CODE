using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.XR.CoreUtils;
using System.IO;
using DG.Tweening.Plugins.Core.PathCore;
//using UnityEngine.UIElements;

public class MusicManager : MonoBehaviour
{
   
    private List<MusicData> musicList = new List<MusicData>();

    public List<RectTransform> questPanel = new List<RectTransform>();
    public List<RectTransform> questPanelPosition = new List<RectTransform>();

    public List<AudioClip> audioClips = new List<AudioClip>();
    public List<string> titles = new List<string>();
    public List<string> artists = new List<string>();
    public List<Sprite> albums = new List<Sprite>();

    public GameObject panelPrefab;
    public GameObject QuestPanelList;
    private AudioSource songAudio;

    public string MusicDataFile;

    static int count = -2;
    int position = 0;
    int delete;

    bool isTweening = false;
    bool isRight = false;
    bool isLeft = false;
    bool isFirst = true;

    int countNext = 0;

    SongData songData;

    private void Awake()
    {
        /*
        for (int i = 0; i < audioClips.Count; i++)
        {
            musicList.Add(new MusicData(audioClips[i], titles[i], artists[i], albums[i], 0));

        }*/

        songAudio = GetComponent<AudioSource>();

        //Instantiate(panelPrefab, questPanelPosition[1], GameObject.Find("QuestPanelList").transform);
        
        Instantiate(panelPrefab, questPanelPosition[0].position, Quaternion.identity, QuestPanelList.transform);
        Instantiate(panelPrefab, questPanelPosition[1].position, Quaternion.identity, QuestPanelList.transform);
        Instantiate(panelPrefab, questPanelPosition[2].position, Quaternion.identity, QuestPanelList.transform);
        Instantiate(panelPrefab, questPanelPosition[3].position, Quaternion.identity, QuestPanelList.transform);
        Instantiate(panelPrefab, questPanelPosition[4].position, Quaternion.identity, QuestPanelList.transform);
        
          UpdateSongInfo();
        
    }

    private void Start()
    {
        // Resources ������ �ִ� JSON ������ �о�ɴϴ�.
        TextAsset jsonFile = Resources.Load<TextAsset>("MusicData");

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
   
    
    public void UpdateSongInfo()
    {
        Debug.Log("*************************");
        TextAsset jsonFile = Resources.Load<TextAsset>("MusicData");

        if (jsonFile != null)
        {
            // JSON ���� ������ ���ڿ��� �о�ɴϴ�.
            string jsonString = jsonFile.text;

            // JSON ���ڿ��� �Ľ��Ͽ� SongData ��ü�� ��ȯ�մϴ�.
            songData = JsonUtility.FromJson<SongData>(jsonString);

        }
            Debug.Log("musicList.Count :" + songData.songs.Length);
        Debug.Log("0 :" + count);
        for (int i = 0; i < 5; i++)
        {
            if (count >= songData.songs.Length) count = 0;
            else if (count < 0) count = songData.songs.Length + count;
            Debug.Log("count : " + count + " i : " + i + " tltle : " + songData.songs[count].title); //musicList[count].title);
            QuestPanelList.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = songData.songs[count].title;
            QuestPanelList.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = songData.songs[count].artist;
            Sprite coverImage = LoadSpriteFromPath(songData.songs[count].cover_image_path);
            QuestPanelList.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = coverImage;
            if (i == 2)
            {
                AudioClip musicClip = Resources.Load<AudioClip>(songData.songs[count].audio_file_path);
                songAudio.clip = musicClip;//musicList[count].audioClip;
                songAudio.Play();
            }
            count++;
            Debug.Log("2 :" + count);
        }
        count -= 3;
        Debug.Log("3 : " + count);
        if (count < 0) count = songData.songs.Length + count; Debug.Log("musicList.Count" + musicList.Count + "count" + count);
        
        Debug.Log("count :" + count);

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
    /*
     *  public void UpdateSongInfo()
    {
        Debug.Log("*************************");
        Debug.Log("musicList.Count :" + musicList.Count);
        Debug.Log("0 :" + count);
        for (int i = 0; i < 5; i++)
        {
            if (count >= musicList.Count) count = 0;
            else if (count < 0) count = musicList.Count + count;
            Debug.Log("count : " + count + " i : " + i + " tltle : " + musicList[count].title);
            QuestPanelList.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = musicList[count].title;
            QuestPanelList.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = musicList[count].artist;
            QuestPanelList.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = musicList[count].album;
            if (i == 2)
            {
                songAudio.clip = musicList[count].audioClip;
                songAudio.Play();
            }
            count++;
            Debug.Log("2 :" + count);
        }
        count -= 3;
        Debug.Log("3 : " + count);
        if (count < 0) count = musicList.Count + count; Debug.Log("musicList.Count" + musicList.Count + "count" + count);
        
        Debug.Log("count :" + count);

    }
     */

    public void UpdateSongInfoMoveNext()
    {
        if (isTweening) return;
        if (isFirst) delete = 4; isFirst = false;

        //delete ���� �� ����
        isRight = true;
        if (isLeft)
        {
            delete = 4;
            isLeft = false;
            countNext = 0;
           
        }

        // �̵� ������ ǥ��
        isTweening = true;

        // ���ο� �г� ����
        GameObject newPanel = Instantiate(panelPrefab, questPanelPosition[0].position, Quaternion.identity, QuestPanelList.transform);

        //���ο� �гο� ���� �ֱ�
        for (int i = 0; i < 3; i++)
        {
            count--;
            if (count >= songData.songs.Length) count = 0;
            else if (count < 0) count = songData.songs.Length + count;
        }


        newPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = songData.songs[count].title;//musicList[count].title;
        newPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = songData.songs[count].artist;//musicList[count].artist;
        Sprite coverImage = LoadSpriteFromPath(songData.songs[count].cover_image_path);
        newPanel.transform.GetChild(2).GetComponent<Image>().sprite = coverImage;//musicList[count].album;

       // QuestPanelList.transform.GetChild(count).GetChild(0).GetComponent<TextMeshProUGUI>().text);
        // ���� �ٲٱ�
        for (int i = 0; i < 2; i++)
        {
            count++;
            if (count >= songData.songs.Length) count = 0;
            else if (count < 0) count = songData.songs.Length + count;
        }
        AudioClip musicClip = Resources.Load<AudioClip>(songData.songs[count].audio_file_path);
        songAudio.clip = musicClip;
        Debug.Log("songData.songs[count].title : " + songData.songs[count].title);

        Debug.Log("audio_file_path : " + songData.songs[count].audio_file_path);
        songAudio.Play();
        //�г� �̵�
        int movedPanels = 0;
        for (int i = 0; i <= 4; i++)
        {

            RectTransform rectTransform = QuestPanelList.transform.GetChild(i).GetComponent<RectTransform>();
            
            if (rectTransform != newPanel)
            {

                rectTransform.DOAnchorPos3D(new Vector3(rectTransform.anchoredPosition3D.x + 400, rectTransform.anchoredPosition3D.y, rectTransform.anchoredPosition3D.z), 0.5f)
                    .OnComplete(() =>
                    {
                        movedPanels++;

                        if (movedPanels == 4)
                        {

                           // Debug.Log("delete : " + delete + " | " + QuestPanelList.transform.GetChild(delete).GetChild(0).GetComponent<TextMeshProUGUI>().text);
                            Destroy(QuestPanelList.transform.GetChild(delete).gameObject);
                            if (delete > 0) delete--;

                            isTweening = false;
                        }
                    });
            }
        }
        if(countNext < 4) countNext++;
       // Debug.Log("countNext : " + countNext);
    }

    public void UpdateSongInfoMovePre()
    {
        if (isTweening) return;
        if (isFirst) delete = 0; isFirst = false;

        // �̵� ������ ǥ��
        isTweening = true;

        int movedPanels = 0;

        //delete ���� �� ����
      
        switch (countNext)
        {
            case 4 :
                if (delete > 0)delete--;
               // Debug.Log("case 4, delete : " + delete);
                break;
            case 3:
                if (delete == 4) delete = 3;
                else if(delete == 3) delete = 2;
                else { delete = 0; if(!isRight) countNext = 0; }
              //  Debug.Log("case 3, delete : " + delete);
                break;
            case 2:
                if (delete != 3) delete = 3;
                else { delete = 0; countNext = 0; }
              //  Debug.Log("case 2, delete : " + delete);
                break;
            case 1: 
                delete = 0; 
                break;
            case 0:
                delete = 0;
                break;

        }


        isLeft = true;
        if (isRight) //������ right �̵��� �־��ٸ�
        {
            delete = 4;
            isRight = false;
            
        }
        // ���ο� �г� ����
        GameObject newPanel = Instantiate(panelPrefab, questPanelPosition[4].position, Quaternion.identity, QuestPanelList.transform);

        //���ο� �гο� ���� �ֱ�
        for (int i = 0; i < 3; i++)
        {
            count++;
            if (count >= songData.songs.Length) count = 0;
            else if (count < 0) count = songData.songs.Length + count;
        }
        //Debug.Log("count : " + songData.songs[count].title);// QuestPanelList.transform.GetChild(count).GetChild(0).GetComponent<TextMeshProUGUI>().text);

        newPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = songData.songs[count].title;
        newPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = songData.songs[count].artist;
        Sprite coverImage = LoadSpriteFromPath(songData.songs[count].cover_image_path);

        newPanel.transform.GetChild(2).GetComponent<Image>().sprite = coverImage;
        // ���� �ٲٱ�

        for (int i = 0; i < 2; i++)
        {
            count--;
            if (count < 0) count = songData.songs.Length + count;
            //else if (count >= musicList.Count) count = musicList.Count - count;
        }
        AudioClip musicClip = Resources.Load<AudioClip>(songData.songs[count].audio_file_path);
        songAudio.clip = musicClip;
        songAudio.Play();
        Debug.Log("songData.songs[count].title : " + songData.songs[count].title);

        Debug.Log("audio_file_path : " + songData.songs[count].audio_file_path);


        //�г� �̵�
        for (int i = 0; i <= 4; i++)
        {

            RectTransform rectTransform = QuestPanelList.transform.GetChild(i).GetComponent<RectTransform>();

            if (rectTransform != newPanel)
            {

                rectTransform.DOAnchorPos3D(new Vector3(rectTransform.anchoredPosition3D.x - 400, rectTransform.anchoredPosition3D.y, rectTransform.anchoredPosition3D.z), 0.5f)
                    .OnComplete(() =>
                    {
                        movedPanels++;

                        if (movedPanels == 4)
                        {
                          //  Debug.Log("delete : " + delete + " | " + QuestPanelList.transform.GetChild(delete).GetChild(0).GetComponent<TextMeshProUGUI>().text);
                            
                            Destroy(QuestPanelList.transform.GetChild(delete).gameObject);
                            isTweening = false;
                        }
                    });
            }
        }
    }


    //---------------------------------------

    public void UpdateSongInfoNext()
    {
        //questPanel[0].GetComponent<RectTransform>().DORotateQuaternion(questPanel[1].rotation, 1f);
        // questPanel[0].GetComponent<RectTransform>().DOAnchorPos3D(questPanel[1].anchoredPosition3D, 1f);
        // Instantiate(panelPrefab, questPanelPosition[0].position, Quaternion.identity, QuestPanelList.transform);
        int i;
        count--;
        if (count < 0) count = musicList.Count - 1;

        for (i = 0; i < 5; i++)
        {
            Debug.Log("i" + i + "position : " + position);

            if (position == 4) //position : �ű�� �� ��ġ
            {
                position = 0;
                Debug.Log("i : " + i);
                //QuestPanelList.transform.GetChild(i).transform.position = questPanelPosition[0].position;
                //QuestPanelList.transform.GetChild(i).transform.rotation = questPanelPosition[0].rotation;
                QuestPanelList.transform.GetChild(i).GetComponent<RectTransform>().position = questPanelPosition[position].position;
                QuestPanelList.transform.GetChild(i).GetComponent<RectTransform>().rotation = questPanelPosition[position].rotation;

                //  Debug.Log("questPanelPosition[0].position" + questPanelPosition[0].position);
                // Debug.Log("QuestPanelList.transform.GetChild(" + i + ").transform.position : " + QuestPanelList.transform.GetChild(i).transform.position);


                if (i == 2)
                {
                    songAudio.clip = musicList[count].audioClip;
                    songAudio.Play();
                }
            }
            else
            {
                QuestPanelList.transform.GetChild(i).GetComponent<RectTransform>().DORotateQuaternion(questPanelPosition[++position].rotation, 1f);
                QuestPanelList.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos3D(questPanelPosition[position].anchoredPosition3D, 1f);
                if (i == 2)
                {
                    songAudio.clip = musicList[count].audioClip;
                    songAudio.Play();
                }
            }
            // if (move >= 5) move = 0;
            // Debug.Log("after position : " + position);

        }
        if (position >= 4) position = 0;
        else position++;
        // Debug.Log("* : " + position);
        Debug.Log("***********************");


        // move++;
        // position -= 3;

        // questPanel[1].GetComponent<RectTransform>().DORotateQuaternion(questPanel[2].rotation, 1f);
        // questPanel[1].GetComponent<RectTransform>().DOAnchorPos3D(questPanel[2].anchoredPosition3D, 1f);


        // NewUpdateSongInfo(1);
        // Destroy(QuestPanelList.transform.GetChild(0));
        //  UpdateSongInfo();
    }

    /*
    public void UpdateSongInfoMoveNext()
    {
        //���ο�� ����
        Instantiate(panelPrefab, questPanelPosition[0].position, Quaternion.identity, QuestPanelList.transform);
        //�����Ѱſ� ���� �־��ֱ�
        //���� �ٲٱ�
        count--;
        if (count < 0) count = musicList.Count - 1;
        //������ �ϳ��� �б�
        for (int i = 0; i < 5; i++)
        {
            RectTransform changePosition = QuestPanelList.transform.GetChild(i).GetComponent<RectTransform>();
            changePosition.position = new Vector3(changePosition.position.x + 400, changePosition.position.y, changePosition.position.z);

            RectTransform rectTransform = QuestPanelList.transform.GetChild(i).GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                // DOTWEEN�� ����Ͽ� �ִϸ��̼� ����
                rectTransform.DORotateQuaternion(changePosition.rotation, 1f);
                rectTransform.DOAnchorPos3D(changePosition.anchoredPosition3D, 1f);
                // RectTransform�� ��ġ�� ������ �ʿ� ����
            }
            else
            {
                Debug.LogError("RectTransform is null or destroyed.");
            }
        }
        //��������
        // rightDelete = leftDelete;
        //����
        Debug.Log(QuestPanelList.transform.GetChild(rightDelete).name);
        Destroy(QuestPanelList.transform.GetChild(rightDelete).gameObject);
        //���ο� ���� ����
//        if (rightDelete >= 5) rightDelete = 0;
  //      else rightDelete--;

    }
    */

    public void UpdateSongInfoPre()
    {
        // questPanel[1].GetComponent<RectTransform>().DORotateQuaternion(questPanel[0].rotation, 1f);
        //  questPanel[1].GetComponent<RectTransform>().DOAnchorPos3D(questPanel[0].anchoredPosition3D, 1f);

        // questPanel[2].GetComponent<RectTransform>().DORotateQuaternion(questPanel[1].rotation, 1f);
        // questPanel[2].GetComponent<RectTransform>().DOAnchorPos3D(questPanel[1].anchoredPosition3D, 1f);
        // Instantiate(panelPrefab, questPanelPosition[4].position, Quaternion.identity, QuestPanelList.transform);


        for (int i = 4; i >= 0; i--)
        {
            // i += move;
            Debug.Log("before position : " + position);


            if (position == 0)
            {
                position = 4;
                QuestPanelList.transform.GetChild(i).transform.position = questPanelPosition[position].position;
                QuestPanelList.transform.GetChild(i).transform.rotation = questPanelPosition[position].rotation;
            }
            else
            {
                QuestPanelList.transform.GetChild(i).GetComponent<RectTransform>().DORotateQuaternion(questPanelPosition[--position].rotation, 1f);
                QuestPanelList.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos3D(questPanelPosition[position].anchoredPosition3D, 1f);
            }
            // if (move >= 5) move = 0;
            Debug.Log("after position : " + position);

        }
        if (position == 0) position--;
        Debug.Log("* : " + position);
        Debug.Log("***********************");
        /*
        position += 2;

        for (int i = 3; i >= 0; i--)
        {
            Debug.Log("position : " + position);
            QuestPanelList.transform.GetChild(i).GetComponent<RectTransform>().DORotateQuaternion(questPanelPosition[--position].rotation, 1f);
            QuestPanelList.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos3D(questPanelPosition[position].anchoredPosition3D, 1f);
        }
        count++;*/
        //NewUpdateSongInfo(-1);
        // Destroy(QuestPanelList.transform.GetChild(QuestPanelList.transform.childCount - 1));
        count++;
        if (count > musicList.Count) count = 0;
        
      //    UpdateSongInfo();

    }
}
    
    