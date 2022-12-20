using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spheresParent_script : MonoBehaviour
{
    Quaternion staticRotation;

    void Start()
    {
        staticRotation = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = staticRotation;
    }
}
