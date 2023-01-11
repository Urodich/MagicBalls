using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory_script : MonoBehaviour
{

    //BOOTS
    [SerializeField] Transform bootsTransformL;
    [SerializeField] Transform bootsTransformR;
    itemUI_script boots;
    //BRACERS
    [SerializeField] Transform bracersTransformL;
    [SerializeField] Transform bracersTransformR;
    itemUI_script bracers;
    //HEAD
    [SerializeField] Transform headTransform;
    itemUI_script head;

    void Start(){
        GameObject inventory=GameObject.Find("inventory");
        boots=inventory.transform.Find("boots slot").GetComponent<itemUI_script>();
        head=inventory.transform.Find("head slot").GetComponent<itemUI_script>();
        bracers=inventory.transform.Find("bracers slot").GetComponent<itemUI_script>();
    }
    public Item? Equip(Item item){
        Item curr;
        switch (item.type)
        {
            case ItemType.Head: {
                curr=head._item;
                Set(item,head);
                WearOn(head._item, headTransform);
                break;
            }
            case ItemType.Boots: {
                curr=boots._item;
                Set(item,boots);
                WearOn(boots._item, bootsTransformL);
                WearOn(boots._item, bootsTransformR);
                break;
            }
            case ItemType.Bracer: {
                curr=bracers._item;
                Set(item,bracers);
                WearOn(bracers._item, bracersTransformL);
                WearOn(bracers._item, bracersTransformR);
                break;
            }
            default:{
                curr=null;
                break;
            }
        }
        return curr;
    }

    Item Set(Item item, itemUI_script slot){
        Item curr = slot.DeleteItem();
        slot.SetItem(item);
        curr?.Drop();
        return slot._item;
    }
    void WearOn(Item slot, Transform transform){
        slot.transform.SetParent(transform);
        slot.transform.SetLocalPositionAndRotation(Vector3.zero, new Quaternion());
    }

    
}
