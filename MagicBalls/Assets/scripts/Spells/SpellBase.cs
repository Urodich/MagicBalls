using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public abstract class SpellBase : MonoBehaviour
{
    protected bool CD=false;
    [SerializeField] protected float delay, CoolDown, ManaCost;
    //[SerializeField] protected string _animation, _icon;
    protected GameObject player;
    protected player_script stats;
    protected Spells_script spells;
    protected NavMeshAgent navMesh;
    protected buffs_script buffs;
    [SerializeField] protected GameObject prefab;
    protected void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<player_script>();
        navMesh = player.GetComponent<NavMeshAgent>();
        buffs = player.GetComponent<buffs_script>();
        spells=player.GetComponent<Spells_script>();
    }

    public void Cast(){
        if(!spells.GodMod){
            if(CD) {spells.CoolDawn(); return;}
            if(stats.CurMana<ManaCost) {spells.NotEnoughtMana(); return;}
        }
        Debug.Log("cast "+this);
        spells.currentCast = StartCoroutine(core());
    }
    public void Cast(int i){
        if(!spells.GodMod){
            if(CD) {spells.CoolDawn(); return;}
            if(stats.CurMana<ManaCost*i) {spells.NotEnoughtMana(); return;}
        }
        Debug.Log("cast "+this);
        spells.currentCast = StartCoroutine(core(i));
    }
    public void Cast(int a, int b){
        if(!spells.GodMod){
            if(CD) {spells.CoolDawn(); return;}
            if(stats.CurMana<ManaCost) {spells.NotEnoughtMana(); return;}
        }
        Debug.Log("cast "+this);
        spells.currentCast = StartCoroutine(core(a, b));
    }
    protected abstract IEnumerator core();
    protected abstract IEnumerator core(int a);
    protected abstract IEnumerator core(int a, int b);
    public virtual void Break(){
        StopCoroutine(spells.currentCast);
    }

    
}
