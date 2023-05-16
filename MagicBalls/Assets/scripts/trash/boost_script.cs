using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boost_script : MonoBehaviour
{
    [SerializeField] public Text description;
    [SerializeField] public Image image;

    [SerializeField] public Stats stats;
    [SerializeField] public float value;
    GameObject panel;
    void Start(){
        panel=GameObject.Find("boost panel");
    }
    public void Set(boost_str boost_Str){
        stats=boost_Str.stats;
        description.text=boost_Str.description;
        value=boost_Str.value;
        image=boost_Str.image;
    }
    public void Click(){
        buffs_script buffs = GameObject.FindGameObjectWithTag("Player").GetComponent<buffs_script>();
        buffs.ChangeStats(stats,value);
        buffs.UpdateStatsText();
        panel.SetActive(false);
    }
}


