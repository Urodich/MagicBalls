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
    static Dialoge_script activeDialogue=null;
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
        isActive=false;
        //Destroy(gameObject);
    }

    public void ResumeDialoge(){
        dialoge=StartCoroutine(Dialoge());
    }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.tag!="Player") return;
        if (isActive) return;
        if(activeDialogue!=null) return;//activeDialogue.PauseDialoge();
        activeDialogue=this;
        Debug.Log("activate dialogue");
        isActive=true;
        HUD_script hud=GameObject.Find("HUD(Clone)").GetComponent<HUD_script>();
        textPanel = hud.DialogePanel;
        textPanel.SetActive(true);
        text = textPanel.transform.Find("Dialoge Text").GetComponent<TextMeshProUGUI>();
        dialoge=StartCoroutine(Dialoge());
    }
}


