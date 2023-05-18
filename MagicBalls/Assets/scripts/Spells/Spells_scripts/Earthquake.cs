using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : SpellBase
{
    [SerializeField] float damage;
    protected override IEnumerator core()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator core(int a)
    {
        spells.animator.SetTrigger("cast6");
        spells.StopMoving(true);
        yield return new WaitForSeconds(delay);
        spells.StopMoving(false);
        if(!spells.GodMod){
            stats.CurMana-=ManaCost*a;
            CD=true;
            spells.CastSpell(CoolDown*a,"Earthquake",()=>CD=false);
        }
        Play();
        prefab.GetComponent<ParticleSystem>().Play();
        float _damage = a*damage*buffs.GetStats(Stats.damage)*buffs.GetStats(Stats.physicalDamage);
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, 2*a, spells.enemies);
        foreach (Collider collider in colliders){
            if (collider.tag!="enemy") continue;
            unit_script enemy = collider.gameObject.GetComponent<unit_script>();
            if(enemy.isFlying) continue;
            enemy.Stun(0.5f*a);
            enemy.TakeDamage(_damage, DamageType.Physical);
        }
    }

    protected override IEnumerator core(int a, int b)
    {
        throw new System.NotImplementedException();
    }
}
