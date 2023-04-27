using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Audio;

public class Dialoge_script : MonoBehaviour
{
    bool isActive = false;
    GameObject textPanel;
    TextMeshProUGUI text;
    Coroutine dialoge;
    AudioSource audio;
    void Start(){
        audio = GetComponent<AudioSource>();
    }
    [SerializeField] List<DialogeLine> Lines=new List<DialogeLine>();
    IEnumerator Dialoge(){
        foreach(DialogeLine line in Lines){
            text.text=line.text;
            if (line.clip!=null){
                audio.clip=line.clip;
                audio.Play();
            }
            line.action.Invoke();
            yield return new WaitForSeconds(line.time); 
        }
        textPanel.SetActive(false);
        Destroy(gameObject);
    }
    public void PauseDialoge(){
        StopCoroutine(dialoge);
    }

    public void ResumeDialoge(){
        dialoge=StartCoroutine(Dialoge());
    }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.tag!="Player") return;
        if (isActive) return;
        Debug.Log("activate dialogue");
        isActive=true;
        HUD_script hud=GameObject.Find("HUD(Clone)").GetComponent<HUD_script>();
        textPanel = hud.DialogePanel;
        textPanel.SetActive(true);
        text = textPanel.transform.Find("Dialoge Text").GetComponent<TextMeshProUGUI>();
        dialoge=StartCoroutine(Dialoge());
    }
}

[Serializable]
public struct DialogeLine{
    public string text;
    public int time;
    public UnityEvent action;
    public AudioClip clip;
}
