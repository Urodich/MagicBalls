using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_scene_load : MonoBehaviour
{
    GameObject player;
    [SerializeField] Load_script load;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<camera_script>().player=player;
        Instantiate(load);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
