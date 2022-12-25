using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteor_script : MonoBehaviour
{
    [SerializeField] float damage=15;
    public Vector3 direction;
    Rigidbody rigidbody;
    buffs_script buffs;
    GameObject player;
    LayerMask enemies;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidbody=gameObject.GetComponent<Rigidbody>();
        rigidbody.AddForce(direction*3, ForceMode.Impulse);
        buffs = player.GetComponent<buffs_script>();
        StartCoroutine(Damage());
        Destroy(gameObject, 6f);
    }

        IEnumerator Damage(){
        while (true)
        {
            Collider[] cols=Physics.OverlapSphere(transform.position, 1, enemies);
            foreach(Collider i in cols){
                unit_script enemy = i.gameObject.GetComponent<unit_script>();
                enemy.TakeDamage(damage*buffs.damage*buffs.fireDamage*Time.deltaTime, DamageType.Fire);
                enemy.Stun(.2f);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
