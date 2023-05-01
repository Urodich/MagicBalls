using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phoenix : SpellBase
{
    [SerializeField] float damage, time;
    GameObject phoen;
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
        spells.animator.SetTrigger("cast1");
        yield return new WaitForSeconds(delay);
        if(!spells.GodMod){
            stats.CurMana-=ManaCost;
            CD=true;
            spells.CastSpell(CoolDown,"Phoenix",()=>CD=false);
        }
        if(phoen) Destroy(phoen);
        if(prefab!=null) phoen = Instantiate(prefab);
        Debug.Log("phoenix");
        
        float _damage=damage*buffs.GetStats(Stats.damage)*buffs.GetStats(Stats.fireDamage)*b;
        StartCoroutine(Damage(a*time));

        IEnumerator Damage(float time){
            while (time>0)
            {
                Collider[] aims = Physics.OverlapSphere(gameObject.transform.position, 3, spells.enemies);
                foreach(Collider i in aims){
                    if(i.tag!="enemy") continue;
                    i.gameObject.GetComponent<unit_script>().TakeDamage(_damage, DamageType.Fire);
                }
                yield return new WaitForSeconds(0.5f);
                time-=.5f;
            }
        }
    }


}
