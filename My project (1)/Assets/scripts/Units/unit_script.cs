using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class unit_script : MonoBehaviour
{
    [SerializeField]protected UnityEngine.UI.Slider hpBar;
    [SerializeField] protected float maxHp;
    protected float maxHpFactor=1;
    protected float curHp;
    public float CurHp{get{return curHp;} set{curHp=value>maxHp?maxHp:value; if(hpBar!=null) hpBar.value=curHp;}}
    [SerializeField] protected float hpRegen;
    protected float hpRegenFactor=1;
    [SerializeField] protected float damage;
    [SerializeField] public float speed;
    [SerializeField] protected float minSpeed;
    public float armor;
    public event Action<GameObject> dieEvent;
    protected float speedFactor=1;
    Timer stun;
    
    public int Priority=0; 
    //Сопротивления
    public float FireResist=0;
    public float ThunderResist=0;
    public float PhysicalResist=0;
    public float damageFactor=1;

    public bool isFlying=false;
    public bool isStunned=false;
    protected Vector3 startAim=Vector3.zero;

    Material mainMaterial;
    //[SerializeField] Material damageMaterial;
    Color main;
    SkinnedMeshRenderer ModelRenderer;
    new MeshRenderer renderer;
    [SerializeField] GameObject model;
    [SerializeField] public Animator animator;
    protected NavMeshAgent navMesh;

    public void Start(){
        navMesh=gameObject.GetComponent<NavMeshAgent>();
        navMesh.speed=speed;
        if(startAim!=Vector3.zero) navMesh.destination=startAim;
        if(hpBar!=null) hpBar.maxValue=maxHp;
        CurHp=maxHp;
        ModelRenderer = model.GetComponentInChildren<SkinnedMeshRenderer>();
        if(ModelRenderer!=null)mainMaterial = ModelRenderer.material;
        else {renderer = gameObject.GetComponentInChildren<MeshRenderer>();mainMaterial = renderer.material;} //delete this
        
        StartCoroutine(regen());
        mainMaterial.DisableKeyword("HOVERED");
        mainMaterial.DisableKeyword("DAMAGED");
    }
    public void SetAim(Vector3 pos){
        startAim=pos;
    }
    IEnumerator regen(){
        while (true){
            if(CurHp<maxHp)CurHp+=hpRegen*hpRegenFactor;
            if(CurHp>maxHp) CurHp=maxHp;
            yield return new WaitForSeconds(1f);
        }
    }
    public void AddHP(float value){
        CurHp+=value;
    }
    public void ChangeSpeed(float value){
        speedFactor+=value;
        navMesh.speed=((speed*speedFactor)<minSpeed?minSpeed:speed*speedFactor);
    }
    //работает как говно не трогать
    Coroutine cor=null;
    public void Move(Vector3 direction, float power, float time, bool Stunned){
        if(Stunned) Stun(time);
        if(cor==null) cor=StartCoroutine(move(direction, power, time));
        else {
            StopCoroutine(cor);
            cor=StartCoroutine(move(direction, power, time));
        }
    }
    float? baseOffset=null;
    IEnumerator move(Vector3 direction, float power, float time){
        if(baseOffset==null)baseOffset = navMesh.baseOffset;
        float baseTime=time;
        while (time>0){
            if(direction.y>0) 
                if(time>baseTime/2)navMesh.baseOffset+=direction.y*power*Time.deltaTime;
                else navMesh.baseOffset-=direction.y*power*Time.deltaTime;
            gameObject.transform.position+=direction*power*Time.deltaTime;
            time -=Time.deltaTime;
            yield return null;
        }
        navMesh.baseOffset=(float)baseOffset;
        if(time<=0)baseOffset=null;
    }
    public virtual void Stun(float time){
        isStunned=true;
        navMesh.isStopped=true;
        if(animator)animator.SetBool("stunned",true);
        if (!stun) {
            stun = gameObject.AddComponent<Timer>();
            stun.func = ()=>{isStunned=false;navMesh.isStopped=false; animator.SetBool("stunned",false);};
            stun.SetTime(time, false);
        }
        else stun.SetTime(time, false);
    }

    public void TakeDamage(float damage, DamageType type){
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
        if(animator)animator.SetBool("die",true);
        Debug.Log(gameObject.name + " died");
        if(dieEvent!=null)dieEvent(gameObject);
        Stun(1);
        Destroy(gameObject,1f);
    }

    public void Lighting(bool value){
        if(value){
            Debug.Log("hover");
            mainMaterial.EnableKeyword("HOVERED");}
        else{
            Debug.Log("unhover");
            mainMaterial.DisableKeyword("HOVERED");}
    }

}

public enum DamageType{
    Fire,
    Thunder,
    Physical
}