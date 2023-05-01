using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haste : SpellBase
{
    [SerializeField] float addSpeed;
    float value;
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
        if(!spells.GodMod){
            stats.CurMana-=ManaCost*a;
            CD=true;
            spells.CastSpell(CoolDown*b,"Haste",()=>CD=false);
        }
        yield return new WaitForSeconds(delay);
        value=addSpeed*a;
        player.GetComponent<unit_script>().ChangeSpeed(value);
        spells.animator.SetBool("haste",true);
        Invoke("Remove", 5*b);
    }
    void Remove(){player.GetComponent<unit_script>().ChangeSpeed(-value);spells.animator.SetBool("haste",false);}
}
