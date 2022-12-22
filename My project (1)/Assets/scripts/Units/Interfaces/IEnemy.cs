using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnemy
{
    bool attacking{get;set;}
    public bool isFlying{get;set;}
    bool isGrounded{get;set;}

    [SerializeField] DamageType damageType{get;set;}
    [SerializeField] Collider visibleCol{get;set;}
    [SerializeField] float attackDistance{get;set;}
    [SerializeField] float attackDelay{get;set;}
    [SerializeField] float attackSpeed{get;set;}

    public List<GameObject> aims{get;set;}
    public GameObject aim{get;set;}

    Coroutine attack{get;set;}
    public LayerMask enemies{get;set;}


    public bool ChangeAim();
    void Attack();
    IEnumerator AttackDelay();
    public void OnColliderEnter(GameObject obj, Collider collider);
    public void OnColliderExit(GameObject obj, Collider collider);
}
