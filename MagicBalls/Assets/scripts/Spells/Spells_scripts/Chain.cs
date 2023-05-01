using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : SpellBase
{
    protected override IEnumerator core()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator core(int a)
    {
        spells.animator.SetTrigger("cast8");
        yield return new WaitForSeconds(delay);
        if(!spells.GodMod){
            stats.CurMana-=ManaCost*a;
            CD=true;
            spells.CastSpell(3f,"Chain",()=>CD=false);
        }
        
        float radius = 2f;
        int steps=5+a*2;
        Vector3 oldPoint;
        ParticleSystem particleSystem;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, spells.ground)){
            particleSystem = Instantiate(prefab.GetComponent<ParticleSystem>(), gameObject.transform);
            StartCoroutine(ChainStep(hit.point, radius, null));

        }

        IEnumerator ChainStep(Vector3 position, float radius, Collider last){
            while(steps>0){
                bool hit=false;
                Collider[] colliders = Physics.OverlapSphere(position, radius, spells.enemies);
                foreach(Collider col in colliders){
                    if(col.tag!="enemy")continue;
                    if(col==last) continue;
                    colliders=null;
                    ParticleSystem.EmitParams param = new ParticleSystem.EmitParams();
                    param.position = col.transform.position;
                    particleSystem.Emit(param, 1);
                    //particleSystem.Emit(col.transform.position, Vector3.zero, 0, 2f,Color.white);
                    unit_script enemy = col.GetComponent<unit_script>();
                    enemy.TakeDamage(15*buffs.GetStats(Stats.thunderDamage)*buffs.GetStats(Stats.damage), DamageType.Thunder);
                    last=col;
                    hit=true;
                    steps--;
                    oldPoint=col.gameObject.transform.position;
                    break;
                }
                if(!hit) break;
                yield return new WaitForSeconds(.1f);
            }
            Destroy(particleSystem.gameObject,2f);
        }
    }

    protected override IEnumerator core(int a, int b)
    {
        throw new System.NotImplementedException();
    }
}
