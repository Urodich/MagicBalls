using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class buffs_script : MonoBehaviour
{
    private Dictionary<Stats, float> stats;
    TextMeshProUGUI text;

    void Start(){
        text=GameObject.Find("StatsText").GetComponent<TextMeshProUGUI>();
        stats = new Dictionary<Stats, float>();
        foreach(Stats elem in Enum.GetValues(typeof(Stats))){
            stats[elem]=1;
        }
    }
///////////////////////////////////////////////////
    public void Load(){
        foreach(Stats elem in Enum.GetValues(typeof(Stats))){           //ADD SAVES
            stats[elem]=PlayerPrefs.GetFloat(elem.ToString(),1);
        }
    }
    public void Save(){
        foreach(Stats elem in Enum.GetValues(typeof(Stats))){           //SAVE
            PlayerPrefs.SetFloat(elem.ToString(),stats[elem]);
        }
    }
    public void Reset(){
        foreach(Stats elem in Enum.GetValues(typeof(Stats))){           //SET PREFS TO DEFAULT
            PlayerPrefs.SetFloat(elem.ToString(),1);
        }
    }
///////////////////////////////////////////////////
    public void UpdateStatsText(){
        text.text=$"отталкивание" + Math.Round(stats[Stats.repulsion], 2)*100+"%\n"+ 
        "доп. снаряды"+(stats[Stats.projectiles]-1)+"\n"+
        "урон от огня"+ Math.Round(stats[Stats.fireDamage], 2)*100+"%\n"+
        "урон от молний"+ Math.Round(stats[Stats.thunderDamage], 2)*100+"%\n"+
        "физический урон"+ Math.Round(stats[Stats.physicalDamage], 2)*100+"%\n"+
        "урон"+ Math.Round(stats[Stats.damage], 2)*100+"%\n"+
        "скорость"+Math.Round(stats[Stats.speed], 2)*100+"%";
    }

    public void ChangeStats(Stats index, float value){
        stats[index]+=value;
        player_script player = gameObject.GetComponent<player_script>();
        player.UpdateStaticStats();
    }
    public float GetStats(Stats stats){
        return this.stats[stats];
    }
}
[Serializable]
public struct StatsCount{
    public Stats stat;
    public float value;
}
public enum Stats{
    MaxHP,
    HpRegen,
    MaxMana,
    ManaRegen,
    repulsion,
    projectiles ,
    fireDamage,
    thunderDamage,
    physicalDamage,
    damage,
    projectileDamage,
    projectileSpeed,
    speed
}
