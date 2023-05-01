using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class unit_script : MonoBehaviour
{
    [SerializeField] protected UnityEngine.UI.Slider hpBar;
    [SerializeField] protected float maxHp;
    protected float maxHpFactor=1;
    protected float curHp;
    public float CurHp{get{return curHp;} set{curHp=value>maxHp?maxHp:value; if(hpBar!=null) hpBar.value=curHp;}}
    [SerializeField] protected float hpRegen;
    protected float hpRegenFactor=1;
    
    [SerializeField] public float speed;
    [SerializeField] protected float minSpeed;
    public float armor;
    public event Action<GameObject> dieEvent;
    protected float speedFactor=1;
    int stun;
    protected List<Effect> Effects = new List<Effect>();
    public int Priority=0; 
    //Сопротивления
    public float FireResist=0;
    public float ThunderResist=0;
    public float PhysicalResist=0;
    public float damageFactor=1;

    public bool isFlying=false;
    public bool isStunned=false;
    public bool isGrounded=true;
    [SerializeField]LayerMask ground;
    protected Vector3 startAim=Vector3.zero;

    Material mainMaterial;
    Material defaultMaterial;
    //[SerializeField] Material damageMaterial;
    SkinnedMeshRenderer ModelRenderer;
    new MeshRenderer renderer;
    [SerializeField] GameObject model;
    [SerializeField] public Animator animator;
    protected NavMeshAgent navMesh;
    Rigidbody rb;
    bool disableGround=false;
    protected Coroutine curAction;
    public bool isDead=false;
    protected Collider collider;
    void Awake(){
        collider=GetComponent<Collider>();
    }
    public void Start(){
        
        navMesh=gameObject.GetComponent<NavMeshAgent>();
        navMesh.speed=speed;
        if(startAim!=Vector3.zero) {
            navMesh.destination=startAim;
            animator.SetBool("run", true);
        }
        if(hpBar!=null) hpBar.maxValue=maxHp;
        CurHp=maxHp;
        if(model==null) return;
        ModelRenderer = model.GetComponentInChildren<SkinnedMeshRenderer>();
        if(ModelRenderer!=null){
            mainMaterial = ModelRenderer.material;
            model=ModelRenderer.gameObject;
        }
        else {
            renderer = gameObject.GetComponentInChildren<MeshRenderer>();
            mainMaterial = renderer.material;
            model=renderer.gameObject;
        } //delete this

        rb = GetComponent<Rigidbody>();
        defaultMaterial=mainMaterial;
        mainMaterial.DisableKeyword("HOVERED");
        mainMaterial.DisableKeyword("DAMAGED");
    }
    public void FixedUpdate(){
        if(isDead) return;
        //is grounded
        if(!disableGround){
            Ray ray = new Ray(transform.position, Vector3.down);
            if(Physics.Raycast(ray, out RaycastHit hit, 0.1f, ground)){
                isGrounded=true;
            }
        }
        if(CurHp<maxHp)CurHp+=hpRegen*hpRegenFactor*Time.fixedDeltaTime;
        if(CurHp>maxHp) CurHp=maxHp;
        //Stunned
        if(isGrounded){
            navMesh.enabled=true;
            rb.useGravity=false;
            if(stun>0){
                isStunned=true;
                navMesh.isStopped=true;
                if(animator)animator.SetBool("stunned",true);
            }
            else{
                isStunned=false;
                navMesh.isStopped=false; 
                animator.SetBool("stunned",false);
            }
        }
        else{
            isStunned=true;
            if(animator)animator.SetBool("falling",true);
        }
    }
    public virtual void ChangeSpeed(float value){
        speedFactor+=value;
        navMesh.speed=((speed*speedFactor)<minSpeed?minSpeed:speed*speedFactor);
    }
    public void PosDispell(){
        foreach(Effect effect in Effects){
            if (effect.isPositive) effect.Dispell();
        }
    }
    public void NegDispell(){
        foreach(Effect effect in Effects){
            if (!effect.isPositive) effect.Dispell();
        }
    }
    
    [SerializeField] ParticleSystem _stun;
    public virtual void Stun(float time){
        if(curAction!=null)StopCoroutine(curAction);
        _stun.Play();
        curAction=null;
        stun+=1;
        Effect eff=gameObject.AddComponent<Effect>();
        Effects.Add(eff.Set(time,()=>{stun-=1; Effects.Remove(eff); _stun.Stop();},false));

    }
    public void AddEffect(Action action,Action endAction,float time, bool isPositive){
        action();
        Effect eff=gameObject.AddComponent<Effect>();
        Effects.Add(eff.Set(time,()=>{endAction(); Effects.Remove(eff);},isPositive));
    }

    public virtual void TakeDamage(float damage, DamageType type){
        if (isDead) return;
        mainMaterial.EnableKeyword("DAMAGED");
        //if(animator)animator.SetTrigger("hit");
        float dameg = type switch{
            DamageType.Fire => damage * (1 - (FireResist>1?1:FireResist)),
            DamageType.Thunder => damage * (1 - (ThunderResist>1?1:ThunderResist)),
            DamageType.Physical => damage * (1 - (PhysicalResist>1?1:PhysicalResist))
        };
        if(armor>0){
            armor-=dameg;
            if(armor<0){CurHp+=armor; armor=0;}}
        else 
            CurHp -=dameg;
        if(CurHp<=0) Die();
        else{
            Invoke("resetMaterial",0.05f);
        }
    }
    void resetMaterial(){mainMaterial.DisableKeyword("DAMAGED");}
    public virtual void Die(){
        isDead=true;
        if(animator)animator.SetBool("die",true);
        collider.enabled=false;
        CallDieEvent(gameObject);
        //Stun(1);
        Destroy(gameObject,1f);
    }

    protected void CallDieEvent(GameObject g){
        Debug.Log(gameObject.name+"Die Event");
        if(dieEvent!=null)dieEvent(g);
        
    }

    /////////////////////////////////
    //LOGIC

    public void AddHP(float value){
        CurHp+=value;
    }
    
    public void SetInactive(bool idleAnim){
        
    }
    //Move2 curren
    public void Move(Vector3 direction, float power, float time, bool Stunned){
        if(Vector3.Dot(direction, Vector3.up)>0){
            isGrounded=false;
            disableGround=true;
            navMesh.enabled=false;
        }
        if (Stunned) {
            Stun(time);
        }
        rb.isKinematic = false;
        rb.AddForce(direction*power,ForceMode.Impulse);
        StartCoroutine(remove());
        
        IEnumerator remove(){
            yield return new WaitForSeconds(time/2);
            rb.useGravity=true;
            disableGround=false;
            yield return new WaitForSeconds(time/2);
            yield return new WaitUntil(()=>isGrounded);
            rb.isKinematic=true;
            navMesh.enabled=true;
            rb.useGravity=false;
        }
    }

    /////////////////////////////////
    //UI
    public void Lighting(bool value){
        if (mainMaterial!=defaultMaterial)return;
        if(value){
            model.layer=14;}
        else{
            model.layer=0;}
    }

    protected Material ChangeMaterial(Material material){
        Lighting(false);
        Material tmp=mainMaterial;
        if (ModelRenderer)ModelRenderer.material=material;
        else renderer.material=material;
        return tmp;
    }
}

public enum DamageType{
    Fire,
    Thunder,
    Physical
}