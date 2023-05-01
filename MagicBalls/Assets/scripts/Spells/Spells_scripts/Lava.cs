using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : SpellBase
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, spells.ground)) yield return null;
        spells.animator.SetTrigger("cast6");

        if(!spells.GodMod){
            stats.CurMana-=ManaCost*b;
            CD=true;
            spells.CastSpell(CoolDown*a,"Lava",()=>CD=false);
        }
        
        lava_script lava = Instantiate(prefab, hit.point, new Quaternion()).GetComponent<lava_script>();
        lava.fire=b;
        lava.earht=a;
    }
}
