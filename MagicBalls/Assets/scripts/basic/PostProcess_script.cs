using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PostProcess_script : MonoBehaviour
{
    [SerializeField] Volume volume;
    Vignette vignette;
    ColorAdjustments colorAdjustments;
    HUD_script hud;
    public void Start(){
        vignette=(Vignette)volume.profile.components.Find((component)=>component.name=="Vignette(Clone)");
        colorAdjustments=(ColorAdjustments)volume.profile.components.Find((component)=>component.name=="ColorAdjustments(Clone)");
        hud=GameObject.Find("HUD(Clone)").GetComponent<HUD_script>();
    }

    public void SetColor(Color color, float time){
        if(hud==null) return;
        hud.FrontImage.SetActive(true);
        Image image = hud.FrontImage.GetComponent<Image>();

        StartCoroutine(core());

        IEnumerator core(){
            image.color=color;
            for (float i=0;i<time;i+=Time.fixedDeltaTime){
                color.a = Mathf.Lerp(0,time,i);
                image.color=color;
                yield return new WaitForFixedUpdate();
            }
        }
    }
    public void DeleteColor(){
        if(hud==null) return;
        hud.FrontImage.SetActive(false);
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
