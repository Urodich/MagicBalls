using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test_scene_load : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Load_script load;
    buffs_script buffs;
    [SerializeField] HUD_script HUD;
    bool isPaused=false;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<camera_script>().player=player;
        Instantiate(load);
        buffs = player.GetComponent<buffs_script>();

        HUD.exit=Exit;
        HUD.resume=Resume;
        HUD.restart=Restart;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){ //pause on Esc
            if(!isPaused) Pause();
            else Resume();
        }
    }
    void Resume(){
        isPaused=false;
        pauseMenu.SetActive(false);
        Time.timeScale=1;
    }
    void Pause(){
        isPaused=true;
        Time.timeScale=0;
        buffs.UpdateStatsText();
        pauseMenu.SetActive(true);
    }

    void Exit(){
        SceneManager.LoadScene("MainMenu");
    }
    void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
