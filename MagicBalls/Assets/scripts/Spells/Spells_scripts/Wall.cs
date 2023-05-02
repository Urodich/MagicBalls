using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : SpellBase
{
    [SerializeField] float time;
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
        if(!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, spells.ground)) yield break;

        spells.animator.SetTrigger("cast1");
        yield return new WaitForSeconds(delay);

        if(!spells.GodMod){
            stats.CurMana-=ManaCost;
            CD=true;
            spells.CastSpell(CoolDown,"Wall",()=>CD=false);
        }
        
        Transform tf = Instantiate(prefab, hit.point, new Quaternion()).transform;
        tf.LookAt(gameObject.transform);
        tf.eulerAngles = new Vector3(0,tf.eulerAngles.y, 0);
        Destroy(tf.gameObject, time);
    }
}
