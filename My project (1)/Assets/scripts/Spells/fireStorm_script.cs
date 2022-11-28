using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireStorm_script : MonoBehaviour
{
    [SerializeField] LayerMask enemies;
    [SerializeField] float damage=15f;
    [SerializeField] ParticleSystem _boom;
    buffs_script buffs;
    // Start is called before the first frame update
    void Start()
    {
        buffs=GameObject.Find("Player").GetComponent<buffs_script>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position+=Vector3.down*Time.fixedDeltaTime*4f;
    }
    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.layer==6) {Debug.Log("ground"); Die();}
    }
    void Die(){
        _boom.Play();
        Collider[] enemy=Physics.OverlapSphere(transform.position, 3f, enemies);
        foreach(Collider elem in enemy){
            enemy_script en = elem.gameObject.GetComponent<enemy_script>();
            en.Move(elem.transform.position-gameObject.transform.position, 1f, .5f, true);
            en.TakeDamage(damage*buffs.damage*buffs.fireDamage, DamageType.Fire);
            
        }
        Destroy(gameObject);
    }
}
