using UnityEngine;

public class projectile_script : MonoBehaviour
{
    public int damage;
    public DamageType type;
    [SerializeField] float speed;
    [SerializeField] LayerMask target;
    [SerializeField] LayerMask enemy;
    [SerializeField] ParticleSystem hit;

    void FixedUpdate()
    {
        transform.position += speed * Time.fixedDeltaTime * transform.forward;
        //transform.Translate(Vector3.forward*speed*Time.fixedDeltaTime, Space.Self);
    }
    void OnTriggerEnter(Collider collider){
        if (1<<collider.gameObject.layer != (1 << collider.gameObject.layer & target)) return;
        if (1<<collider.gameObject.layer == (1 << collider.gameObject.layer & enemy)){
            collider.gameObject.GetComponent<unit_script>().TakeDamage(damage, type);
        }
        if (hit!=null) Destroy(Instantiate(hit, transform.position, transform.rotation).gameObject,1f);
        Destroy(gameObject);
    }
}
