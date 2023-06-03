using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_script : SplashEnemy_script
{
    [SerializeField] float echoDelay=0.5f;
    [SerializeField] float echoRadius = 3;
    [SerializeField] ParticleSystem echo;
    void FixedUpdate()
    {
        base.FixedUpdate();

        if (isStunned) return;
        if (!aim) { animator.SetBool("run", false); return; }

        transform.LookAt(aim.transform);
        //атаки и передвижение
        if ((aim.transform.position - gameObject.transform.position).sqrMagnitude > attackDistance * attackDistance)
        {
            if (attacking && (aim.transform.position - gameObject.transform.position).sqrMagnitude > attackDistance * attackDistance * 2)
            {
                StopAttack();
                animator.SetTrigger("break");
            }
            WalkTo(aim.transform.position);
            return;
        }
        else
            StopWalking();
        if (!attacking) curAction =StartCoroutine(Echo());
        if (!attacking) { Attack(); Debug.Log("golem attack"); animator.SetTrigger("attack"); }

        if (!attacking) ChangeAim();
    }

    IEnumerator Echo()
    {
        attacking = true;
        animator.SetTrigger("Echo");
        yield return new WaitForSeconds(echoDelay);
        echo.Play();
        Collider[] col = Physics.OverlapSphere(transform.position, echoRadius, enemies);
        foreach (Collider elem in col)
        {
            enemy_script enemy = elem.GetComponent<enemy_script>();
            enemy.TakeDamage(damage, damageType);
            enemy.Stun(1);
        }
        attacking = false;
    }

    protected override IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay); //delay
        attackCol.enabled = true;
        yield return new WaitForSeconds(attackTime); //attack time
        if (sound) sound.Attack();
        attackCol.enabled = false;
        yield return new WaitForSeconds(attackSpeed); //residual animation  
        navMesh.isStopped = false;
        attacking = false;
    }

    public override void OnColliderEnter(GameObject obj, Collider collider)
    {
        if (obj.name.Equals(attackCol.name))
        {
            if (Utils.LayerComparer(collider.gameObject, enemies)) { collider.gameObject.GetComponent<unit_script>().TakeDamage(damage * damageFactor, damageType);}
            return;
        }
        base.OnColliderEnter(obj, collider);
    }
}
