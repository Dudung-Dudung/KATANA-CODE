using UnityEngine;

public class RightNote : BaseNote
{
    protected override void Start()
    {
        base.Start();
        direction = NoteDirection.Right;
    }


    protected override void UpdateMovement()
    {
        // 오른쪽으로 이동
        transform.localPosition += Vector3.left * noteSpeed * Time.deltaTime;
    }

    protected override bool IsNoteDirection(NoteDirection checkDirection)
    {
        return checkDirection == NoteDirection.Right;
    }
}
