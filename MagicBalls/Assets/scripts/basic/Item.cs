using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour, IItem
{
    public ItemType type;
    buffs_script buffs;
    Rigidbody rb;
    [SerializeField] Canvas info;
    public Sprite pict;
    public string itemName;
    public string description;
    [SerializeField] List<StatsCount> startBuffs=new List<StatsCount>();
    Dictionary<Stats, float> changeStats = new Dictionary<Stats, float>();
    void Awake(){
        rb=GetComponent<Rigidbody>();
    }
    void Start(){
        string statInfo="";
        foreach(StatsCount elem in startBuffs){
            changeStats.Add(elem.stat, elem.value);
            statInfo+=(elem.value>0?"+":"")+elem.value*100+"% "+elem.stat+" \n";
        }
        Transform inf = transform.Find("info");
        Transform p = inf.Find("image");
        if (p!=null)p.gameObject.GetComponent<Image>().sprite=pict;
        inf.Find("name").gameObject.GetComponent<TextMeshProUGUI>().text=itemName;
        description+="\n"+statInfo;
        inf.Find("description").gameObject.GetComponent<TextMeshProUGUI>().text=description;
        info.gameObject.SetActive(false); 
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

    public void Take(GameObject player){
        inventory_script inv = player.GetComponent<inventory_script>();
        buffs = player.gameObject.GetComponent<buffs_script>();
        foreach (var stat in changeStats)
            buffs.ChangeStats(stat.Key, stat.Value);
        gameObject.layer=0;
        Item old=inv.Equip(this);
        CloseInfo();
        rb.isKinematic=true;
    }
    public void TakeCopy(){
        gameObject.layer=0;
        CloseInfo();
        rb=GetComponent<Rigidbody>();
        rb.isKinematic=true;
    }    
}

public enum ItemType{
    Head,
    Bracer,
    Boots
}
