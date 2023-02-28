using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torrent_script : MonoBehaviour
{
    [SerializeField] float stunDuration=1;
    public int air=1;
    public int water=1;

    void Start(){
        Destroy(gameObject, 2);
        gameObject.transform.localScale = new Vector3 (water,1,water);
    }
    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.tag!="enemy") return;
        unit_script enemy = collider.gameObject.GetComponent<unit_script>();
        //enemy.Stun(stunDuration*air);
        enemy.Move(Vector3.up, air, stunDuration*air, true);
    }
}