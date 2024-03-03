using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    private float distance; // ���� ��ġ�� ��ǥ ��ġ ������ �Ÿ�
    private float duration = 2.6f; // �̵��� �ɸ��� �ð�
    private float speed; // ������ �ӵ�

    private Vector3 startPosition; // �ʱ� ��ġ
    private Vector3 targetPosition; // ��ǥ ��ġ

    private bool isMoving = true; // �̵� ���θ� ��Ÿ���� ����

/*    void Start()
    {
        // ť���� �ʱ� ��ġ�� ����
        startPosition = new Vector3(0, 1.5f, 30);
        // ��ǥ ��ġ�� ����
        targetPosition = new Vector3(0, 1.5f, -7);
        transform.position = startPosition;

        // ���� ��ġ�� ��ǥ ��ġ ������ �Ÿ� ���
        distance = Vector3.Distance(startPosition, targetPosition);

        // ������ �ӵ� ���
        speed = distance / duration;
    }*/


    // ť���� ���� ��ġ�� ��ǥ ��ġ�� �����ϴ� �޼ҵ�
    public void SetCubeMovement(Vector3 startPosition, Vector3 targetPosition)
    {
        this.startPosition = startPosition;
        this.targetPosition = targetPosition;

        // ���� ��ġ�� ��ǥ ��ġ ������ �Ÿ� ���
        distance = Vector3.Distance(startPosition, targetPosition);

        // ������ �ӵ� ���
        speed = distance / duration;

        // ť���� ��ġ�� ���� ��ġ�� ����
        transform.position = startPosition;
    }

    void Update()
    {
        if (isMoving)
        {
            // ���� ��ġ���� ��ǥ ��ġ���� ������ �ӵ��� �̵�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
