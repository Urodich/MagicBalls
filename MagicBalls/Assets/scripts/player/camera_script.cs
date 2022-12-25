using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_script : MonoBehaviour
{
    [SerializeField] GameObject player;
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = player.transform.position + new Vector3(0,9,-4);
    }
}
