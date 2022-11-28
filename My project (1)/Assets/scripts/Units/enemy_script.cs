using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy_script : unit_script
{
    [SerializeField] DamageType damageType;
    [SerializeField] Collider attackCol;
    [SerializeField] Collider visibleCol;
    [SerializeField] protected float attackDistance=4f;
    [SerializeField] protected float attackDelay=1f;
    [SerializeField] protected float attackSpeed=1f;
    [SerializeField] protected float attackTime=1f;
    public List<GameObject> aims;
    public GameObject aim;
    protected bool attacking=false;
    protected Coroutine attack;
    public LayerMask enemies;

    new void Start()
    {
        base.Start();
        //navMesh = gameObject.GetComponent<NavMeshAgent>();
        attackCol.enabled=false;
        navMesh.speed=speed;
        aims = new List<GameObject>();
    }

    new void FixedUpdate()
    {
        //base.FixedUpdate();
        if(isStunned) return;
        if (!aim) {animator.SetBool("run", false); return;}

        transform.LookAt(aim.transform);
        //атаки и передвижение
        if ((aim.transform.position-gameObject.transform.position).sqrMagnitude>attackDistance*attackDistance) {
            if(attacking){
                StopCoroutine(attack);
                attacking=false;
                attackCol.enabled =false;
                navMesh.isStopped=false;
            } 
            navMesh.destination = aim.transform.position;
            animator.SetBool("run", true);
            return;
        }
        else       
            animator.SetBool("run", false);
            if(!attacking) {Attack(); animator.SetTrigger("attack");}

        if(!attacking) ChangeAim();
        
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
    void Attack(){
        navMesh.isStopped=true;
        attacking =true;
        attack=StartCoroutine(AttackDelay());
    }
    //Attacking function
    IEnumerator AttackDelay(){
        yield return new WaitForSeconds(attackDelay); //delay
        attackCol.enabled=true;
        yield return new WaitForSeconds(attackTime); //attack time
        attackCol.enabled =false;
        yield return new WaitForSeconds(attackSpeed); //residual animation  
        navMesh.isStopped=false;
        attacking=false;
    }
    //Triggers
    public void OnColliderEnter(GameObject obj, Collider collider){
        if (obj.name.Equals("attack")) {
            if((1<<collider.gameObject.layer == (1 << collider.gameObject.layer & enemies))) {collider.gameObject.GetComponent<unit_script>().TakeDamage(damage*damageFactor, damageType); } 
            return;}
        
        if (obj.name.Equals("vision")){
            if(1<<collider.gameObject.layer == (1 << collider.gameObject.layer & enemies)) {aims.Add(collider.gameObject); ChangeAim();}
            return;
        }
    } 
    public void OnColliderExit(GameObject obj, Collider collider){
        if(obj.name=="vision" && (1<<collider.gameObject.layer == (1 << collider.gameObject.layer & enemies))){
            aims.Remove(collider.gameObject);
            if(aim==collider.gameObject)ChangeAim();
        }
    }
}
