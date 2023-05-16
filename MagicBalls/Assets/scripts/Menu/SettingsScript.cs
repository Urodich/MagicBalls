using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;
public class SettingsScript : MonoBehaviour
{
    
    [SerializeField] AudioMixerGroup mixer;
    
    UniversalRenderPipelineAsset currentPipe;

    public void Start(){
        loadPrefs();
        sync();
        update();
    }
#region /////Base/////////
    public void Save(){
        PlayerPrefs.SetInt("FullScreenMode", (int)curMode-1);
        PlayerPrefs.SetInt("width", width);
        PlayerPrefs.SetInt("height", height);

        PlayerPrefs.SetFloat("Master", Master);
        PlayerPrefs.SetFloat("Sounds", Sounds);
        PlayerPrefs.SetFloat("Music", Music);
        PlayerPrefs.SetFloat("Speech", Speech);

        PlayerPrefs.SetInt("Quality", Quality);
        PlayerPrefs.SetInt("msaa", msaa);
        PlayerPrefs.SetInt("shadows", shadows?1:0);
    }
    public void Close(){
        loadPrefs();
        update();
        sync();
    }
    void update(){
        Screen.SetResolution(width,height,curMode);

        mixer.audioMixer.SetFloat("Master", Master);
        mixer.audioMixer.SetFloat("Sounds", Sounds);
        mixer.audioMixer.SetFloat("Music", Music);
        mixer.audioMixer.SetFloat("Speech", Speech);

        QualitySettings.SetQualityLevel(Quality);
        
        currentPipe = (UniversalRenderPipelineAsset)QualitySettings.renderPipeline;
        currentPipe.msaaSampleCount=msaa;
        currentPipe.shadowDistance=shadows?50:0;
    }
    
    void loadPrefs(){
        curMode = getMode(PlayerPrefs.GetInt("FullScreenMode",0));
        width = PlayerPrefs.GetInt("width", 1920);
        height = PlayerPrefs.GetInt("height",1080);

        Master = PlayerPrefs.GetFloat("Master", 0);
        Sounds = PlayerPrefs.GetFloat("Sounds", 0);
        Music = PlayerPrefs.GetFloat("Music", 0);
        Speech = PlayerPrefs.GetFloat("Speech", 0);

        Quality = PlayerPrefs.GetInt("Quality", 3);
        msaa = PlayerPrefs.GetInt("msaa", 4);
        shadows = PlayerPrefs.GetInt("shadows", 1)==1;
    }
#endregion

#region //////////////MUSIC//////////
float Master,Sounds,Music,Speech;
    public void ChangeMasterVolume(float i){
        Master = Mathf.Lerp(-80, 0, i);
        mixer.audioMixer.SetFloat("Master", Master);
    }
    public void ChangeSoundVolume(float i){
        Sounds =  Mathf.Lerp(-80, 0, i);
        mixer.audioMixer.SetFloat("Sounds", Sounds);
    }
    public void ChangeMusicVolume(float i){
        Music = Mathf.Lerp(-80, 0, i);
        mixer.audioMixer.SetFloat("Music", Music);
    }
    public void ChangeSpeechVolume(float i){
        Speech = Mathf.Lerp(-80, 0, i);
        mixer.audioMixer.SetFloat("Speech", Speech);
    }
#endregion

#region ////////////SETINGS/////////
bool shadows;
int width,height,Quality,msaa;
FullScreenMode curMode;

    public void ChangeShadows(bool value){
        shadows=value;
        currentPipe.shadowDistance=shadows?50:0;
    }
    public void QualityLevel(int i){
        Quality=i;
        QualitySettings.SetQualityLevel(i);
        currentPipe = (UniversalRenderPipelineAsset)QualitySettings.renderPipeline;
    }
    public void MSAA(int i){
        switch(i){
            case 0: {msaa=0; break;}
            case 1: {msaa=2; break;}
            case 2: {msaa=4; break;}
            case 3: {msaa=8; break;}
        }
        //QualitySettings.antiAliasing=msaa;
        currentPipe.msaaSampleCount=msaa;
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
        Screen.SetResolution(width,height,curMode);
    }
    int getResolution(){
        switch (width){
            case 1920: return 0;
            case 1366: return 1;
            case 2560: return 2;
            case 3840: return 3;
            default: return 0;
        }
    }
    public void ChangeDisplayMode(int i){
        Debug.Log(i);
        curMode=getMode(i);
        Screen.SetResolution(width,height,curMode);
    }
    FullScreenMode getMode(int i){
        switch (i){
            case 0: return FullScreenMode.FullScreenWindow;
            case 1: return FullScreenMode.MaximizedWindow;
            case 2: return FullScreenMode.Windowed;
            default: return FullScreenMode.FullScreenWindow;
        }

    }
    
#endregion

#region ///////////UI//////////
    [SerializeField] TMP_Dropdown _display,_resolution,_msaa,_quality;
    [SerializeField] Slider _master, _music, _speech, _sounds;
    [SerializeField] Toggle _shadows;

    void sync(){
        _display.SetValueWithoutNotify((int)curMode-1);
        _resolution.SetValueWithoutNotify(getResolution());
        _msaa.SetValueWithoutNotify(msaa);
        _quality.SetValueWithoutNotify(Quality);

        _master.SetValueWithoutNotify(Mathf.InverseLerp(-80,0,Master));
        _music.SetValueWithoutNotify(Mathf.InverseLerp(-80,0,Music));
        _speech.SetValueWithoutNotify(Mathf.InverseLerp(-80,0,Speech));
        _sounds.SetValueWithoutNotify(Mathf.InverseLerp(-80,0,Sounds));

        _shadows.SetIsOnWithoutNotify(shadows);
    }
#endregion
}
