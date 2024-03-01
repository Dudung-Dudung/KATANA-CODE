using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTimer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.CompareTag("Note"))
            {
                Debug.Log(Time.time); // 큐브 날아오는 속도 구하기용 
            }
    }
}
