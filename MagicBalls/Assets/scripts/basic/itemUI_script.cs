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

    Color _empty = Color.white, _fill = Color.white;
    void Start(){
        _empty.a=0.5f;
        _fill.a=1f;
        if(!_item)_image.raycastTarget=false;
    }
    public void SetItem(Item item){
        _image.raycastTarget=true;
        _item=item;
        _image.sprite=item.pict;
        _image.color=_fill;
        info.text=item.description;
        itemName.text=item.itemName;
        
    }
    public Item DeleteItem(){
        _image.raycastTarget=false;
        _image.sprite=null;
        _image.color=_empty;
        Item old=_item;
        _item=null;
        return old;
    }
}
