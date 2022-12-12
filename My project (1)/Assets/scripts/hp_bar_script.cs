using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hp_bar_script : MonoBehaviour
{
    GameObject mainCamera;
    // Update is called once per frame
    void Start(){
        mainCamera = GameObject.Find("Main Camera");
    }
    void FixedUpdate()
    {
        transform.rotation=mainCamera.transform.rotation;
    }
}
