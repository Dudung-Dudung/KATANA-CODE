using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Fade : MonoBehaviour
{

    public float fadeInTime = 1.0f;
    public float fadeOutTime = 1.0f;
    public float fadeDurationTime = 1.0f;

    public enum FadeType
    {
        fadeIn,
        fadeOut,
        fadeInOut,
        fadeOutIn
    }

    public FadeType fadeType;

    private CanvasGroup fade;

    [Header("ȭ�� ��ȯ")]
    public GameObject beforeObj;
    public GameObject nextObj;
    private void Awake()
    {
        fade = this.GetComponent<CanvasGroup>();  
    }
    public void StartFade()
    {
        StartCoroutine(SetFade(fadeType));
    }

    IEnumerator SetFade(FadeType type)
    {
        switch (type)
        {
            case FadeType.fadeIn:
                Tween tweenIn = fade.DOFade(0.0f, fadeInTime);
                yield return tweenIn.WaitForCompletion();
                fade.gameObject.SetActive(false);      
                break;

            case FadeType.fadeOut:
                Tween tweenOut = fade.DOFade(1.0f, fadeOutTime);
                yield return tweenOut.WaitForCompletion();
                if (beforeObj != null)
                {
                    beforeObj.gameObject.SetActive(false);
                }
                if (nextObj != null)
                {
                    nextObj.gameObject.SetActive(true);
                }
                break;

            case FadeType.fadeInOut:
                StartCoroutine(SetFade(FadeType.fadeIn));
                yield return new WaitForSeconds(fadeDurationTime + fadeInTime);
                StartCoroutine(SetFade(FadeType.fadeOut));
                break;

            case FadeType.fadeOutIn:
                StartCoroutine(SetFade(FadeType.fadeOut));
                yield return new WaitForSeconds(fadeDurationTime + fadeOutTime);
                StartCoroutine(SetFade(FadeType.fadeIn));
                break;
        }
    }
}
