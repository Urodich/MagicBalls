using UnityEngine;
public class bonus_script : MonoBehaviour
{
    [SerializeField] float value;
    [SerializeField] Stats stat;
    void Start()
    {
        Destroy(gameObject, 10f);
    }
    
    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.tag!="Player") return;
        buffs_script buff = collider.gameObject.GetComponent<buffs_script>();
        collider.gameObject.GetComponent<player_script>().AddEffect(()=>buff.ChangeStats(stat, value), ()=>buff.ChangeStats(stat, -value), 5, true);
        Destroy(gameObject);
    }
}
