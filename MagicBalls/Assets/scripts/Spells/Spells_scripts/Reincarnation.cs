using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reincarnation : SpellBase
{
    protected override IEnumerator core()
    {
        spells.animator.SetTrigger("cast1");
        yield return new WaitForSeconds(delay);
        if(!spells.GodMod){
            stats.CurMana-=ManaCost;
            CD=true;
            spells.CastSpell(CoolDown,"Reincarnation",()=>CD=false);
        }
        
        player.GetComponent<player_script>().reincarnation=true;
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
