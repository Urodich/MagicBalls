using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombie : enemy_script
{
    
    [SerializeField] Transform attackPoint;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        aim.GetComponent<unit_script>().dieEvent+=(GameObject gameObject)=>Destroy(this.gameObject);
        aim.GetComponent<unit_script>().ChangeSpeed(-.1f);
        dieEvent+=(GameObject)=>aim.GetComponent<unit_script>().ChangeSpeed(.1f);
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isStunned) return;

        transform.LookAt(aim.transform);
        //атаки и передвижение
        if ((aim.transform.position-gameObject.transform.position).sqrMagnitude>attackDistance*attackDistance) {
            if(attacking){
                StopCoroutine(curAction);
                attacking=false;
                navMesh.isStopped=false;
            } 
            navMesh.destination = aim.transform.position;
            return;
        }
        else       
            if(!attacking) Attack();
    }
    public void SetZombieAim(GameObject aim){
        this.aim=aim;
    }
    void Attack(){
        navMesh.isStopped=true;
        attacking =true;
        curAction =StartCoroutine(AttackDelay());
    }
    //Attacking function
    IEnumerator AttackDelay(){
        yield return new WaitForSeconds(attackDelay); //delay
        Physics.OverlapSphere(attackPoint.position, .5f, enemies);
        yield return new WaitForSeconds(attackSpeed); //residual animation  
        navMesh.isStopped=false;
        attacking=false;
    }
}
