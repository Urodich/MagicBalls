using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : SpellBase
{
    [SerializeField] float repulsion;
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
        spells.animator.SetTrigger("cast6");
        yield return new WaitForSeconds(delay);
        if(!spells.GodMod){
            stats.CurMana-=ManaCost;
            CD=true;
            spells.CastSpell(CoolDown,"Bird",()=>CD=false);
        }
        prefab.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(.6f);
        
        Collider[] enemy = Physics.OverlapSphere(player.transform.position, 3f*b, spells.enemies);
        foreach(Collider i in enemy){
            if(i.tag!="enemy") continue;
            unit_script unit = i.gameObject.GetComponent<unit_script>();
            unit.TakeDamage(1f,DamageType.Physical);
            Vector3 dir=(i.gameObject.transform.position-player.transform.position);
            dir.Scale(new Vector3(1,0,1));
            unit.Move(dir.normalized, repulsion*a*buffs.GetStats(Stats.repulsion), 1f, false);
        }
    }
}
