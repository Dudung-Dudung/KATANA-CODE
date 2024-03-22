using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MusicManager;

public class MainMenuManager : MonoBehaviour
{
    public GameObject songSelection; //껐다 켰다 할 main ui
    public Image ARASAKAFillAmount; //동그라미 퍼센트 보여주기 용
    public TextMeshProUGUI ARASAKAPercent; //동그라미 숫자
    
    public int ARASAKAClearPercentage = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ARASAKAPercentage();
    }

    public void StagePicker()
    {
        songSelection.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ARASAKAPercentage()
    {
        ARASAKAFillAmount.fillAmount = ARASAKAClearPercentage / 100f;
        
        ARASAKAPercent.text = ARASAKAClearPercentage.ToString() + " %";
    }
}
