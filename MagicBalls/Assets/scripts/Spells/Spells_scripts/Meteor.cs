using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : SpellBase
{
    protected override IEnumerator core()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, spells.ground)) yield break;

        yield return StartCoroutine(spells.VectorCast());
        meteor_script meteor = Instantiate(prefab,hit.point+new Vector3(0,4,0),Quaternion.LookRotation(spells.vectorCastDirection)).GetComponentInChildren<meteor_script>();
        meteor.direction=spells.vectorCastDirection;
        spells.animator.SetTrigger("cast4");
        if(!spells.GodMod){
            stats.CurMana-=ManaCost;
            CD=true;
            spells.CastSpell(20f,"Meteor",()=>CD=false);
        }
        
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
