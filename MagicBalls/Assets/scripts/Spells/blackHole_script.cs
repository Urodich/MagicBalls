using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackHole_script : MonoBehaviour
{
    buffs_script buffs;
    [SerializeField] float damage=5;
    [SerializeField] float radius=15f;
    [SerializeField] float damageRadius=2f;
    [SerializeField] float power=10f;
    public float lifeTime=3f;
    // Start is called before the first frame update
    void Start()
    {
        
        buffs=GameObject.Find("Player").GetComponent<buffs_script>();
        StartCoroutine(work());
        Invoke("Die", lifeTime);
    }
    void Die(){
        Collider[] colliders = Physics.OverlapSphere(transform.position+Vector3.up, 2*damageRadius);
            foreach (Collider i in colliders){
                if(i.tag!="enemy") continue;
                Vector3 dir=(gameObject.transform.position-i.gameObject.transform.position);
                dir.Scale(new Vector3(1,0,1));
                i.gameObject.GetComponent<unit_script>().TakeDamage(damage*buffs.damage*8, DamageType.Physical);
            }
        Destroy(gameObject);
    }

    // Update is called once per frame
    IEnumerator work(){
        while(true){
            Collider[] colliders = Physics.OverlapSphere(transform.position+Vector3.up, radius);
            foreach (Collider i in colliders){
                if(i.tag!="enemy") continue;
                Vector3 dir=(gameObject.transform.position-i.gameObject.transform.position);
                dir.Scale(new Vector3(1,0,1));
                i.gameObject.GetComponent<unit_script>().Move(dir.normalized, power, 1f, false);
            }
            float _damage=damage*buffs.damage*buffs.thunderDamage;
            colliders = Physics.OverlapSphere(transform.position+Vector3.up, damageRadius);
            foreach (Collider i in colliders){
                if(i.tag!="enemy") continue;
                Vector3 dir=(gameObject.transform.position-i.gameObject.transform.position);
                dir.Scale(new Vector3(1,0,1));
                if(i!=null)i.gameObject.GetComponent<unit_script>().TakeDamage(_damage, DamageType.Thunder);
            }
        yield return new WaitForSeconds(.3f);
        }
    }
}
