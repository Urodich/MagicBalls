using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : SpellBase
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
        if (!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, spells.ground)) yield break;
        spells.animator.SetTrigger("cast6");

        yield return new WaitForSeconds(delay);

        if (!spells.GodMod)
        {
            stats.CurMana -= ManaCost * b;
            CD = true;
            spells.CastSpell(CoolDown * a, "Golem", () => CD = false);
        }

        Golem_script golem = Instantiate(prefab, hit.point, new Quaternion()).GetComponent<Golem_script>();
    }
}
