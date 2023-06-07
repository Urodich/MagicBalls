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

    void Awake(){
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
        text.text=
        String.Format("отталкивание {0,0}", Math.Round(stats[Stats.repulsion], 2)*100)+"%\n"+ 
        String.Format("доп. снаряды {0,0}",(stats[Stats.projectiles]-1))+"\n"+
        String.Format("урон от огня {0,0}", Math.Round(stats[Stats.fireDamage], 2)*100)+"%\n"+
        String.Format("урон от молний {0,0}", Math.Round(stats[Stats.thunderDamage], 2)*100)+"%\n"+
        String.Format("физический урон {0,0}", Math.Round(stats[Stats.physicalDamage], 2)*100)+"%\n"+
        String.Format("урон {0,0}", Math.Round(stats[Stats.damage], 2)*100)+"%\n"+
        String.Format("скорость {0,0}",Math.Round(stats[Stats.speed], 2)*100)+"%"+
        String.Format("здоровье {0,0}",Math.Round(stats[Stats.MaxHP], 2)*100)+"%"+
        String.Format("мана {0,0}",Math.Round(stats[Stats.MaxMana], 2)*100)+"%";
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