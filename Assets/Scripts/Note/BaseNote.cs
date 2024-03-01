using UnityEngine;
using UnityEngine.UI;

public class BaseNote : MonoBehaviour
{

    TimingManager timingManager;
    public float noteSpeed = 400f;

    protected Image noteImage;

    public enum NoteDirection
    {
        Left,
        Right
    }

    public NoteDirection direction; 

    protected virtual void Start()
    {
        timingManager = GetComponent<TimingManager>();
        noteImage = GetComponent<Image>();
    }

    public void HideNote()
    {
        noteImage.enabled = false;
    }

    protected virtual void UpdateMovement()
    {
        transform.localPosition += Vector3.right * noteSpeed * Time.deltaTime;
    }

    protected virtual void Update()
    {
        UpdateMovement();
    }

    protected virtual bool IsNoteDirection(NoteDirection checkDirection)
    {
        return direction == checkDirection;
    }
}
