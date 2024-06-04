using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using static MusicManager;
using UnityEngine.Networking;

[System.Serializable]
public class NoteData
{
    public float time;
    public string type;
    public int pos;
}

[System.Serializable]
public class NoteList
{
    public List<NoteData> notes;
    public float length;
    public int noteCount;
    public int hitNoteCount;
    public string rank;
}

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;

    [SerializeField] Transform LeftNoteAppearLocation = null;
    [SerializeField] Transform RightNoteAppearLocation = null;
    [SerializeField] GameObject singleNotePrefab = null;
    [SerializeField] GameObject doubleNotePrefab = null;

    public GameObject gameClearBtn;
    public GameObject gameOverBtn;

    public TextMeshProUGUI rankUI;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI ProgressUI;

    public GameObject sceneMover;

    public bool isGameOver = false;

    public string notesJsonPath;
    public string musicDataJsonPath;

    [SerializeField] Vector3[] cubePositions;

    TimingManager timingManager;
    CubeGenerator cubeGenerator;

    [SerializeField] float songLength = 0;

    [SerializeField] public static float songNoteCount;

    [SerializeField] public static float songHitNoteCount;

    public static int bonusScore;

    [SerializeField] float score = 0f;

    [SerializeField] string rank;
    [SerializeField] int percentage;

    private float runningTime = 0f;
    private bool isRunning = false;

    private void Awake()
    {
        timingManager = GetComponent<TimingManager>();
        cubeGenerator = FindObjectOfType<CubeGenerator>();
        CopyJsonFromStreamingAssetsToPersistentDataPath("Stylish Rock Beat Trailer.json");
        CopyJsonFromStreamingAssetsToPersistentDataPath("MusicData.json");

        // Android에서 JSON 파일 경로 설정
        notesJsonPath = Path.Combine(Application.persistentDataPath, "Stylish Rock Beat Trailer.json");
        musicDataJsonPath = Path.Combine(Application.persistentDataPath, "MusicData.json");

        

        Debug.Log(GameManager.songTitle + " 현재 게임매니저에서 넘어온 값");
        if (GameManager.songTitle != null)
        {
            notesJsonPath = Path.Combine(Application.persistentDataPath, "Stylish Rock Beat Trailer.json");
        }

        Debug.Log(notesJsonPath + "json 파일 경로 - NoteManager.cs");
        LoadNotes();
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


    private void Start()
    {
        Debug.Log(runningTime + "곡 길이");
        StartCoroutine(StartTimer(runningTime));
    }

    private void Update()
    {
    }

    void LoadNotes()
    {
        if (File.Exists(notesJsonPath))
        {
            string json = File.ReadAllText(notesJsonPath);
            NoteList noteList = JsonUtility.FromJson<NoteList>(json);

            runningTime = noteList.length;
            songNoteCount = noteList.noteCount;
            songHitNoteCount = 0;
            Debug.Log(songNoteCount);
            Debug.Log(songHitNoteCount + "맞춘 노트 갯수");

            foreach (NoteData noteData in noteList.notes)
            {
                StartCoroutine(CreateNoteDelayed(noteData.time, noteData.type, noteData.pos));
            }
        }
        else
        {
            Debug.LogError("노트 JSON 파일을 찾을 수 없습니다: " + notesJsonPath);
        }
    }

    IEnumerator CreateNoteDelayed(float time, string type, int pos)
    {
        yield return new WaitForSeconds(time);
        CreateNote(type, pos);
    }

    void CreateNote(string type, int pos)
    {
        GameObject t_note = null;

        if (type == "lt")
        {
            t_note = Instantiate(singleNotePrefab, LeftNoteAppearLocation.position, Quaternion.identity);
            cubeGenerator.Create_Cube_Lt(cubePositions[pos]);
        }
        else if (type == "rt")
        {
            t_note = Instantiate(doubleNotePrefab, RightNoteAppearLocation.position, Quaternion.identity);
            cubeGenerator.Create_Cube_Rt(cubePositions[pos]);
        }

        if (t_note != null)
        {
            t_note.transform.SetParent(this.transform);
            timingManager.boxNoteList.Add(t_note);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            timingManager.boxNoteList.Remove(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }

   

    public void GameClear()
    {
        percentage = (int)((int)(BossStatus.bossclearhitcount + BossStatus.bosshitcount) / songNoteCount * 100f);
        Debug.Log("클리어 결과 : " + BossStatus.bosshitcount + " " + BossStatus.bossclearhitcount + " " + songNoteCount);

        Debug.Log("퍼센티지 : " + percentage);

        score = BossStatus.bossclearhitcount * 4;
        Debug.Log(score + " 최종 점수, 현재 노래의 score에 반영될거임");

        CalculateRank();

        SongScoreManager songScoreManager = FindObjectOfType<SongScoreManager>();
        if (songScoreManager != null)
        {
            foreach (SongData song in songScoreManager.songs)
            {
                if (song.title == "Stylish Rock Beat Trailer")
                {
                    songScoreManager.UpdateSongState(song.title, score, rank, percentage);
                    Debug.Log("점수 수정 반영됬음 - NoteManager");
                    Debug.Log(Resources.Load<TextAsset>("MusicData"));

                    break;
                }
                else
                {
                    Debug.Log(GameManager.songTitle + "곡 제목 수정해라");
                }
            }
        }

        Invoke("SetMainScene", 2f);
    }

    IEnumerator StartTimer(float duration)
    {
        isRunning = true;
        runningTime = 0f;

        while (runningTime < duration)
        {
            yield return null;
            runningTime += Time.deltaTime;
        }

        Debug.Log("타임아웃");
        GameFinish();
        isRunning = false;
    }

    public void GameFinish()
    {
        if (!isGameOver)
        {
            gameClearBtn.SetActive(true);
            GameClear();

            Button buttonComponent = gameClearBtn.GetComponent<Button>();
            TextMeshProUGUI tmpText = buttonComponent.GetComponentInChildren<TextMeshProUGUI>();

            if (BossStatus.isClear)
            {
                scoreUI.text = ((int)score).ToString();
                rankUI.text = rank.ToString();
                ProgressUI.text = (percentage + "%").ToString();
                Debug.Log(percentage.ToString() + " 반영되는 퍼센티지");
            }
            else
            {
                scoreUI.text = ((int)score).ToString();
                rankUI.text = rank.ToString();
                ProgressUI.text = (percentage + "%").ToString();
            }
        }
    }

    public void SetMainScene()
    {
        SceneManager.LoadScene("TestMainScene");
    }

    public void SceneMove()
    {
        Debug.Log("게임 종료 후 씬 이동!");
        sceneMover.GetComponent<Fade>().StartFade();
        Invoke("SetMainScene", 2f);
    }

    void CalculateRank()
    {
        rankUI.color = new Color(35 / 255f, 248 / 255f, 248 / 255f);
        if (percentage >= 97)
        {
            rank = "SS";
        }
        else if (percentage >= 95)
        {
            rank = "S+";
        }
        else if (percentage >= 90)
        {
            rank = "S";
        }
        else if (percentage >= 80)
        {
            rank = "A";
            rankUI.color = new Color(57 / 255f, 174 / 255f, 174 / 255f);
        }
        else if (percentage >= 75)
        {
            rank = "B";
            rankUI.color = new Color(84 / 255f, 129 / 255f, 129 / 255f);
        }
        else if (percentage >= 70)
        {
            rank = "C";
            rankUI.color = new Color(135 / 255f, 135 / 255f, 135 / 255f);
        }
        else
        {
            rank = "F";
            rankUI.color = new Color(57 / 255f, 57 / 255f, 57 / 255f);
        }

        Debug.Log("랭크: " + rank);
    }
}
