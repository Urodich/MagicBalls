using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PostProcess_script : MonoBehaviour
{
    [SerializeField] Volume volume;
    Vignette vignette;
    ColorAdjustments colorAdjustments;
    public void Start(){
        vignette=(Vignette)volume.profile.components.Find((component)=>component.name=="Vignette(Clone)");
        colorAdjustments=(ColorAdjustments)volume.profile.components.Find((component)=>component.name=="ColorAdjustments(Clone)");
    }

    public void DieEffect(float speed){
        StartCoroutine(cast());
        IEnumerator cast(){
            float value=0;
            while (value<1){
                value+=speed;
                vignette.intensity.value = value;
                colorAdjustments.saturation.value=value*-100;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    Coroutine Dark;
    public void ScreenDarkening(bool dark){
        if(Dark!=null)StopCoroutine(Dark);
        Dark=StartCoroutine(cast(dark));
        IEnumerator cast(bool dark){
            float value=colorAdjustments.postExposure.value;
            if(dark)
                while (value>-8){
                    value-=0.5f;
                    colorAdjustments.postExposure.value=value;
                    yield return new WaitForSeconds(0.1f);
                }
            else
                while (value<0){
                    value+=0.5f;
                    colorAdjustments.postExposure.value=value;
                    yield return new WaitForSeconds(0.1f);
                }
            Dark=null;
        }
    }

}
