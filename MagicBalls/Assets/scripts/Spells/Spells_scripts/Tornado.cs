using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : SpellBase
{
    protected override IEnumerator core()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, spells.ground)) yield break;
        spells.animator.SetTrigger("cast9");
        yield return new WaitForSeconds(delay);
        if(!spells.GodMod){
            stats.CurMana-=ManaCost;
            CD=true;
            spells.CastSpell(CoolDown,"Tornado",()=>CD=false);
        }
        
        Debug.Log("Tornado");
        
        Instantiate(prefab, hit.point, player.transform.rotation).GetComponent<tornado_script>();
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
