using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy_script : unit_script, IEnemy
{
    [SerializeField] protected DamageType damageType;
    [SerializeField] protected float damage;
    [SerializeField] protected Collider visibleCol;
    [SerializeField] protected float attackDistance=4f;
    [SerializeField] protected float attackDelay=1f;
    [SerializeField] protected float attackSpeed=1f;
    

    public List<GameObject> aims = new List<GameObject>();
    public GameObject aim;
    protected bool attacking=false;
    protected Coroutine attack;
    public LayerMask enemies;

    protected virtual void StopAttack(){
        StopCoroutine(attack);
        attacking=false;
        if(!isStunned)navMesh.isStopped=false;
    }
    public void ClearAims()
    {
        aims.Clear();
    }
    //выбор цели
    public bool ChangeAim(){
        bool a =false;
        if(aims.Count==0) {aim=null; return false;}
        if (!aim) {aim = aims[0]; a=true;}
        foreach (GameObject i in aims){
            if(i==aim) continue;
            if(i.GetComponent<unit_script>().Priority>aim.GetComponent<unit_script>().Priority) {aim = i; a=true; continue;}
            if((i.transform.position-gameObject.transform.position).sqrMagnitude<(aim.transform.position-gameObject.transform.position).sqrMagnitude) {aim = i; a=true;}
        }
        return a;
    }
    public void AddAim(GameObject _aim){
        if(aims.Contains(_aim)) return;
        aims.Add(_aim);
        ChangeAim();
        OnFindAim(_aim);
    }
    protected void Attack(){
        navMesh.isStopped=true;
        attacking =true;
        attack=StartCoroutine(AttackDelay());
    }
    //Attacking function
    protected virtual IEnumerator AttackDelay(){
        yield return new WaitForSeconds(attackDelay); //delay
        aim.gameObject.GetComponent<unit_script>().TakeDamage(damage*damageFactor, damageType);
        yield return new WaitForSeconds(attackSpeed); //residual animation  
        navMesh.isStopped=false;
        attacking=false;
    }
    void OnFindAim(GameObject _aim){
        Collider[] friends=Physics.OverlapSphere(transform.position, 10f, 8);
        foreach(Collider col in friends){
            col.gameObject.GetComponent<enemy_script>().AddAim(_aim);
        }

    }
    //Triggers
    public virtual void OnColliderEnter(GameObject obj, Collider collider){
        if (obj.name.Equals("vision")){
            if(1<<collider.gameObject.layer == (1 << collider.gameObject.layer & enemies)) {
                AddAim(collider.gameObject); 
                OnFindAim(collider.gameObject);
            }
            return;
        }
    } 
    public virtual void OnColliderExit(GameObject obj, Collider collider){
        if(obj.name=="vision" && (1<<collider.gameObject.layer == (1 << collider.gameObject.layer & enemies))){
            aims.Remove(collider.gameObject);
            if(aim==collider.gameObject)ChangeAim();
        }
    }
}
