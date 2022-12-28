using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MageEnemy_script : unit_script, IEnemy
{
    [SerializeField] float attackDistance=4f;
    [SerializeField] float attackDelay=1f;
    [SerializeField] float attackSpeed=1f;
    [SerializeField] float blinkRange=2f;

    [SerializeField] DamageType damageType;
    [SerializeField] Collider visibleCol;

    public List<GameObject> aims;
    public GameObject aim;
    bool attacking=false;
    public LayerMask enemies;
    [SerializeField] LayerMask spells;

    //SPELLS
    float blinkCD=0;
    float defCD=0;
    float attackCD=0;
    [SerializeField] float BlinkCD;
    [SerializeField] float DefCD;
    new void Start()
    {
        base.Start();
        aims = new List<GameObject>();
    }

    [SerializeField]float fearRange=5;
    new void FixedUpdate()
    {
        blinkCD-=(blinkCD>0)?Time.fixedDeltaTime:0;
        defCD-=(defCD>0)?Time.fixedDeltaTime:0;
        attackCD-=(attackCD>0)?Time.fixedDeltaTime:0;
        base.FixedUpdate();
        if (isStunned) return;
        if (!aim) 
            if((transform.position-navMesh.destination).sqrMagnitude<0.5f) {animator.SetBool("run", false); return;}
            else return;
        transform.LookAt(aim.transform);
        if (curAction==null){
            if (blinkCD<=0 && (aim.transform.position-transform.position).sqrMagnitude<fearRange*fearRange) {animator.SetBool("run", false); Blink(aim.transform.position); return;}
            if (attackCD<=0 && (aim.transform.position-transform.position).sqrMagnitude<=attackDistance*attackDistance) {animator.SetBool("run", false); Attack(aim.transform.position); return;}
            if ((aim.transform.position-transform.position).sqrMagnitude>attackDistance){
                animator.SetBool("run", true);
                navMesh.destination=aim.transform.position;
            }
        }
    }
    [SerializeField] ParticleSystem _blink;
    void Blink(Vector3 aim){
        curAction=StartCoroutine(blink());
        Debug.Log("blink");
        IEnumerator blink(){
            yield return new WaitForSeconds(0.3f);
            Destroy(Instantiate(_blink,transform.position, new Quaternion()).gameObject,1f);
            yield return new WaitForSeconds(0.2f);
            Vector3 direction = (transform.position=aim);
            direction=Vector3.Scale(direction, new Vector3(1,0,1));
            transform.Translate(direction*blinkRange);
            blinkCD=BlinkCD;
            curAction=null;
        }
        
    }
    [SerializeField] bomb_script _bomb;
    void Attack(Vector3 pos){
        curAction=StartCoroutine(attack());
        Debug.Log("attack");
        IEnumerator attack(){
            attacking=true;
            animator.SetTrigger("attack");
            yield return new WaitForSeconds(attackDelay);
            Instantiate(_bomb, pos, new Quaternion()).Esplose(damage,damageType,enemies);
            attackCD=attackSpeed;
            yield return new WaitForSeconds(0.3f);
            attacking=false;
            curAction=null;
    }
    }
    
    [SerializeField] ParticleSystem defSpell;
    [SerializeField] ParticleSystem deadProject;
    void Defend(){
        curAction=StartCoroutine(def());
        Debug.Log("defend");
        IEnumerator def(){
            Destroy(Instantiate(defSpell, transform).gameObject,1);
            yield return new WaitForSeconds(0.1f);
            Collider[] proj=Physics.OverlapSphere(transform.position, 3.5f, spells);
            foreach (Collider i in proj){
                Destroy(i.gameObject);
                Destroy(Instantiate(deadProject, i.gameObject.transform.position, new Quaternion()).gameObject,1);
            }
            defCD=DefCD;
            curAction=null;
        }
    }
    public void ClearAims()
    {
        aims.Clear();
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

    public void OnColliderEnter(GameObject obj, Collider collider){
        if (obj.name.Equals("vision")){
            if(1<<collider.gameObject.layer == (1 << collider.gameObject.layer & enemies)) {aims.Add(collider.gameObject); ChangeAim();}
            return;
        }
        if(obj.name.Equals("defend")){
            Debug.Log("def_col");
            if(1<<collider.gameObject.layer == (1 << collider.gameObject.layer & spells) && curAction==null && defCD<=0) Defend();
            
        }
    } 
    public void OnColliderExit(GameObject obj, Collider collider){
        if(obj.name=="vision" && (1<<collider.gameObject.layer == (1 << collider.gameObject.layer & enemies))){
            aims.Remove(collider.gameObject);
            if(aim==collider.gameObject)ChangeAim();
        }
    }

}
