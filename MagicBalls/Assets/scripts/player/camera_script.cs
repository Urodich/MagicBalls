using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_script : MonoBehaviour
{
    GameObject player;
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = gameObject.GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = player.transform.position + new Vector3(0,9,-4);
    }
}
