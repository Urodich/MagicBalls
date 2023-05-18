using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashEnemy_script : MeleeEnemy_script
{
    [SerializeField] protected Collider attackCol;
    [SerializeField] protected float attackTime=1f;
    new void Start()
    {
        base.Start();
        attackCol.enabled=false;
    }


    protected override void StopAttack(){
        attackCol.enabled =false;
        base.StopAttack();
    }
    protected override IEnumerator AttackDelay(){
        yield return new WaitForSeconds(attackDelay); //delay
        attackCol.enabled=true;
        yield return new WaitForSeconds(attackTime); //attack time
        if(sound) sound.Attack();
        attackCol.enabled =false;
        yield return new WaitForSeconds(attackSpeed); //residual animation  
        navMesh.isStopped=false;
        attacking=false;
    }
    //Triggers
    public override void OnColliderEnter(GameObject obj, Collider collider){
        if (obj.name.Equals("attack")) {
            if(Utils.LayerComparer(collider.gameObject, enemies)/*(1<<collider.gameObject.layer == (1 << collider.gameObject.layer & enemies))*/) {collider.gameObject.GetComponent<unit_script>().TakeDamage(damage*damageFactor, damageType); } 
            return;}
        
        base.OnColliderEnter(obj, collider);
    } 
}
