using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : SpellBase
{
    [SerializeField] float damage, speed;
    
    protected override IEnumerator core(int a, int b){
        damage*=buffs.GetStats(Stats.damage)*buffs.GetStats(Stats.fireDamage)*buffs.GetStats(Stats.projectileDamage)*a;
        spells.animator.SetTrigger("cast8");
        yield return new WaitForSeconds(delay);
        if(!spells.GodMod){
            stats.CurMana-=ManaCost;
            CD=true;
            spells.CastSpell(CoolDown, "FireBall", ()=>CD=false);
        }
        foreach (GameObject i in spells.projectileCast((int)buffs.GetStats(Stats.projectiles),prefab, 1)){
            fireball_script fb = i.GetComponent<fireball_script>();
            fb.damage *=damage;
            fb.speed *=speed*b;
        }
    }

    protected override IEnumerator core(int a)
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator core()
    {
        throw new System.NotImplementedException();
    }
}
