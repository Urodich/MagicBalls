using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : SpellBase
{
    [SerializeField] float _heal;
    protected override IEnumerator core()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator core(int a)
    {
        spells.animator.SetTrigger("cast1");
        yield return new WaitForSeconds(delay);
        if(!spells.GodMod){
            stats.CurMana-=ManaCost*a;
            CD=true;
            spells.CastSpell(CoolDown*a,"Wisp",()=>CD=false);
        }
        StartCoroutine(heal(5*a));

        IEnumerator heal(float time){
            while(time>0){
                yield return new WaitForSeconds(.5f);
                player.GetComponent<player_script>().AddHP(_heal*a);
                time-=.5f;
            }
        }
    }

    protected override IEnumerator core(int a, int b)
    {
        throw new System.NotImplementedException();
    }
}
