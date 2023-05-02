using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaRegen : SpellBase
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
        spells.animator.SetBool("casting", true);
        spells.animator.SetTrigger("cast_blast");
        spells.StopMoving(true);
        yield return new WaitForSeconds(delay);
        prefab.GetComponent<ParticleSystem>().Play();
        player_script ps = player.GetComponent<player_script>();
        ps.manaRegen+=5f*a;
        float ownArmor=ps.armor;
        ps.armor+=15*b;
        yield return new WaitUntil(()=>Input.GetMouseButtonUp(1));
        ps.manaRegen-=5f*a;
        if(ps.armor>ownArmor) ps.armor=ownArmor;
        spells.StopMoving(false);
        spells.CastSpell(CoolDown,"Mana Regen",()=>CD=false);
        prefab.GetComponent<ParticleSystem>().Stop(true);
        spells.animator.SetBool("casting", false);
    }
    public override void Break(){
        base.Break();
        prefab.GetComponent<ParticleSystem>().Stop();
        spells.StopMoving(false);
        spells.CastSpell(CoolDown,"Mana Regen",()=>CD=false);
    }
}
