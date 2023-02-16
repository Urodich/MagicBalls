using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class tornado_script : MonoBehaviour
{
    buffs_script buffs;
    [SerializeField] float damage=5;
    [SerializeField] float radius=8f;
    [SerializeField] float power=4f;
    public float lifeTime=10f;
    NavMeshAgent navMesh;
    // Start is called before the first frame update
    void Start()
    {
        navMesh=gameObject.GetComponent<NavMeshAgent>();
        buffs=GameObject.Find("Player").GetComponent<buffs_script>();
        StartCoroutine(work());
        Destroy(gameObject,lifeTime);
    }

    // Update is called once per frame
    IEnumerator work(){
        while(true){
            Vector3 offset = new Vector3(Random.Range(-5,5),0,Random.Range(-5,5));
            navMesh.destination=transform.position+offset;
            Collider[] colliders = Physics.OverlapSphere(transform.position+Vector3.up, radius);
            foreach (Collider i in colliders){
                if(i.tag!="enemy") continue;
                Vector3 dir=(gameObject.transform.position-i.gameObject.transform.position);
                dir.Scale(new Vector3(1,0,1));
                i.gameObject.GetComponent<unit_script>().Move(dir.normalized, power, 1f, false);
            }
            Collider[] colliders2 = Physics.OverlapSphere(transform.position+Vector3.up, radius/2);
            foreach (Collider i in colliders2){
                if(i.tag!="enemy") continue;
                i.gameObject.GetComponent<unit_script>().TakeDamage(damage*buffs.GetStats(Stats.damage)*buffs.GetStats(Stats.physicalDamage), DamageType.Physical);
            }
        yield return new WaitForSeconds(.3f);
        }
    }
}
