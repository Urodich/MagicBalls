using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chest_script : MonoBehaviour, IItem
{
    bool closed=true;
    void Start()
    {
        go.SetActive(true);
    }
    [SerializeField]GameObject go;
    public void Take(GameObject player){
        if(!closed) return;
        go.transform.Rotate(new Vector3(0,0,-90), Space.Self);
        closed=false;
    }
    public void ShowInfo(){}
    public void CloseInfo(){}
}
