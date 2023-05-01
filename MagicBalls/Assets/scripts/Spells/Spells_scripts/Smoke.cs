using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : SpellBase
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
        
        spells.animator.SetTrigger("cast4");
        yield return new WaitForSeconds(delay);

        if(!spells.GodMod){
            stats.CurMana-=ManaCost;
            CD=true;
            spells.CastSpell(CoolDown,"Smoke",()=>CD=false);
        }
        
        smoke_script smoke=Instantiate(prefab, hit.point, new Quaternion()).GetComponent<smoke_script>();
        smoke.fire=a;
        smoke.water=b;
        
    }
}
