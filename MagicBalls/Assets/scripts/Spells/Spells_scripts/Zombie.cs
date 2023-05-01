using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : SpellBase
{
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
        if(spells.control.mouseTarget==null) yield return null;

        spells.animator.SetTrigger("cast4");
        yield return new WaitForSeconds(delay);
        if(!spells.GodMod){
            stats.CurMana-=ManaCost;
            CD=true;
            spells.CastSpell(15f,"Zombie",()=>CD=false);
        }
        Debug.Log("zombie");

        for(int i=0;i<2*a;i++){
            Instantiate(prefab, spells.control.mouseTarget.transform.position, new Quaternion()).GetComponent<zombie>().SetZombieAim(spells.control.mouseTarget);
        }
        
    }
}
