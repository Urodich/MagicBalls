using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IProjectile : MonoBehaviour
{
    public LayerMask obstacles;
    protected Vector3 direction;
    protected Collider collider;
    [SerializeField] protected float liveTime;
    public float speed = 25;

    public void SetDirection(Vector3 direction){
        this.direction=direction;
    }

    void Update()
    {
        transform.position += direction*speed*Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider collision){
        Debug.Log("collision");
        if (Utils.LayerComparer(collider.gameObject, obstacles)) {Destroy(gameObject); return;}
    }
}
