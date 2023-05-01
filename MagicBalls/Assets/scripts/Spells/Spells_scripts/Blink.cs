using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : SpellBase
{
    [SerializeField] float damage;
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
        Vector3 mousePos;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, spells.ground)){
            spells.animator.SetTrigger("jump");
        }
        yield return new WaitForSeconds(delay);
        if(!spells.GodMod){
            stats.CurMana-=ManaCost*b;
            CD=true;
            spells.CastSpell(CoolDown,"Blink",()=>CD=false);
        }
        Debug.Log("blink");
        mousePos= hit.point;
        navMesh.enabled=false;
        player.transform.position=mousePos;
        navMesh.enabled=true;
        navMesh.destination=mousePos;
        prefab.GetComponent<ParticleSystem>().Play();
        float _damage = b*damage*buffs.GetStats(Stats.thunderDamage)*buffs.GetStats(Stats.damage);

        Collider[] colliders = Physics.OverlapCapsule(player.transform.position + new Vector3(0,1,0), player.transform.position, 2*a, spells.enemies.value);
        foreach (Collider collider in colliders){
            if (collider.tag!="enemy") continue;
            unit_script enemy = collider.gameObject.GetComponent<unit_script>();
            if(enemy.isFlying) continue;
            enemy.Stun(0.5f*a);
            enemy.TakeDamage(_damage, DamageType.Thunder);
        }
    }
}
