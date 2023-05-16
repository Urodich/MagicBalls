using UnityEngine;

public class camera_script : MonoBehaviour
{
    public GameObject player;
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = gameObject.GetComponent<Camera>();
        transform.rotation=Quaternion.Euler(60,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = player.transform.position + new Vector3(0,9,-4);
    }
}