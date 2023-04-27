using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball_script : IProjectile
{
    //Vector3 direction;
    GameObject player;
    buffs_script buffs;

    //BoxCollider collider;
    public  float damage=10;
    //public float speed = 25;
    void Awake(){
        player = GameObject.FindGameObjectWithTag("Player");
        buffs = player.GetComponent<buffs_script>();
    }
    void Start()
    {
        float y = (player.GetComponent<playerControl_script>().mousePos - player.transform.position).normalized.y;
        direction = (gameObject.transform.position - player.transform.position).normalized;
        direction = (direction + new Vector3(0,y,0)).normalized;
        gameObject.transform.position+=Vector3.up*0.5f;
        collider = gameObject.GetComponent<BoxCollider>();
        gameObject.transform.rotation = Quaternion.LookRotation(direction);
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction*speed*Time.deltaTime*buffs.GetStats(Stats.projectileSpeed);
    }
    [SerializeField] LayerMask obstacles;
    protected override void OnTriggerEnter(Collider collision){
        base.OnTriggerEnter(collider);
        
        if (collision.gameObject.tag != "enemy") return;

        collision.gameObject.GetComponent<unit_script>().Move(direction, 0.5f*buffs.GetStats(Stats.repulsion), 0.1f, false);
        collision.gameObject.GetComponent<unit_script>().TakeDamage(damage*buffs.GetStats(Stats.fireDamage)*buffs.GetStats(Stats.damage)*buffs.GetStats(Stats.projectileDamage), DamageType.Fire);
    }
}
