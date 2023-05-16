using UnityEngine;

public class wave_script : MonoBehaviour
{
    Vector3 direction;
    GameObject player;
    buffs_script buffs;

    BoxCollider collider;
    //public  float damage=0;
    public float speed = 10;
    [SerializeField] float time=1.5f;
    [SerializeField] float strength=3;
    void Awake(){
        player = GameObject.FindGameObjectWithTag("Player");
        buffs = player.GetComponent<buffs_script>();
    }
    void Start()
    {
        direction = (gameObject.transform.position - player.transform.position).normalized;
        collider = gameObject.GetComponent<BoxCollider>();
        gameObject.transform.rotation = Quaternion.LookRotation(direction);
        Destroy(gameObject, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction*speed*Time.deltaTime*buffs.GetStats(Stats.projectileSpeed);
    }

    void OnTriggerEnter(Collider collision){
        if (collision.gameObject.layer == 6) {Destroy(gameObject); return;}
        if (collision.gameObject.tag != "enemy") return;
        
        if (collision.gameObject.GetComponent<unit_script>().isFlying) return;
        collision.gameObject.GetComponent<unit_script>().Move(direction, strength*buffs.GetStats(Stats.repulsion), time, false);
    }
}
