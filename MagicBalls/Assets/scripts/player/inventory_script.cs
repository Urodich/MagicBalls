using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory_script : MonoBehaviour
{
    Item boots;
    [SerializeField] Transform bootsTransform;
    Item bracers;
    [SerializeField] Transform bracersTransform;
    Item head;
    [SerializeField] Transform headTransform;

    public Item? Equip(Item item){
        Item curr;
        switch (item.type)
        {
            case ItemType.Head: {
                curr=head;
                head=Set(item,head,headTransform);
                break;
            }
            case ItemType.Boots: {
                curr=boots;
                boots=Set(item,boots,bootsTransform);
                break;
            }
            case ItemType.Bracer: {
                curr=bracers;
                bracers=Set(item,bracers,bracersTransform);
                break;
            }
            default:{
                curr=null;
                break;
            }
        }
        return curr;
    }

    Item Set(Item item, Item slot, Transform transform){
        Item curr = slot;
        slot = item;
        curr?.Drop();
        slot.transform.SetParent(headTransform);
        slot.transform.SetLocalPositionAndRotation(Vector3.zero, new Quaternion());
        return slot;
    }

    
}
