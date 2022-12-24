using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyScript : unit_script, IEnemy
{
    [SerializeField] protected float attackDistance=4f;
    [SerializeField] protected float attackDelay=1f;
    [SerializeField] protected float attackSpeed=1f;

    public List<GameObject> aims;
    public GameObject aim;
    protected bool attacking=false;
    protected Coroutine attack;
    public LayerMask enemies;

    [SerializeField] LayerMask obstacles;
    new void Start()
    {
        base.Start();
        navMesh.speed=speed;
        aims = new List<GameObject>();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        
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

    public void OnColliderEnter(GameObject obj, Collider collider){
        if (obj.name.Equals("vision")){
            if(1<<collider.gameObject.layer == (1 << collider.gameObject.layer & enemies)) {aims.Add(collider.gameObject); ChangeAim();Debug.Log("add enemy");}
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
