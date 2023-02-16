using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public abstract class SpellBase 
{
    bool CD=false;
    [SerializeField] float dealy;
    float CoolDown;
    float ManaCost;
    GameObject player;
    player_script stats;
    Spells_script spells;
    playerControl_script control;
    NavMeshAgent navMesh;
    buffs_script buffs;
    GameObject spellPanel;

    static Coroutine currentCast;

    SpellBase(){
        spellPanel=GameObject.Find("SpellPanel");
        player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<player_script>();
        navMesh = player.GetComponent<NavMeshAgent>();
        buffs = player.GetComponent<buffs_script>();
        control=player.GetComponent<playerControl_script>();
        spells=player.GetComponent<Spells_script>();
    }

    void cast(){
        if(!spells.GodMod){
            if(CD) {CoolDawn(); return;}
            if(stats.CurMana<ManaCost) {NotEnoughtMana(); return;}
        }
    }
    public static void Break(){
    }
    void NotEnoughtMana(){
        Debug.Log("NotMana");
    }
    void CoolDawn(){
        Debug.Log("CoolDown");
    }
    

}
