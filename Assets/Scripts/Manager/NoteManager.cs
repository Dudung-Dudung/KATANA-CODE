using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


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
}

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    /*double currentTime = 0d;*/

    [SerializeField] Transform LeftNoteAppearLocation = null;
    [SerializeField] Transform RightNoteAppearLocation = null;
    [SerializeField] GameObject singleNotePrefab = null; // ������ ����
    [SerializeField] GameObject doubleNotePrefab = null; // ������ ����
    [SerializeField] GameObject cubePrefab;

    public string notesJsonPath = "Assets/Notes/notes.json"; // JSON ������ ���
    public string musicDataJsonPath = "Assets/Resources/MusicData.json"; // JSON ������ ���

    [SerializeField] Vector3[] cubePositions;

    TimingManager timingManager;
    CubeGenerator cubeGenerator;

    [SerializeField]
    float songLength = 0;

    [SerializeField] // ��ü ��Ʈ ����
    public static int songNoteCount;

    [SerializeField] // ���� ��Ʈ ����
    public static int songMissNoteCount;

    [SerializeField]
    float score = 0f;

    private void Awake()
    {
        timingManager = GetComponent<TimingManager>();
        cubeGenerator = FindObjectOfType<CubeGenerator>();

        LoadNotes();
    }

    void LoadNotes()
    {
        string json = System.IO.File.ReadAllText(notesJsonPath);
        NoteList noteList = JsonUtility.FromJson<NoteList>(json);

        songLength = noteList.length;
        songNoteCount = noteList.noteCount;
        songMissNoteCount = noteList.hitNoteCount;
        Debug.Log(songNoteCount);
        Debug.Log(songMissNoteCount);


        foreach (NoteData noteData in noteList.notes)
        {
            StartCoroutine(CreateNoteDelayed(noteData.time, noteData.type, noteData.pos));
        }
    }

    IEnumerator CreateNoteDelayed(float time, string type, int pos)
    {
        yield return new WaitForSeconds(time);

        CreateNote(type,pos);
    }

    void CreateNote(string type, int pos)
    {
        GameObject t_note = null;

        // Ÿ�Կ� ���� �ٸ� ������ ���
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

    //���� Ŭ����, ������ ���� ui��

    public void GameClear()
    {
        score = ((float)songNoteCount - songMissNoteCount) / songNoteCount * 100f;
        Debug.Log(score);

        // SongScoreManager�� ã�ų� �����մϴ�.
        SongScoreManager songScoreManager = FindObjectOfType<SongScoreManager>();
        if (songScoreManager != null)
        {
            // �� ����� ��ȸ�Ͽ� �ش� ���� ã�� ������ �����մϴ�.
            foreach (SongData song in songScoreManager.songs)
            {
                if (song.title == "Do You Want To Build A Snowman")
                {
                    songScoreManager.UpdateScore(song.title, score);
                    break; // ���ϴ� ���� ã�����Ƿ� ���� ����
                }
            }
        }
    }




    public void GameOver()
    {

    }

}
