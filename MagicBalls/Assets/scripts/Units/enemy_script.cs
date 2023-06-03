using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Diagnostics;

//TO DO
//low hp mobs go away
//patrol
//
public class enemy_script : unit_script, IEnemy
{
    [SerializeField] protected DamageType damageType;
    [SerializeField] protected float damage;
    [SerializeField] protected Collider visibleCol;
    [SerializeField] protected float attackDistance=4f;
    [SerializeField] protected float attackDelay=1f;
    [SerializeField] protected float attackSpeed=1f;

    public List<GameObject> aims = new List<GameObject>();
    [HideInInspector] public GameObject aim;
    protected bool attacking=false;
    //protected Coroutine attack;
    protected bool isActive=true;
    public LayerMask enemies;
    public LayerMask friends;
    

    public void SetStartAim(Vector3 pos){
        startAim=pos;
    }

    [SerializeField] Transform[] patrolPoints;
    protected virtual void Patrol(){
        StartCoroutine(patrol());
        IEnumerator patrol(){
            animator.SetBool("walk", true);
            if(patrolPoints.Length>1){          //has patrol path
                int i=0;
                while(true){
                    navMesh.SetDestination(patrolPoints[i++].position);
                    i=i==patrolPoints.Length?0:i;
                    yield return new WaitUntil(()=>navMesh.remainingDistance<2);
                }
            }
            else{                               //generate patrol path

            }
        }
    }

    public void Activate(bool value){
        if(value){
            isActive=true;
            animator.enabled=true;
            collider.enabled=true;
            hpBar.gameObject.SetActive(true);
        }
        else{
            isActive=false;
            animator.enabled=false;
            hpBar.gameObject.SetActive(false);
            collider.enabled=false;
        }
    }
    public override void Die()
    {
        base.Die();
        navMesh.Stop();
    }
    public void StrenghtScale(float multiply){
        maxHp*=multiply;
        damage*=multiply;
        speed*=multiply;
        armor+=5*multiply;
    }

    protected virtual void StopAttack(){
        if(curAction!=null)StopCoroutine(curAction);
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
        if(aims.Count==0) {aim=null; StopAttack(); return false;}
        if (aim==null) {aim = aims[0]; a=true;}
        foreach (GameObject i in aims){
            if(i==aim) continue;
            if(i.GetComponent<unit_script>().Priority>aim.GetComponent<unit_script>().Priority) {aim = i; a=true; continue;}
            if((i.transform.position-gameObject.transform.position).sqrMagnitude<(aim.transform.position-gameObject.transform.position).sqrMagnitude) {aim = i; a=true;}
        }
        return a;
    }
    public void FindAim(GameObject _aim){
        if(aims.Contains(_aim)) return;
        aims.Add(_aim);
        visibleCol.transform.localScale=new Vector3(2,1,2);
        _aim.GetComponent<unit_script>().dieEvent+=LostAim;
        ChangeAim();
        NotifyAllies(_aim);
    }
    void LostAim(GameObject _aim){
        if(this==null) return;
        if(aims.Contains(_aim))aims.Remove(_aim);
        if(aim==_aim){aim=null; ChangeAim();}
        visibleCol.transform.localScale=new Vector3(1,1,1);
    }
    protected void Attack(){
        navMesh.isStopped=true;
        attacking =true;
        curAction =StartCoroutine(AttackDelay());
    }
    //Attacking function
    protected virtual IEnumerator AttackDelay(){
        yield return new WaitForSeconds(attackDelay); //delay
        if(sound) sound.Attack();
        aim.gameObject.GetComponent<unit_script>().TakeDamage(damage*damageFactor, damageType);
        yield return new WaitForSeconds(attackSpeed); //residual animation  
        navMesh.isStopped=false;
        attacking=false;
    }
    protected virtual void NotifyAllies(GameObject _aim){
        Collider[] cols=Physics.OverlapSphere(transform.position, 10f, friends);
        if (cols == null) return;
        foreach(Collider col in cols){
            if(Utils.LayerComparer(col.gameObject, friends)&&col.gameObject.tag!="Player")
                col.gameObject.GetComponent<enemy_script>().FindAim(_aim);
        }

    }
    //Triggers
    public virtual void OnColliderEnter(GameObject obj, Collider collider){
        if (obj.name.Equals("vision")){
            if(Utils.LayerComparer(collider.gameObject, enemies))
                FindAim(collider.gameObject); 
            }
    } 
    public virtual void OnColliderExit(GameObject obj, Collider collider){
        if(obj.name=="vision" && Utils.LayerComparer(collider.gameObject, enemies)){
            LostAim(collider.gameObject);
        }
    }
}
