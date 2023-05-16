using UnityEngine;

public class spheresParent_script : MonoBehaviour
{
    Quaternion staticRotation;
    void Start()
    {
        staticRotation = gameObject.transform.rotation;
    }
    void Update()
    {
        gameObject.transform.rotation = staticRotation;
    }
}
