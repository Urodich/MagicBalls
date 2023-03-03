using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Dialoge_script : MonoBehaviour
{
    GameObject textPanel;
    TextMeshProUGUI text;
    Coroutine dialoge;
    [SerializeField] List<DialogeLine> Lines=new List<DialogeLine>();
    IEnumerator Dialoge(){
        foreach(DialogeLine line in Lines){
            text.text=line.text;
            line.action.Invoke();
            yield return new WaitForSeconds(line.time); 
        }
        textPanel.SetActive(false);
        Destroy(this, 1f);
    }
    public void PauseDialoge(){
        StopCoroutine(dialoge);
    }

    public void ResumeDialoge(){
        dialoge=StartCoroutine(Dialoge());
    }

    void OnColliderEnter(Collider collider){
        HUD_script hud=GameObject.Find("HUD").GetComponent<HUD_script>();
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
}
