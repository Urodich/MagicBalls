using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : SpellBase
{
    protected override IEnumerator core()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator core(int a)
    {
        spells.animator.SetTrigger("cast9");
        yield return new WaitForSeconds(delay);
        if (!spells.GodMod){
            stats.CurMana-=ManaCost*a;
            CD=true;
            spells.CastSpell(CoolDown*a,"Wave",()=>CD=false);
        }
        spells.projectileCast((int)buffs.GetStats(Stats.projectiles),prefab, a);
    }

    protected override IEnumerator core(int a, int b)
    {
        throw new System.NotImplementedException();
    }
}
