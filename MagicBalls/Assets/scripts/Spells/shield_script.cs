using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield_script : MonoBehaviour
{
    Spells_script spells;
    void Start(){
        spells = GameObject.Find("Player").GetComponent<Spells_script>();
    }
    void FixedUpdate()
    {
        if (spells.currentCast==null) Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.tag=="projectile") Destroy(collider.gameObject);
    }
}
