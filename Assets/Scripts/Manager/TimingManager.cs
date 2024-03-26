using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();

    [SerializeField] Transform center = null;
    [SerializeField] RectTransform[] timingRect = null;
    Vector2[] timingBoxs = null;

    void Start()
    {
        timingBoxs = new Vector2[timingRect.Length];

        for (int i = 0; i < timingRect.Length; i++)
        {
            timingBoxs[i].Set(center.localPosition.x - timingRect[i].rect.width / 2, center.localPosition.x + timingRect[i].rect.width / 2);
        }
    }

    public void CheckTiming()
    {
        for (int i = 0; i < boxNoteList.Count; i++)
        {
            GameObject t_note = boxNoteList[i];
            float t_notePosX = t_note.transform.localPosition.x;

            for (int x = 0; x < timingBoxs.Length; x++)
            {
                if (timingBoxs[x].x <= t_notePosX && t_notePosX <= timingBoxs[x].y)
                {
                    BaseNote note = t_note.GetComponent<BaseNote>();

                    if (note != null)
                    {
/*                        if (note.direction == BaseNote.NoteDirection.Left)
                        {
                            // Left 노트와 상호작용하는 코드
                            Debug.Log("Left Hit");
                            boxNoteList.RemoveAt(i);
                            Destroy(t_note);
                        }
                        else if (note.direction == BaseNote.NoteDirection.Right)
                        {
                            // Right 노트와 상호작용하는 코드
                            Debug.Log("Right Hit");
                            boxNoteList.RemoveAt(i);
                            Destroy(t_note);
                        }*/

                        Debug.Log("Hit!!!!");
                        Debug.Log(Time.realtimeSinceStartup);
                        boxNoteList.RemoveAt(i);
                        Destroy(t_note);
                    }

                    return;
                }
            }
        }
    }
}