using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class MainGague : CircleGague
{
    [Header("Status letters")]
    public Text status;
    [Header("Before letters")]
    public string before;
    public float exeTimeBefore;
    [Tooltip("��� Ÿ��")]
    public Ease easeB;

    [Header("After letters")]
    public string after;
    public float exeTimeAfter;
    [Tooltip("��� Ÿ��")]
    public Ease easeA;

    [Header("���� �� ���� ���� ��")]
    public float delay;

    [Header("���� ���̵�")]
    public GameObject beforeFade;

    [Header("���� ���̵�")]
    public GameObject afterFade;

    private CircleGagueManager CG = null;
    private float totalValue;
    private bool isReach = false;
    private bool isStart = false;
    private bool CanStart = false;

    private Fade fadeTime;
    // Start is called before the first frame update

    private void Awake()
    {
        CG = transform.parent.GetComponent<CircleGagueManager>();
        if(CG == null)
        {
            Debug.Log("��ũ��Ʈ ���� ����");
        }
        fadeTime = beforeFade.GetComponent<Fade>();
    }
    public override void Start()
    {
        Invoke("StartMainGague",fadeTime.fadeDurationTime + fadeTime.fadeInTime + 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // ���� �ʱ�ȭ ���� : ������ ���ؾ� �ϴµ� ���̰� ������ �ȵ�
        totalValue = 0.0f;
        foreach(GameObject circleValue in CG.CircleGagues)
        {
            totalValue += circleValue.GetComponent<CircleGague>().Fill.fillAmount;
        }

        totalValue = totalValue / CG.CircleGagues.Length;

        Fill.fillAmount = totalValue;

        if(totalValue >= 0.0f && totalValue < 1.0f)
        {
            if (isStart)
            {
                Persent.text = string.Format("{0:F0}%", totalValue * 100);
            }
        }
        else 
        {
            if (!isReach)
            {
                isReach = true;
                StartCoroutine("DelayWrite");
            }
        }
    }

    IEnumerator DelayWrite()
    {
        yield return new WaitForSeconds(delay);
        status.text = "";
        Persent.text = "";
        status.DOText(after, exeTimeAfter).OnComplete(()=> afterFade.GetComponent<Fade>().StartFade());
    }

    public void StartMainGague()
    {
        status.DOText(before, exeTimeBefore).OnComplete(() => ShowPersent());
    }

    public void ShowPersent()
    {
        CG.StartCoroutine("SpawnGague");
        isStart = true;
    }
}
