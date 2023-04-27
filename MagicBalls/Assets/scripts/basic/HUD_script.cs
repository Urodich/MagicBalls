using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HUD_script : MonoBehaviour
{
    public GameObject DialogePanel;
    public GameObject BoostPanel;
    public GameObject PauseMenu;
    public GameObject Inventory;
    public GameObject StatsText;
    Spells_script spells;
    public UnityAction resume;
    public UnityAction restart;
    public UnityAction exit;
    void Start()
    {
        spells = GameObject.FindWithTag("Player").GetComponent<Spells_script>();
    }

    public void Resume(){
        resume.Invoke();
    }
    public void Restart(){
        restart.Invoke();
    }
    public void Exit(){
        exit.Invoke();
    }
    public void GodMod(bool value){
        spells.GodMod=value;
    }

}
