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
    [Tooltip("출력 타입")]
    public Ease easeB;

    [Header("After letters")]
    public string after;
    public float exeTimeAfter;
    [Tooltip("출력 타입")]
    public Ease easeA;

    [Header("도달 후 글자 쓰는 텀")]
    public float delay;

    [Header("이전 페이드")]
    public GameObject beforeFade;

    [Header("이후 페이드")]
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
            Debug.Log("스크립트 참조 실패");
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
        // 변수 초기화 이유 : 값들을 더해야 하는데 더미가 있으면 안됨
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
