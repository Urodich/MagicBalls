using System.Collections;
using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    public Action func;
    private float _timeLeft = 0f;
    Coroutine coroutine;
    [SerializeField] UnityEngine.UI.Text text;
    [SerializeField] UnityEngine.UI.Slider slider;
    [SerializeField] UnityEngine.UI.Image image;
    public Sprite SrcImage;
    
    IEnumerator StartTimer()
    {
        while (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
            if(text!=null)text.text=((int)_timeLeft).ToString();
            if(slider!=null)slider.value=_timeLeft;
            yield return null;
        }
        if(func!=null)func();
        coroutine=null;
        if(slider!=null)Destroy(gameObject);
    }
 
    public void SetTime(float time, bool isSpell)
    {
        if(isSpell){
            image.sprite=SrcImage;
            slider.maxValue=time;
        }
        _timeLeft = time;
        if(coroutine==null) coroutine = StartCoroutine(StartTimer());
    }
}
