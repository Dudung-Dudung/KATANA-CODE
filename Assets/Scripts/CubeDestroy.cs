using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Cube_lt" || other.gameObject.tag == "Cube_rt")
        {
            Destroy(other.gameObject);
            NoteManager.songMissNoteCount += 1;
            Debug.Log(NoteManager.songMissNoteCount + " ≥ı√∆Ω¿¥œ¥Ÿ");
        }
    }
}
