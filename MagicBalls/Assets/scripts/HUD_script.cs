using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_script : MonoBehaviour
{
    // Start is called before the first frame update
    arena_script Arena;
    Spells_script spells;
    void Start()
    {
        Arena = GameObject.Find("ArenaScript").GetComponent<arena_script>();
        spells = GameObject.FindWithTag("Player").GetComponent<Spells_script>();
    }

    public void Resume(){
        Arena.Resume();
    }
    public void Restart(){
        Arena.Restart();
    }
    public void Exit(){
        Arena.Exit();
    }
    public void GodMod(bool value){
        spells.GodMod=value;
    }

}