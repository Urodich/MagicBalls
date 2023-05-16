using UnityEngine;

public class wind_script : MonoBehaviour
{
    public int strength;
    [SerializeField] float power;
    [SerializeField] float time;
    buffs_script buffs;
    GameObject player;
    public Vector3 direction;

    void Start()
    {
        Destroy(gameObject,time);
        Debug.Log("wind");
        player = GameObject.FindGameObjectWithTag("Player");
        buffs = player.GetComponent<buffs_script>();
        gameObject.transform.localScale=new Vector3(strength,strength,strength);
    }

    void OnTriggerStay(Collider collider){
        if (collider.gameObject.tag != "enemy") return;
        Debug.Log(direction);
        collider.gameObject.GetComponent<unit_script>().Move(direction, power*strength*buffs.GetStats(Stats.repulsion), Time.deltaTime, false);
    }
}
