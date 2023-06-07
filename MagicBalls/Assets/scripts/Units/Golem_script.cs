using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_script : SplashEnemy_script
{
    [SerializeField] float echoDelay=0.5f;
    [SerializeField] float echoRadius = 3;
    [SerializeField] float echoCD = 5;
    float _echoCD = 0;
    [SerializeField] ParticleSystem echo;
    GameObject player;
    void Awake(){
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        if(_echoCD > 0) _echoCD-=Time.fixedDeltaTime;
        if (isStunned) return;
        if (!aim) {
            if((transform.position-player.transform.position).sqrMagnitude>4) {
                WalkTo(player.transform.position);
                animator.SetBool("run", true);
                transform.LookAt(player.transform);
            }
            else animator.SetBool("run", false); 
            return; 
        }

        transform.LookAt(aim.transform);
        //����� � ������������
        if ((aim.transform.position - gameObject.transform.position).sqrMagnitude > attackDistance * attackDistance)
        {
            if (attacking && (aim.transform.position - gameObject.transform.position).sqrMagnitude > attackDistance * attackDistance * 2)
            {
                StopAttack();
                animator.SetTrigger("break");
            }
            if(!attacking) WalkTo(aim.transform.position);
            return;
        }
        else
            StopWalking();
        if (!attacking && _echoCD<=0) curAction = StartCoroutine(Echo());
        if (!attacking) { Attack(); Debug.Log("golem attack"); animator.SetTrigger("attack"); }

        if (!attacking) ChangeAim();
    }

    IEnumerator Echo()
    {
        Debug.Log("ECHO");
        attacking = true;
        animator.SetTrigger("echo");
        yield return new WaitForSeconds(echoDelay);
        _echoCD=echoCD;
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
