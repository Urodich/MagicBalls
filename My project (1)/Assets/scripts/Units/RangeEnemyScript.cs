using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyScript : enemy_script
{
    [SerializeField] LayerMask obstacles;
    new void Start()
    {
        base.Start();
        navMesh.speed=speed;
        aims = new List<GameObject>();
    }

    void FixedUpdate()
    {
        if(isStunned) return;
        if (!aim) {animator.SetBool("run", false); return;}

        transform.LookAt(aim.transform);
        //атаки и передвижение
        if ((aim.transform.position-gameObject.transform.position).sqrMagnitude>attackDistance*attackDistance) {
            if(attacking){
                StopCoroutine(attack);
                attacking=false;
                navMesh.isStopped=false;
            } 
            navMesh.destination = aim.transform.position;
            animator.SetBool("run", true);
            return;
        }
        else{
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, obstacles)){
                if(1<<hit.collider.gameObject.layer == (1 << hit.collider.gameObject.layer & enemies)){
                    animator.SetBool("run", false);
                    if(!attacking) {Attack(); animator.SetTrigger("attack");}
                }
            }
        }
        //if(!attacking) ChangeAim();
        
    }
    [SerializeField] GameObject _projectile;
    void Attack(){
        navMesh.isStopped=true;
        attacking =true;
        attack=StartCoroutine(AttackDelay());
    }
    //Attacking function
    IEnumerator AttackDelay(){
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(attackDelay); //delay
        Instantiate(_projectile, transform.position, transform.rotation);
        yield return new WaitForSeconds(attackSpeed); //residual animation  
        navMesh.isStopped=false;
        attacking=false;
    }
}
