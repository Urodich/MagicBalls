using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class illusion_script : unit_script
{
    public Vector3 destination;

    new void Start(){
        base.Start();
        Priority = 5;
        navMesh.destination=destination;
        Invoke("Die", 7f);
    }

    public override void Die()
    {
        Debug.Log(gameObject.name + " died");
        navMesh.enabled=false;
        gameObject.transform.position=new Vector3(0,-100,0);
        Destroy(gameObject, .2f);
    }
}
