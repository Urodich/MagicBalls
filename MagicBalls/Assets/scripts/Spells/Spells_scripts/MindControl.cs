using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindControl : SpellBase
{
    protected override IEnumerator core()
    {
        if(spells.control.mouseTarget==null) yield break;

        spells.animator.SetTrigger("cast4");
        yield return new WaitForSeconds(delay);
        if(!spells.GodMod){
            stats.CurMana-=ManaCost;
            CD=true;
            spells.CastSpell(CoolDown,"Mind Control",()=>CD=false);
        }
        
        Debug.Log("mind control");

        enemy_script enemy=spells.control.mouseTarget.GetComponent<enemy_script>();
        enemy.enemies=spells.enemies;
        enemy.friends = spells.friends;
        enemy.aims.Clear();
        
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
