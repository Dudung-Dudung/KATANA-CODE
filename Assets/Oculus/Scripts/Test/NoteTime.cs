using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NoteTime : MonoBehaviour
{
    public AudioSource audioSource;
    private AudioClip audioClip;
    private string filePath;
    private StreamWriter writer;

    bool musicStart = false;
    float startTime;
    int count = 0;
    int totallines = 0;
    List<string> lines = new List<string>();
    bool exist = false;
    // Start is called before the first frame update
    void Start()
    {
        audioClip = audioSource.clip;
        Debug.Log(audioClip.name);
        filePath = "Assets/Notes/NoteTime/" + audioClip.name + ".csv";
        if (File.Exists(filePath))
        {
            lines.AddRange(File.ReadAllLines(filePath));
            totallines = lines.Count;
            Debug.Log("���� ���� : " +totallines);
            exist = true;
        }
        writer = new StreamWriter(filePath, true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!musicStart)
        {
            if (collision.CompareTag("Note"))
            {
                musicStart = true;
                startTime = Time.realtimeSinceStartup;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && musicStart)
        {
            float noteTime = Time.realtimeSinceStartup - startTime;
            if (count >= totallines)
            {
                lines.Add(0 + "," + (count + 1) + "," + noteTime.ToString("N2"));
            }
            else
            {
                lines[count] += "," + noteTime.ToString("N2");
            }
            Debug.Log(count);
            Debug.Log(noteTime.ToString("N2") + "ms");
            Debug.Log(Time.realtimeSinceStartup);
            count++;
        }
    }

    void OnApplicationQuit()
    {

        if (writer != null)
        {
            writer.Close();
        }
        if (exist)
        {
            File.WriteAllLines(filePath, lines);
        }
    }
}
