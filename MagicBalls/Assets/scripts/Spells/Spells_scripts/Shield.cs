using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : SpellBase
{
    [SerializeField]float time;
    protected override IEnumerator core()
    {
        navMesh.isStopped=true;

        spells.animator.SetTrigger("cast_global");
        yield return new WaitForSeconds(delay);
        if(!spells.GodMod) stats.CurMana-=ManaCost;
        
        GameObject _shield=Instantiate(prefab,transform.position, new Quaternion());
        CD=true;
        if(!spells.GodMod)spells.CastSpell(CoolDown,"Shield",()=>CD=false);
        Destroy(_shield, time);
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
