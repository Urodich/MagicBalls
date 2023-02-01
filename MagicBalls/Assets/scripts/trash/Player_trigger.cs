using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_trigger : MonoBehaviour
{
    [SerializeField] GameObject aim;
    void OnCollisionExit(Collision collision){
        if (collision.collider.gameObject.tag=="Player") {
            Debug.Log(collision.contactCount);
            if (collision.transform.position.y>gameObject.transform.position.y)
                aim.layer=6;
            else aim.layer=10;
        }
    }
    void OnCollisionEnter(Collision collision){
        if (collision.collider.gameObject.tag=="Player") aim.layer=10;
    }
}
