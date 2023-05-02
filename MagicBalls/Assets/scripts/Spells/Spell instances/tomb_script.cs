using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tomb_script : MonoBehaviour
{
    [SerializeField] LayerMask enemies;
    [SerializeField] GameObject _zombie;
    void Start()
    {
        StartCoroutine(spawn());
        Destroy(gameObject, 15f);
    }

    IEnumerator spawn(){
        while(true){
            Collider[] enemy=Physics.OverlapSphere(transform.position, 3f, enemies);
            foreach(Collider elem in enemy){
                Instantiate(_zombie, elem.gameObject.transform.position, new Quaternion()).GetComponent<zombie>().SetZombieAim(elem.gameObject);
            }
            yield return new WaitForSeconds(1f);
        }
    }
    // Update is called once per frame
}
