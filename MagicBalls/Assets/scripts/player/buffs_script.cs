using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class buffs_script : MonoBehaviour
{
    public float MaxHP=1;
    public float HpRegen=1;
    public float MaxMana=1;
    public float ManaRegen=1;
    //old
    public float repulsion = 1;
    public int projectile = 1;
    public float fireDamage = 1;
    public float thunderDamage = 1;
    public float physicalDamage = 1;
    public float damage = 1;
    public float projectileDamage = 1;
    public float projectileSpeed = 1;
    public float speed=1;

    private Dictionary<Stats, float> stats;

    [SerializeField] private Text text;

    void Start(){
        stats = new Dictionary<Stats, float>();
        foreach(Stats elem in Enum.GetValues(typeof(Stats))){
            stats[elem]=1;
        }
    }

    public void ResetStats(){
        text.text=$"отталкивание" + Math.Round(stats[Stats.repulsion], 2)*100+"%\n"+ 
        "доп. снаряды"+projectile+"\n"+
        "урон от огня"+ Math.Round(stats[Stats.fireDamage], 2)*100+"%\n"+
        "урон от молний"+ Math.Round(stats[Stats.thunderDamage], 2)*100+"%\n"+
        "физический урон"+ Math.Round(stats[Stats.physicalDamage], 2)*100+"%\n"+
        "урон"+ Math.Round(stats[Stats.damage], 2)*100+"%\n"+
        "скорость"+Math.Round(stats[Stats.speed], 2)*100+"%";
    }

    public void ChangeStats(Stats index, float value){
        stats[index]+=value;
        if(stats[index]<0)stats[index]=0;
        player_script player = gameObject.GetComponent<player_script>();
        player.UpdateStats();
        if(index==Stats.speed)player.ChangeSpeed(value);
    }
    public float GetStats(Stats stats){
        return this.stats[stats];
    }
}

public enum Stats{
    MaxHP,
    HpRegen,
    MaxMana,
    ManaRegen,
    repulsion,
    ojectile ,
    fireDamage,
    thunderDamage,
    physicalDamage,
    damage,
    projectileDamage,
    projectileSpeed,
    speed
}
