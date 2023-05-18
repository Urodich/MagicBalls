using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSound_script : MonoBehaviour
{
    [SerializeField] AudioSource step, attack, damage, die;
    public void Die(){
        if(die==null || die.clip==null) return;
        die.pitch=Random.Range(0.8f, 1.2f);
        die.Play();
    }
    public void Walk(bool value){
        if(step==null || step.clip==null) return;
        if(value) {if(!step.isPlaying)step.Play();}
        else step.Stop();
    }
    public void Damaged(){
        if(damage==null || damage.clip==null || damage.isPlaying) return;
        damage.pitch=Random.Range(0.8f, 1.2f);
        damage.Play();
    }
    public void Attack(){
        if(attack==null || attack.clip==null) return;
        attack.pitch=Random.Range(0.8f, 1.2f);
        attack.Play();
    }
}
