using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

interface IUnit
{
    bool isStunned{get;set;}

    public float FireResist{get;set;}
    public float ThunderResist{get;set;}
    public float PhysicalResist{get;set;}

    [SerializeField] UnityEngine.UI.Slider hpBar{get;set;}
    [SerializeField] float maxHp{get;set;}
    float CurHp{get;set;}
    [SerializeField] float hpRegen{get;set;}
    float armor{get;set;}

    [SerializeField] float speed{get;set;}
    [SerializeField] float minSpeed{get;set;}

    event Action<GameObject> dieEvent;
    Timer stun {get;set;}
}
