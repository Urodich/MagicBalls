using UnityEngine;

public class canvasRotate_script : MonoBehaviour
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
