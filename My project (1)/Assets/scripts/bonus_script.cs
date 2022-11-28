using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bonus_script : MonoBehaviour
{

    [SerializeField] float value;
    [SerializeField] bool IsHpHeal;
    void Start()
    {
        Destroy(gameObject, 10f);
    }
    
    void OnTriggerEnter(Collider collider){
        Debug.Log("Taken");
        if(collider.gameObject.tag!="Player") return;
        Debug.Log("Taken");
        if(IsHpHeal)collider.gameObject.GetComponent<player_script>().AddHP(value);
        else collider.gameObject.GetComponent<player_script>().AddMana(value);
        Destroy(gameObject);
    }
    
}
