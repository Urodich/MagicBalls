using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HUD_script : MonoBehaviour
{
    public GameObject DialogePanel,BoostPanel,PauseMenu,Inventory,StatsText, FrontImage;
    [SerializeField] SettingsScript Settings;
    Spells_script spells;
    public UnityAction resume,restart,exit;
    void Start()
    {
        spells = GameObject.FindWithTag("Player").GetComponent<Spells_script>();
        FrontImage.SetActive(false);
        Settings.Start();
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
