using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : SpellBase
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
        if(!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, spells.ground)) yield break;
        spells.animator.SetTrigger("cast6");
        yield return new WaitForSeconds(delay);
        if(!spells.GodMod){
            stats.CurMana-=ManaCost*b;
            CD=true;
            spells.CastSpell(CoolDown*b,"Mud",()=>CD=false);
        }
        
        mud_script mud = Instantiate(prefab,hit.point,new Quaternion()).GetComponent<mud_script>();
        mud.earht = b;
        mud.water = a;
    }
}
