using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Effect : MonoBehaviour
{
    Action end;
    float _timeLeft = 0f;
    public bool isPositive;
    //bool isAddable;

    void FixedUpdate()
    {
        _timeLeft-=Time.fixedDeltaTime;
        if (_timeLeft<=0){
            Break();
        }
    }

    public void Break(){
        end();
        Destroy(this);
    }
    public void Dispell(){
       Destroy(this);
    }
    public Effect Set(float time, Action endAction, bool isPositive){
        _timeLeft=time;
        this.isPositive=isPositive;
        end=endAction;
        return this;
    }
}
