using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoke_script : MonoBehaviour
{
    public int fire=1;
    public int water=1;
    ParticleSystem sys;
    void Start(){
        sys=gameObject.GetComponent<ParticleSystem>();
        Invoke("End",3);
    }
    void End(){
        sys.Stop(true);
        Destroy(gameObject, 3);
    }

    void OnTriggerEnter(Collider collider){
        if(collider.tag=="enemy"){
            unit_script enemy = collider.GetComponent<unit_script>();
            enemy.ThunderResist-=water*0.2f;
            enemy.FireResist+=water*.3f;
            enemy.damageFactor-=fire*.15f;
            collider.GetComponent<IEnemy>().ClearAims();
            collider.GetComponent<IEnemy>().ChangeAim();
        }
    }
    void OnTriggerExit(Collider collider){
        if(collider.tag=="enemy"){
            unit_script enemy = collider.GetComponent<unit_script>();
            enemy.ThunderResist+=water*0.2f;
            enemy.FireResist-=water*.3f;
            enemy.damageFactor+=fire*.15f;
            collider.GetComponent<IEnemy>().ChangeAim();
        }
    }
}