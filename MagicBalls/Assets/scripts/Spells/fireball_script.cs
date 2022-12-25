using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball_script : MonoBehaviour
{
    Vector3 direction;
    GameObject player;
    buffs_script buffs;

    BoxCollider collider;
    public  float damage=10;
    public float speed = 25;
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
        transform.position += direction*speed*Time.deltaTime*buffs.projectileSpeed;
    }

    void OnTriggerEnter(Collider collision){
        Debug.Log("collision");
        if (collision.gameObject.layer == 6) {Destroy(gameObject); return;}
        if (collision.gameObject.tag != "enemy") return;

        collision.gameObject.GetComponent<unit_script>().Move(direction, 0.5f*buffs.repulsion, 0.1f, false);
        collision.gameObject.GetComponent<unit_script>().TakeDamage(damage*buffs.fireDamage*buffs.damage*buffs.projectileDamage, DamageType.Fire);
    }
}
