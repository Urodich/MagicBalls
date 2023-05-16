using UnityEngine;

public class mud_script : MonoBehaviour
{
    public float water;
    public float earht;
    [SerializeField] float slow=0.25f;
    [SerializeField] float time=3f;

    void Start(){
        gameObject.transform.localScale = new Vector3(earht,1,earht);
        Destroy(gameObject, time*water);
    }
    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.tag=="enemy") collider.gameObject.GetComponent<unit_script>().ChangeSpeed(-slow*water);
    }
    void OnTriggerExit(Collider collider){
        if (collider.gameObject.tag=="enemy") collider.gameObject.GetComponent<unit_script>().ChangeSpeed(+slow*water);
    }
}
