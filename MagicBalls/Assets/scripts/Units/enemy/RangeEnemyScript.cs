using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyScript : enemy_script
{
    [SerializeField] LayerMask obstacles;

    new void FixedUpdate()
    {
        if(!isActive) return;
        base.FixedUpdate();
        
        if(isStunned) return;
        if (!aim) {animator.SetBool("run", false); return;}

        transform.LookAt(aim.transform);
        //атаки и передвижение
        if ((aim.transform.position-gameObject.transform.position).sqrMagnitude>attackDistance*attackDistance) {
            if(attacking){
                StopCoroutine(curAction);
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
                    if(!attacking) Attack();
                }
            }
        }
        if(!attacking) ChangeAim();
        
    }

    [SerializeField] GameObject _projectile;

    //Attacking function
    protected override IEnumerator AttackDelay()
    {
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(attackDelay); //delay
        Destroy(Instantiate(_projectile, transform.position + Vector3.up * 0.5f, transform.rotation), 3);
        yield return new WaitForSeconds(attackSpeed); //residual animation  
        navMesh.isStopped = false;
        attacking = false;
    }
}
