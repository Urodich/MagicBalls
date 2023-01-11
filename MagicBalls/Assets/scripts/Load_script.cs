using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load_script : MonoBehaviour
{
    void Start()
    {
        disableCollisions();

    }

    void disableCollisions(){
        Physics.IgnoreLayerCollision(11,0);
        Physics.IgnoreLayerCollision(11,8);
        Physics.IgnoreLayerCollision(11,7);
        Physics.IgnoreLayerCollision(11,9);

        Physics.IgnoreLayerCollision(12,0);
        Physics.IgnoreLayerCollision(12,8);
        Physics.IgnoreLayerCollision(12,7);
        Physics.IgnoreLayerCollision(12,9);
        Physics.IgnoreLayerCollision(12,11);
    }

    void linkHUD(){
        GameObject player = GameObject.Find("Player");
    }
}
