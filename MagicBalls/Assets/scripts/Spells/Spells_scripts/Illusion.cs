using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Illusion : SpellBase
{
    protected override IEnumerator core()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, spells.ground)) yield break;

        spells.animator.SetTrigger("cast1");
        yield return new WaitForSeconds(delay);
        
        if(!spells.GodMod){
            stats.CurMana-=ManaCost;
            CD=true;
            spells.CastSpell(CoolDown,"Illusion",()=>CD=false);
        }
        
        Instantiate(prefab, player.transform.position+player.transform.forward, player.transform.rotation).GetComponent<illusion_script>().destination=hit.point;
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
