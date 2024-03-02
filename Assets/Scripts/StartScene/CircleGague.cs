using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CircleGague : MonoBehaviour
{
    [Header ("Fill")]
    public Image Fill;


    [Range(0,100)]
    public float value;

    [Header("Persent")]
    public Text Persent;

    [Header("StartToEnd")]
    [Range(0, 100)]
    public float startValue;
    [Range(0, 100)]
    public float endValue;
    public float ETA;

    private float currentValue;

    private void Awake()
    {
        Fill.fillAmount = startValue / 100.0f;
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        FillGague();
    }

    // Update is called once per frame
    void Update()
    {
        currentValue = Fill.fillAmount * 100;
        Persent.text = string.Format("{0:F0}%", currentValue);
    }

    public void FillGague()
    {
        DOTween.To(() => Fill.fillAmount, x => Fill.fillAmount = x, endValue / 100.0f, ETA);
    }
}
