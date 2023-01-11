using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour
{
    public ItemType type;
    buffs_script buffs;
    Rigidbody rb;
    [SerializeField] Canvas info;
    [SerializeField] Sprite pict;
    [SerializeField] string itemName;
    [SerializeField] string description;
    [SerializeField] List<StatsCount> startBuffs=new List<StatsCount>();
    Dictionary<Stats, float> changeStats = new Dictionary<Stats, float>();
    void Start(){
        string statInfo="";
        foreach(StatsCount elem in startBuffs){
            changeStats.Add(elem.stat, elem.value);
            statInfo+=(elem.value>0?"+":"")+elem.value*100+"% "+elem.stat+" \n";
        }
        buffs = GameObject.Find("Player").GetComponent<buffs_script>();
        Transform inf = transform.Find("info");
        Transform p = inf.Find("image");
        if (p!=null)p.gameObject.GetComponent<Image>().sprite=pict;
        inf.Find("name").gameObject.GetComponent<TextMeshProUGUI>().text=itemName;
        inf.Find("description").gameObject.GetComponent<TextMeshProUGUI>().text=description+"\n"+statInfo;
        info.gameObject.SetActive(false);

        rb=GetComponent<Rigidbody>();
    }
    public void ShowInfo(){
        info.gameObject.SetActive(true);
    }
    public void CloseInfo(){
        info.gameObject.SetActive(false);
    }
    public void Drop(){
        foreach (var stat in changeStats)
            buffs.ChangeStats(stat.Key, -stat.Value);
        Vector3 pos = gameObject.transform.position;
        gameObject.transform.SetParent(null);
        gameObject.layer=12;
        rb.isKinematic=false;
    }

    public void Take(inventory_script player){
        foreach (var stat in changeStats)
            buffs.ChangeStats(stat.Key, stat.Value);
        gameObject.layer=0;
        Item old=player.Equip(this);
        CloseInfo();
        rb.isKinematic=true;
    }    
}

public enum ItemType{
    Head,
    Bracer,
    Boots
}
