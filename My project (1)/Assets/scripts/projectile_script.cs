using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile_script : MonoBehaviour
{
    public int damage;
    public DamageType type;
    [SerializeField] float speed;
    [SerializeField] LayerMask target;
    [SerializeField] LayerMask enemy;
    [SerializeField] ParticleSystem hit;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward*speed, Space.Self);
    }
    void OnColliderEnter(Collider collider){
        if (1<<collider.gameObject.layer != (1 << collider.gameObject.layer & target)) return;
        if (1<<collider.gameObject.layer != (1 << collider.gameObject.layer & enemy)){
            collider.gameObject.GetComponent<unit_script>().TakeDamage(damage, type);
        }
        if (hit!=null) Instantiate(hit, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
