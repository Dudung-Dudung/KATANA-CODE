using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    /*double currentTime = 0d;*/

    [SerializeField] Transform LeftNoteAppearLocation = null;
    [SerializeField] Transform RightNoteAppearLocation = null;
    [SerializeField] GameObject singleNotePrefab = null; // 프리팹 설정
    [SerializeField] GameObject doubleNotePrefab = null; // 프리팹 설정
    /*    [SerializeField] GameObject cubePrefab;*/

    public GameObject gameClearBtn;
    public GameObject gameOverBtn;

    /*    public Text gameClearBtnText;
        public Text gameOverBtnText;*/

    public GameObject sceneMover;

    public bool isGameOver = false; //게임 시간 지나고 ui 중복되는거 막기 위한 불리언 변수

    public string notesJsonPath = "Assets/Notes/stylish-rock-beat-trailer-ComaStudio.json"; // JSON 파일의 경로
    public string musicDataJsonPath = "Assets/Resources/MusicData.json"; // JSON 파일의 경로

    [SerializeField] Vector3[] cubePositions;

    TimingManager timingManager;
    CubeGenerator cubeGenerator;

    [SerializeField]
    float songLength = 0;

    [SerializeField] // 전체 노트 갯수
    public static int songNoteCount;

    [SerializeField] // 맞춘 노트 갯수
    public static int songHitNoteCount;

    public static int bonusScore; // 보너스 점수 

    [SerializeField]
    float score = 0f;

    [SerializeField]
    string rank;
    [SerializeField]
    float percentage;



    private float runningTime = 0f;
    private bool isRunning = false;

    private void Awake()
    {
        timingManager = GetComponent<TimingManager>();
        cubeGenerator = FindObjectOfType<CubeGenerator>();

        LoadNotes();
    }

    private void Start()
    {
        Debug.Log(runningTime);
        StartCoroutine(StartTimer(runningTime));
    }

    private void Update()
    {
/*        if ((songMissNoteCount >= songNoteCount / 2) && !isGameOver)
        {
            GameOver();
        }*/
    }

    void LoadNotes()
    {
        string json = System.IO.File.ReadAllText(notesJsonPath);
        NoteList noteList = JsonUtility.FromJson<NoteList>(json);

        runningTime = noteList.length;
        songNoteCount = noteList.noteCount;
        /*        songMissNoteCount = noteList.hitNoteCount;*/
        songHitNoteCount = 0;
        Debug.Log(songNoteCount);
        Debug.Log(songHitNoteCount);


        foreach (NoteData noteData in noteList.notes)
        {
            StartCoroutine(CreateNoteDelayed(noteData.time, noteData.type, noteData.pos));
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

        // 타입에 따라 다른 프리팹 사용
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

    //게임 클리어, 오버시 띄우는 ui용

    public void GameClear()
    {
        //여기 변경해서 점수 배율 조정 가능
        score = StickController.bonusScore;
        Debug.Log(score + " 최종 점수, 현재 노래의 score에 반영될거임");

        percentage = songHitNoteCount / songNoteCount * 100f;
        CalculateRank();

        SongScoreManager songScoreManager = FindObjectOfType<SongScoreManager>();
        if (songScoreManager != null)
        {
            foreach (SongData song in songScoreManager.songs)
            {
                if (song.title == "titanium-AlisiaBeats")
                {
                    songScoreManager.UpdateSongState(song.title, score, rank);
                    break; 
                }
            }
        }
    }

    IEnumerator StartTimer(float duration)
    {
        isRunning = true;
        runningTime = 0f;

        while (runningTime < duration)
        {
            yield return null; // 한 프레임 대기
            runningTime += Time.deltaTime; // 경과 시간 업데이트
        }

        // 타임아웃 발생
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

            // Button 컴포넌트로부터 Text 컴포넌트를 가져옴
            TextMeshProUGUI tmpText = buttonComponent.GetComponentInChildren<TextMeshProUGUI>();

            // 텍스트 변경
            tmpText.text = "Game Clear! " + "score : " + ((int)score).ToString() +" rank : "+ rank.ToString();
        }
    }

/*    public void GameOver()
    {
        isGameOver = true;
        *//* gameOverBtn.SetActive(true);*//*
        GameClear();

        Button buttonComponent = gameOverBtn.GetComponent<Button>();

        // Button 컴포넌트로부터 Text 컴포넌트를 가져옴

        TextMeshProUGUI tmpText = buttonComponent.GetComponentInChildren<TextMeshProUGUI>();

        // 텍스트 변경
        tmpText.text = "Game Over.. " + score.ToString();
    }*/

    public void SetMainScene()
    {
        SceneManager.LoadScene("TestMainScene");
    }

    public void SceneMove()
    {
        sceneMover.GetComponent<Fade>().StartFade();
        Invoke("SetMainScene", 2f);
    }

    //점수 계산용
    void CalculateRank()
    {

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
        }

        else if (percentage >= 70)
        {
            rank = "B";
        }

        else
        {
            rank = "F";
        }

        Debug.Log("랭크: " + rank);
    }

}