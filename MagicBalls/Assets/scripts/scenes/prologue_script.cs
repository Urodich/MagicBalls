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
    [SerializeField] Loader loader;
    [SerializeField] Load_script load;
    [SerializeField] GameObject player;
    [SerializeField] HUD_script HUD;
    [SerializeField] PostProcess_script post;
    NavMeshAgent navMesh;
    Vector3 playerPos = new Vector3(0,0,0);
    [SerializeField] Transform playerDefaultPos;
    [SerializeField] Transform ArenaSpawn;
    [SerializeField] Item[] playerStartEquipment;
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
            if(Vector3.Distance(playerPos, new Vector3(0,0,0))!=0) player.transform.position=playerPos;
            else player.transform.position=playerDefaultPos.position;
            navMesh.enabled=true;
            post.ScreenDarkening(false);
            player.GetComponent<Spells_script>().GodMod=false;
        }
    }
    [SerializeField] enemy_script ogre;
    [SerializeField] Transform ogreSpawn;
    public void Room1(room_activator room){
        room.AddEnemy( Instantiate(ogre, ogreSpawn.position, new Quaternion()));
    }

    [SerializeField] Transform[] EndSpawnPoints;
    [SerializeField] enemy_script[] enemyTypes;
    Coroutine end;
    public void Rescue(){
        end=StartCoroutine(ending());

        IEnumerator ending(){
            while(true){
                foreach(var elem in EndSpawnPoints)
                    Instantiate(enemyTypes[Random.Range(0,1)], elem.position, new Quaternion()).SetStartAim(player.transform.position);
                yield return new WaitForSeconds(1f);
            }
        }
    }
    public void End(){
        StopCoroutine(end);
        StartCoroutine(core());
        IEnumerator core(){
            post.SetColor(Color.white, 2);
            yield return new WaitForSeconds(2);
            player.GetComponent<Spells_script>().GodMod=true;
            yield return new WaitForSeconds(2);
            loader.Delete();
            SceneManager.LoadScene("MainMenu");
        }
    }

    void Awake(){
        HUD=Instantiate(HUD, transform, true);
        player=Instantiate(player, transform.position, new Quaternion());
        loader.player = player;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<camera_script>().player=player;
        buffs = player.GetComponent<buffs_script>();
    }
    void Start(){
        HUD.exit+=Exit;
        HUD.resume+=Resume;
        HUD.restart+=Restart;

        navMesh=player.GetComponent<NavMeshAgent>();

        pauseMenu=HUD.PauseMenu;
        spheres_panel = HUD.transform.Find("spheres").gameObject;
        pauseMenu.transform.Find("Toggle").gameObject.SetActive(false);
        Boost_panel=HUD.BoostPanel;
        
        player.GetComponent<player_script>().dieEvent+=(player)=>Die(player);

        Instantiate(load);

        if(loader!=null && loader.Load()) return;

        spheres_panel.SetActive(false);
        player.GetComponent<playerControl_script>().block=true;
        
        foreach (Item item in playerStartEquipment){
                Instantiate(item).Take(player);
        };
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){ //pause on Esc
            if(!isPaused) Pause();
            else Resume();
        }
    }
    void Resume(){
        isPaused=false;
        HUD.Settings.gameObject.SetActive(false);
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

    void Die(GameObject player){
        StopAllCoroutines();
        StartCoroutine(end());

        IEnumerator end(){
            post.DieEffect(0.02f);
            yield return new WaitForSeconds(4f);
            post.ScreenDarkening(true);
            yield return new WaitForSeconds(2f);
            post.ScreenDarkening(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    void Exit(){
        if(loader!=null)loader.Save();
        SceneManager.LoadScene("MainMenu");
    }
    
}
