using UnityEngine;

public class LeftNote : BaseNote
{
    protected override void Start()
    {
        base.Start();
        direction = NoteDirection.Left;
    }
    protected override void UpdateMovement()
    {
        // 왼쪽으로 이동
        transform.localPosition += Vector3.right * noteSpeed * Time.deltaTime;
    }

    protected override bool IsNoteDirection(NoteDirection checkDirection)
    {
        return checkDirection == NoteDirection.Left;
    }
}
