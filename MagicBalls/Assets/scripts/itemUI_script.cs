using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class itemUI_script : MonoBehaviour
{
    public Item _item{get; private set;}
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI info;

    void Start(){
        if(!_item)_image.raycastTarget=false;
    }
    public void SetItem(Item item){
        _image.raycastTarget=true;
        _item=item;
        _image.sprite=item.pict;
        info.text=item.description;
        itemName.text=item.itemName;
        
    }
    public Item DeleteItem(){
        _image.raycastTarget=false;
        _image.sprite=null;
        Item old=_item;
        _item=null;
        return old;
    }
}
