using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Audio;

[Serializable]
public struct StatsCount{
    public Stats stat;
    public float value;
}
[System.Serializable]
public struct wave{
    public enemys[] value;
    public float scale;
}
[System.Serializable]
public struct enemys{
    public enemy_script type;
    public int count;
    public int points;
}
public struct boost_str{
    public string description;
    public Sprite image;
    public Stats stats;
    public float value;
    public boost_str(Stats stats, string _image, string description, float value){
        image=Resources.Load<Sprite>(_image);
        this.stats=stats;
        this.description=description;
        this.value=value;
    }
}
[Serializable]
public struct DialogeLine{
    public string text;
    public int time;
    public UnityEvent action;
    public AudioClip clip;
}