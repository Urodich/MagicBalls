using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PostProcess_script : MonoBehaviour
{
    [SerializeField] Volume volume;
    Vignette vignette;
    public void Start(){
        vignette=(Vignette)volume.profile.components.Find((component)=>component.name=="Vignette(Clone)");
    }

    public void DieEffect(float speed){
        StartCoroutine(cast());
        IEnumerator cast(){
            float value=0;
            while (value<1){
                value+=speed;
                vignette.intensity.value = value;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

}
