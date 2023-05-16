using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
public class prologue_script : MonoBehaviour
{
    GameObject Boost_panel, pauseMenu, spheres_panel; 
    bool isPaused=false;
    buffs_script buffs;
    [SerializeField] Load_script load;
    [SerializeField] GameObject player;
    [SerializeField] HUD_script HUD;
    [SerializeField] PostProcess_script post;
    NavMeshAgent navMesh;
    Vector3 playerPos;
    [SerializeField] Transform ArenaSpawn;
    public void Dream(){
        StartCoroutine(core());
        IEnumerator core(){
            post.ScreenDarkening(true);
            yield return new WaitForSeconds(1f);
            playerPos=player.transform.position;
            navMesh.enabled=false;
            player.transform.position=ArenaSpawn.position;
            navMesh.enabled=true;
            spheres_panel.SetActive(true);
            player.GetComponent<playerControl_script>().block=false;
            post.ScreenDarkening(false);
            player.GetComponent<Spells_script>().GodMod=true;
        }
    }
    public void FromDream(){
        StartCoroutine(core());
        IEnumerator core(){
            post.ScreenDarkening(true);
            yield return new WaitForSeconds(1f);
            navMesh.enabled=false;
            player.transform.position=playerPos;
            navMesh.enabled=true;
            post.ScreenDarkening(false);
            player.GetComponent<Spells_script>().GodMod=false;
        }
    }
    [SerializeField] enemy_script ogre;
    [SerializeField] Transform ogreSpawn;
    public void Room1(){
        Instantiate(ogre, ogreSpawn);
    }

    public void Rescue(){

    }

    void Awake(){
        HUD=Instantiate(HUD, transform, true);
        player=Instantiate(player, transform.position, new Quaternion());
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<camera_script>().player=player;
        buffs = player.GetComponent<buffs_script>();
    }
    void Start(){
        HUD.exit+=Exit;
        HUD.resume+=Resume;
        HUD.restart+=Restart;

        navMesh=player.GetComponent<NavMeshAgent>();

        pauseMenu=HUD.PauseMenu;
        Boost_panel=HUD.BoostPanel;
        spheres_panel = HUD.transform.Find("spheres").gameObject;
        spheres_panel.SetActive(false);
        player.GetComponent<playerControl_script>().block=true;

        Instantiate(load);
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
    void Restart(){
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex);
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
    
}
