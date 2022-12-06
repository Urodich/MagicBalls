using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScript : MonoBehaviour
{
    FullScreenMode curMode;
    int width;
    int height;
    
    void Start(){
        curMode = getMode(PlayerPrefs.GetInt("FullScreenMode",0));
        width = PlayerPrefs.GetInt("width", 1920);
        height = PlayerPrefs.GetInt("height",1080);
    }
    public void Save(){
        PlayerPrefs.SetInt("FullScreenMode", (int)curMode);
        PlayerPrefs.SetInt("width", width);
        PlayerPrefs.SetInt("height", height);
    }
    public void Close(){
        loadPrefs();
        update();
    }
    public void ChangeResolution(int i){
        switch (i){
            case 0: {
                width=1920;
                height=1080;
                break;
            }
            case 1:{
                width=1366;
                height=768;
                break;
            }
            case 2:{
                width=2560;
                height=1440;
                break;
            }
            case 3:{
                width=3840;
                height=2160;
                break;
            }
        }
        update();
    }

    public void ChangeVolume(float i){

    }
    public void ChangeDisplayMode(int i){
        curMode=getMode(i);
        update();
    }
    FullScreenMode getMode(int i){
        switch (i){
            case 0: return FullScreenMode.FullScreenWindow;
            case 1: return FullScreenMode.MaximizedWindow;
            case 2: return FullScreenMode.Windowed;
            default: return FullScreenMode.FullScreenWindow;
        }

    }
    void update(){
        Screen.SetResolution(width,height,curMode);
    }
    void loadPrefs(){
        curMode = getMode(PlayerPrefs.GetInt("FullScreenMode",0));
        width = PlayerPrefs.GetInt("width", 1920);
        height = PlayerPrefs.GetInt("height",1080);
    }
}
