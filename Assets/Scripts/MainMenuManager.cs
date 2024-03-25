using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MusicManager;

public class MainMenuManager : MonoBehaviour
{
    public GameObject songSelection; //���� �״� �� main ui
    public Image ARASAKAFillAmount; //���׶�� �ۼ�Ʈ �����ֱ� ��
    public TextMeshProUGUI ARASAKAPercent; //���׶�� ����
    
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
