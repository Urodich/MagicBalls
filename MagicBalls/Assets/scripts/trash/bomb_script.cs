using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb_script : MonoBehaviour
{
    [SerializeField] ParticleSystem first;
    [SerializeField] ParticleSystem second;

    public void Esplose(float damage, DamageType type, LayerMask enemies){
        StartCoroutine(exp(damage, type, enemies));
    }

    IEnumerator exp(float damage, DamageType type, LayerMask enemies){
        first.Play();
        yield return new WaitForSeconds(3);
        Instantiate(second, transform);
        Collider[] cols=Physics.OverlapSphere(transform.position, 2, enemies);
        foreach (Collider col in cols){
            col.gameObject.GetComponent<unit_script>().TakeDamage(damage, type);
        }
        Destroy(gameObject); 
    }
}
