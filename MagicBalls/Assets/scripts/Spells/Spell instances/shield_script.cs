using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield_script : MonoBehaviour
{

    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.tag=="projectile") Destroy(collider.gameObject);
    }
}
