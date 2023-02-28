using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lava_script : MonoBehaviour
{
    public float fire;
    public float earht;
    [SerializeField] float slow=0.25f;
    [SerializeField] float time=3f;
    [SerializeField] float damage=3f;
    [SerializeField] LayerMask enemies;
    buffs_script buff;

    void Start(){
        gameObject.transform.localScale = new Vector3(earht*3,1,earht*3);
        buff = GameObject.FindGameObjectWithTag("Player").GetComponent<buffs_script>();
        StartCoroutine(Damage());
        Destroy(gameObject, time);
    }
    IEnumerator Damage(){
        while (true)
        {   
            Collider[] colliders=Physics.OverlapSphere(transform.position, earht*3f, enemies);
            foreach(Collider i in colliders){
                i.gameObject.GetComponent<unit_script>().TakeDamage(damage*fire*buff.GetStats(Stats.fireDamage)*buff.GetStats(Stats.damage)*Time.deltaTime, DamageType.Fire);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.tag=="enemy") collider.gameObject.GetComponent<enemy_script>().ChangeSpeed(-slow);
    }

    void OnTriggerExit(Collider collider){
        if (collider.gameObject.tag=="enemy") collider.gameObject.GetComponent<enemy_script>().ChangeSpeed(+slow);
    }
}