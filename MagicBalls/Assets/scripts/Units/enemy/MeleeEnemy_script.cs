public class MeleeEnemy_script : enemy_script
{
    protected new void FixedUpdate()
    {
        if(!isActive) return;
        base.FixedUpdate();
        
        if(isStunned) return;
        if (!aim) {animator.SetBool("run", false); return;}

        transform.LookAt(aim.transform);
        //атаки и передвижение
        if ((aim.transform.position-gameObject.transform.position).sqrMagnitude>attackDistance*attackDistance) {
            if(attacking && (aim.transform.position-gameObject.transform.position).sqrMagnitude>attackDistance*attackDistance*2){
                StopAttack();
                animator.SetTrigger("break");
            } 
            WalkTo(aim.transform.position);
            return;
        }
        else       
            StopWalking();
            if(!attacking) {Attack(); animator.SetTrigger("attack");}

        if(!attacking) ChangeAim();
        
    }
}
