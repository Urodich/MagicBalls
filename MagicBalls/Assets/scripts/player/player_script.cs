using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.UIElements;

public class player_script : unit_script
{
    public float maxMana=100f;
    protected float maxManaFactor=1;
    float curMana;
    public float CurMana{get{return curMana;} set{curMana = value>maxMana? maxMana:value; manaBar.value=curMana;}}
    public float manaRegen=2f;
    protected float manaRegenFactor=1;
    public bool reincarnation = false;
    arena_script arena_Script;
    buffs_script buffs;
    [SerializeField] LayerMask enemies;
    //[SerializeField] Animator animator;
    //[SerializeField] UnityEngine.UIElements.Slider manaBar;
    UnityEngine.UI.Slider manaBar;
    Spells_script spells;

    new void Start(){
        spells=gameObject.GetComponent<Spells_script>();
        startAim=Vector3.zero;
        arena_Script=GameObject.Find("HUD").GetComponent<arena_script>();
        linkHUD();
        buffs=gameObject.GetComponent<buffs_script>();
        base.Start();
        manaBar.maxValue=maxMana;
        curMana=maxMana;
    }
    new void FixedUpdate(){
        base.FixedUpdate();

        if(CurMana<maxMana)CurMana+=manaRegen*manaRegenFactor*Time.fixedDeltaTime;
        if(CurMana>maxMana) CurMana=maxMana;
    }
    void linkHUD(){
        GameObject bars = GameObject.Find("Player Bars");
        hpBar = bars.transform.Find("HpBar").GetComponent<Slider>();
        manaBar = bars.transform.Find("ManaBar").GetComponent<Slider>();
    }
    public void UpdateStats(){
        maxHpFactor=buffs.GetStats(Stats.MaxHP);
        maxManaFactor=buffs.GetStats(Stats.MaxMana);
        manaRegenFactor=buffs.GetStats(Stats.ManaRegen);
        hpRegenFactor=buffs.GetStats(Stats.HpRegen);
    }
    public void AddMana(float value){
        CurMana+=value;
    }
    public override void TakeDamage(float damage, DamageType type)
    {
        if(!spells.GodMod)base.TakeDamage(damage, type);
        //Debug.Log("hero take " + damage + "damage");
    }
    public override void Stun(float time){
        base.Stun(time);
        spells.BreakCast();
        animator.SetBool("run", false);
        animator.SetBool("moonwalk", false);
        animator.SetBool("casting", false);
    }
    public override void Die(){
        if(reincarnation){
            reincarnation=false;
            curHp=maxHp;
            return;
        }
        animator.SetTrigger("die");
        Debug.Log(gameObject.name + " died");
        arena_Script.End();
    }

}
