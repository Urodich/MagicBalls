using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drain : SpellBase
{
    [SerializeField] float damage=5;
    [SerializeField] float manaPerTick=5;
    [SerializeField] float radius=5;
    protected override IEnumerator core()
    {
        if(!spells.GodMod){
            stats.CurMana-=ManaCost;
            CD=true;
            spells.CastSpell(CoolDown,"Life Drain",()=>CD=false);
        }
       
        bool casting=true;
        float _damage=buffs.GetStats(Stats.damage)*buffs.GetStats(Stats.physicalDamage)*damage;

        spells.animator.SetBool("casting", true);
        spells.animator.SetTrigger("cast_blast");
        spells.StopMoving(true);
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius, spells.enemies);
        foreach (Collider i in colliders){
            if(i.tag!="enemy") continue;
            StartCoroutine(bloodTail(Instantiate(prefab, i.transform, false).GetComponent<ParticleSystem>()));
        }
        while(Input.GetMouseButton(1)){
            foreach (Collider i in colliders){
                if(i==null) continue;
                if(i.tag!="enemy") continue;
                unit_script enemy =  i.gameObject.GetComponent<unit_script>();
                enemy.TakeDamage(_damage, DamageType.Physical);
                stats.AddHP(_damage);
            }
            stats.CurMana-=manaPerTick;
            if(stats.CurMana<=0) break;
            yield return new WaitForSeconds(.5f);
        }
        spells.StopMoving(false);
        casting=false;
        spells.animator.SetBool("casting", false);
        
        

        IEnumerator bloodTail(ParticleSystem blood){
            while(casting && blood!=null){
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[blood.particleCount];
                blood.GetParticles(particles);
                for(int i=0;i<blood.particleCount;i++){
                    particles[i].velocity=(player.transform.position+Vector3.up*.5f-particles[i].position).normalized*2;
                    if((player.transform.position-particles[i].position).sqrMagnitude<.5f) particles[i].remainingLifetime=.1f;
                }
                blood.SetParticles(particles);
                yield return new WaitForSeconds(.2f);
            }
            if(blood!=null)blood.Stop(false);
        }
    }

    protected override IEnumerator core(int a)
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator core(int a, int b)
    {
        throw new System.NotImplementedException();
    }
}
