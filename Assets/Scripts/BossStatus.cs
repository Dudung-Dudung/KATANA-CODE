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

    public Material damageMaterial; 
    private Material originalMaterial; 


    void Start()
    {
        Debug.Log(NoteManager.songNoteCount + " 현재 노트 갯수");
        bossHp = NoteManager.songNoteCount * 70 / 100;
        startHp = NoteManager.songNoteCount * 70 / 100; ;
        originalMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Bullet")){
            bossHp--;
            UpdateHPBar();
            StartCoroutine(DamageEffect());
        }

        if (bossHp < 0) isClear = true;
    }

    void UpdateHPBar()
    {
        hpSlider.value = 1 - (float)bossHp / startHp;
    }

    IEnumerator DamageEffect()
    {
        GetComponent<Renderer>().material = damageMaterial;
        yield return new WaitForSeconds(0.2f);
        GetComponent<Renderer>().material = originalMaterial;
    }
}
