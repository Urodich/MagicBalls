using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void NewGame(){
        
    }
    public void ContinueGame(){

    }
    public void StartArena(int i){
        switch(i){
            case 0:{
                SceneManager.LoadScene("Arena1");
                break;
                }
        }
    }
    public void Exit(){
        Application.Quit();
    }
}
