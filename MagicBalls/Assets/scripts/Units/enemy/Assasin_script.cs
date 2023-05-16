using UnityEngine;

public class Assasin_script : enemy_script
{
    [SerializeField] float blinkDistance=5;
    float blinkCD=0;
    float invisibilityCD=0;
    [SerializeField] float BlinkCD;
    [SerializeField] float InvisibilityCD;
    protected new void FixedUpdate(){
        blinkCD-=Time.fixedDeltaTime;
        if(blinkCD<0)blinkCD=0;
        invisibilityCD-=Time.fixedDeltaTime;
        if(invisibilityCD<0)invisibilityCD=0;

        if(!isActive) return;

        base.FixedUpdate();

        if(isStunned) return;
        if (!aim) {animator.SetBool("run", false); return;}

        transform.LookAt(aim.transform);
        //атаки и передвижение
        if ((aim.transform.position-gameObject.transform.position).sqrMagnitude>attackDistance*attackDistance) {
            if(attacking){
                StopAttack();
            } 
            if(!invisible && (aim.transform.position-gameObject.transform.position).sqrMagnitude<blinkDistance*blinkDistance && blinkCD<=0){
                blink(aim.transform.position-aim.transform.forward);
            }
            else{
                if(invisibilityCD<=0)invisibility();
                navMesh.destination = aim.transform.position;
                animator.SetBool("run", true);
                return;
            }
        }
        else       
            animator.SetBool("run", false);
            if(!attacking) {Attack(); animator.SetTrigger("attack"); resetMaterial();}

        if(!attacking) ChangeAim();
    }
    [SerializeField] ParticleSystem blinkPart;
    void blink(Vector3 pos){
        blinkCD=BlinkCD;
        navMesh.enabled=false;
        transform.position=pos;
        navMesh.enabled=true;
        transform.LookAt(aim.transform);
        Destroy(Instantiate<ParticleSystem>(blinkPart, transform.position, new Quaternion()).gameObject, 2);
    }
    [SerializeField] ParticleSystem invisPart;
    [SerializeField] Material invisMaterial;
    [SerializeField] float acceleration=0.5f;
    Material main;
    int layer=7;
    bool invisible=false;
    void invisibility(){
        layer=gameObject.layer;
        invisibilityCD=InvisibilityCD;
        invisible=true;
        Destroy(Instantiate<ParticleSystem>(invisPart, transform.position, new Quaternion()).gameObject, 2);
        gameObject.layer=0;
        main=ChangeMaterial(invisMaterial);
        ChangeSpeed(acceleration);
        Invoke("resetMaterial", 5);
    }
    void resetMaterial(){
        ChangeMaterial(main);
        ChangeSpeed(-acceleration);
        invisible=false;
        gameObject.layer=layer;
    }
}
