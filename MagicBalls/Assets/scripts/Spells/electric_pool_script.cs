using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electric_pool_script : MonoBehaviour
{
    public float thunder;
    public float water;
    [SerializeField] float time=3f;
    [SerializeField] float damage=3f;
    buffs_script buff;
    List<GameObject> aims = new List<GameObject>();

    void Start(){
        gameObject.transform.localScale = new Vector3(water,1,water);
        buff = GameObject.FindGameObjectWithTag("Player").GetComponent<buffs_script>();
        StartCoroutine(Damage());
        Destroy(gameObject, time);
    }
    IEnumerator Damage(){
        while (true)
        {
            foreach(GameObject i in aims){
                i.GetComponent<unit_script>().TakeDamage(damage*thunder*buff.GetStats(Stats.damage)*buff.GetStats(Stats.thunderDamage)*Time.deltaTime, DamageType.Thunder);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.tag=="enemy")  aims.Add(collider.gameObject);
    }

    void OnTriggerExit(Collider collider){
        if (collider.gameObject.tag=="enemy") aims.Remove(collider.gameObject);
    }
}
