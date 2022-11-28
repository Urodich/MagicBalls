using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_trigger_script : MonoBehaviour
{
    enemy_script parent;
    void Start(){
        parent = gameObject.GetComponentInParent<enemy_script>();
    }

    void OnTriggerEnter(Collider collider){
        parent.OnColliderEnter(gameObject, collider);
    }
    void OnTriggerExit(Collider collider){
        parent.OnColliderExit(gameObject,collider);
    }
}
