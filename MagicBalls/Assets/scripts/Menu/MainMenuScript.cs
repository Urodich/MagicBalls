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
//////////////NAVIGATION//////////
    public void GoToScene(string name){
        SceneManager.LoadScene(name);
    }
    
    public void Exit(){
        Application.Quit();
    }
}
