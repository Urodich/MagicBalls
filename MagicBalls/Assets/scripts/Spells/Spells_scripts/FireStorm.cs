using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStorm : SpellBase
{
    bool casting =false;
    Coroutine fire;
    [SerializeField] float manaPerCast;
    protected override IEnumerator core()
    {
        spells.animator.SetTrigger("cast_global");
        spells.animator.SetBool("casting", true);
        spells.StopMoving(true);

        yield return new WaitForSeconds(delay);

        if(!spells.GodMod){
            stats.CurMana-=ManaCost;
            CD=true;
        }

        
        fire=StartCoroutine(damage());
        casting=true;
        yield return new WaitWhile(()=>Input.GetMouseButton(1) && casting);
        StopCoroutine(fire);
        casting=false;
        spells.StopMoving(false);
        spells.animator.SetBool("casting", false);
        if(!spells.GodMod)spells.CastSpell(CoolDown,"Fire Storm",()=>CD=false);
    
        IEnumerator damage(){
            while(true){
                if(!spells.GodMod){
                    stats.CurMana-=manaPerCast;
                    if (stats.CurMana<=0) {casting=false; break;}
                }
                Vector3 ofset=new Vector3(UnityEngine.Random.Range(-10,10),6,UnityEngine.Random.Range(-10,10));
                Instantiate(prefab, player.transform.position+ofset, new Quaternion());
                yield return new WaitForSeconds(.2f);
            }
        }
    }
    public override void Break(){
        base.Break();
        StopCoroutine(fire);
        spells.StopMoving(false);
        spells.animator.SetBool("casting", false);
        if(!spells.GodMod)spells.CastSpell(CoolDown,"Fire Storm",()=>CD=false);
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
