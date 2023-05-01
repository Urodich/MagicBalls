using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torrent : SpellBase
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, spells.ground)) yield return null;
        
        spells.animator.SetTrigger("cast1");  

        yield return new WaitForSeconds(delay);
        if(!spells.GodMod){
            stats.CurMana-=ManaCost*b;
            CD=true;
            spells.CastSpell(CoolDown*a,"Torrent",()=>CD=false);
        }
        torrent_script torrent = Instantiate(prefab, hit.point, new Quaternion()).GetComponent<torrent_script>();
        torrent.air=a;
        torrent.water=b;
    }
}
