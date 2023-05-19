using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : SpellBase
{
    protected override IEnumerator core()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator core(int a)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, spells.ground)) yield break;
       
        yield return StartCoroutine(spells.VectorCast());
        
        spells.animator.SetTrigger("cast9");
        yield return new WaitForSeconds(delay);

        if(!spells.GodMod){
            stats.CurMana-=ManaCost*a;
            CD=true;
            spells.CastSpell(CoolDown*a,"Wind",()=>CD=false);
        }

        wind_script wind = Instantiate(prefab,hit.point,Quaternion.LookRotation(spells.vectorCastDirection)).GetComponent<wind_script>();
        wind.strength=a;
        wind.direction=spells.vectorCastDirection;
    }

    protected override IEnumerator core(int a, int b)
    {
        throw new System.NotImplementedException();
    }
}