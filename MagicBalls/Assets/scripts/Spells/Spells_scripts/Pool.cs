using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : SpellBase
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
        yield return new WaitForSeconds(delay);

        if(!spells.GodMod){
            stats.CurMana-=ManaCost*((b-1)/2+1);
            CD=true;
            spells.CastSpell(CoolDown*a,"Pool",()=>CD=false);
        }
       
        electric_pool_script EPool = Instantiate(prefab, hit.point, new Quaternion()).GetComponent<electric_pool_script>();
        EPool.thunder=b;
        EPool.water=a;
        
    }
}
