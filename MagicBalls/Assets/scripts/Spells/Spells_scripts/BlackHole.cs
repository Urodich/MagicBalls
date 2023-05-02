using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : SpellBase
{
    protected override IEnumerator core()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, spells.ground)) yield break;

        spells.animator.SetTrigger("cast6");

        if(!spells.GodMod){
            stats.CurMana-=ManaCost;
            CD=true;
            spells.CastSpell(50f,"Black Hole",()=>CD=false);
        }
        Debug.Log("black hole");
        
        Instantiate(prefab, hit.point, new Quaternion());
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
