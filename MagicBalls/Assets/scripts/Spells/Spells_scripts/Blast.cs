using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : SpellBase
{
    [SerializeField] ParticleSystem explosion;
    [SerializeField] float damage;
    protected override IEnumerator core()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator core(int a)
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator core(int a, int b)
    {
        if(!spells.GodMod){
            stats.CurMana-=ManaCost;
            CD=true;}
        
        float time=0f;

        float _damage = damage*a*buffs.GetStats(Stats.fireDamage)*buffs.GetStats(Stats.damage);
        
        spells.animator.SetTrigger("cast_blast");
        spells.animator.SetBool("casting", true);
        spells.StopMoving(true);
        ParticleSystem bl = Instantiate(prefab,gameObject.transform).GetComponent<ParticleSystem>();
        while(Input.GetMouseButton(1)){
            time+=Time.deltaTime;
            if(time>=5f) break;
            yield return new WaitForEndOfFrame();
        }
        spells.StopMoving(false);
        Destroy(bl.gameObject);
        Destroy(Instantiate(explosion, gameObject.transform).gameObject,2);
        spells.animator.SetBool("casting", false);    
        Collider[] enemy = Physics.OverlapSphere(player.transform.position, 1f*b*time, spells.enemies);
        foreach(Collider elem in enemy){
            unit_script en=elem.gameObject.GetComponent<unit_script>();
            en.TakeDamage(_damage*a*buffs.GetStats(Stats.fireDamage)*buffs.GetStats(Stats.damage)*time, DamageType.Fire);
            en.Move(elem.gameObject.transform.position-player.transform.position, 2f*a*time, .5f, false);
        }
        if(time>=5 && !spells.GodMod){
            player_script player_=player.GetComponent<player_script>();
            player_.TakeDamage(_damage*a*buffs.GetStats(Stats.fireDamage)*buffs.GetStats(Stats.damage)*time, DamageType.Fire);
            player_.Stun(2f);
        }
        if(!spells.GodMod) spells.CastSpell(CoolDown,"Blast",()=>CD=false);
        
    }
}
