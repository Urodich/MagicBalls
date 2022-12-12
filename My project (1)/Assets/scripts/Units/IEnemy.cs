using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public bool ChangeAim();
    void Attack();
    IEnumerator AttackDelay();
    public void OnColliderEnter(GameObject obj, Collider collider);
    public void OnColliderExit(GameObject obj, Collider collider);
}
