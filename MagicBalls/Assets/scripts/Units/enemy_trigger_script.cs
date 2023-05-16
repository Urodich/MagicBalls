using UnityEngine;

public class enemy_trigger_script : MonoBehaviour
{
    IEnemy parent;
    void Start(){
        parent = gameObject.GetComponentInParent<IEnemy>();
    }

    void OnTriggerEnter(Collider collider){
        parent.OnColliderEnter(gameObject, collider);
    }
    void OnTriggerExit(Collider collider){
        parent.OnColliderExit(gameObject,collider);
    }
}
