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
        panel=GameObject.Find("Boost_Panel");
    }
    public void Set(boost_str boost_Str){
        stats=boost_Str.stats;
        description.text=boost_Str.description;
        value=boost_Str.value;
        image=boost_Str.image;
    }
    public void Click(){
        buffs_script buffs = GameObject.Find("Player").GetComponent<buffs_script>();
        buffs.ChangeStats(stats,value);
        buffs.ResetStats();
        panel.SetActive(false);
    }
}

public struct boost_str{
    public string description;
    public Image image;
    public Stats stats;
    public float value;
    public boost_str(Stats stats, string description, float value){
        image=null;
        this.stats=stats;
        this.description=description;
        this.value=value;
    }
}
