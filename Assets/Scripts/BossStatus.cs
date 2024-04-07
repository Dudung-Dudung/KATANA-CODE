using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStatus : MonoBehaviour
{
    public static bool isClear = false;
    public int bossHp;
    public int startHp;
    public Slider hpSlider;


    void Start()
    {
        Debug.Log(NoteManager.songNoteCount + " 현재 노트 갯수");
        bossHp = NoteManager.songNoteCount * 70 / 100;
        startHp = NoteManager.songNoteCount * 70 / 100; ;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Bullet")){
            bossHp--;
            UpdateHPBar();
        }

        if (bossHp < 0) isClear = true;
    }

    void UpdateHPBar()
    {
        hpSlider.value = 1 - (float)bossHp / startHp;
    }
}
