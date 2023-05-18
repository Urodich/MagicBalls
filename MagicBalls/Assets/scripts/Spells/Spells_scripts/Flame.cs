using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : SpellBase
{
    [SerializeField] float damage;
    protected override IEnumerator core()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator core(int a)
    {

        spells.StopMoving(true);

        spells.animator.SetTrigger("cast_flame");
        spells.animator.SetBool("casting", true);
        yield return new WaitForSeconds(delay);
        if(audio)audio.loop=true;
        Play();
        float _damage = damage*buffs.GetStats(Stats.fireDamage)*buffs.GetStats(Stats.damage)*a;
        CD=true;
        prefab.GetComponent<ParticleSystem>().Play();
        while(Input.GetMouseButton(1)){
            if(stats.CurMana<ManaCost) break;
            stats.CurMana-=ManaCost;
            Collider[] colliders = Physics.OverlapSphere(player.transform.position+player.transform.forward, 1.5f*a, spells.enemies);
            foreach(Collider enemy in colliders){
                enemy.GetComponent<unit_script>().TakeDamage(_damage, DamageType.Fire);
            }
            yield return new WaitForSeconds(.5f);
        }
        spells.StopMoving(false);
        prefab.GetComponent<ParticleSystem>().Stop();
        if(audio){audio.Stop();
            audio.loop=false;}
        spells.animator.SetBool("casting", false);
        if(!spells.GodMod) spells.CastSpell(CoolDown,"Flame",()=>CD=false);
        
    }

    protected override IEnumerator core(int a, int b)
    {
        throw new System.NotImplementedException();
    }

    public override void Break()
    {
        base.Break();
        if(audio){audio.Stop();
            audio.loop=false;}
        prefab.GetComponent<ParticleSystem>().Stop();
        if(!spells.GodMod) spells.CastSpell(CoolDown,"Flame",()=>CD=false);
    }
}
