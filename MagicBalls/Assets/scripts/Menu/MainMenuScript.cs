using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuScript : MonoBehaviour
{
    string datapath;
    string filename;
    [SerializeField] GameObject continueButton;
    void Start(){
        datapath = Application.dataPath + "/Saves";
        filename = "/" + Application.loadedLevel + ".xml";

        if(!Directory.Exists(datapath))
            Directory.CreateDirectory(datapath);
        if(Directory.GetFiles(datapath).Length>0) continueButton.SetActive(true);
        else continueButton.SetActive(false);
    }
    public void NewGame(){
        foreach (string file in Directory.GetFiles(datapath))
            File.Delete(file); 
        SceneManager.LoadScene("Prologue");
    }
    public void ContinueGame(){
        DirectoryInfo di = new DirectoryInfo(datapath);
        string str = di.GetFiles()[0].Name.Split('.')[0];
        Debug.Log(str);
        int number = int.Parse(str);
        SceneManager.LoadScene(number);
    }
//////////////NAVIGATION//////////
    public void GoToScene(string name){
        SceneManager.LoadScene(name);
    }
    
    public void Exit(){
        Application.Quit();
    }
}