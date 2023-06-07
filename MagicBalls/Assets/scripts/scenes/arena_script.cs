using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


using System.Threading.Tasks;

public class arena_script : MonoBehaviour
{
    [SerializeField] wave[] Waves;
    [SerializeField] Transform[] spawnPoints;
    UnityEngine.UI.Text infoText;
    List<enemy_script> aliveEnemies = new List<enemy_script>();
    Coroutine gameProcess;
    GameObject pauseMenu; 
    Text Score;
    int score;
    buffs_script buffs;
    GameObject Boost_panel;
    [SerializeField] GameObject player;
    [SerializeField] HUD_script HUD;
    [SerializeField] Load_script load;
    [SerializeField] Transform PlayerSpawnPoint;
    [SerializeField] Item[] playerStartEquipment;
    void Awake(){
        HUD=Instantiate(HUD, PlayerSpawnPoint, true);
        player=Instantiate(player, PlayerSpawnPoint.position, new Quaternion());
        AddBoost();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<camera_script>().player=player;
    }
    void Start(){
        pauseMenu=HUD.PauseMenu;
        pauseMenu.transform.Find("Toggle").gameObject.SetActive(false);
        Score=HUD.gameObject.transform.Find("arena timer").gameObject.GetComponent<Text>();
        infoText=HUD.gameObject.transform.Find("arena info").gameObject.GetComponent<Text>();
        Boost_panel=HUD.BoostPanel;

        HUD.exit=Exit;
        HUD.resume=Resume;
        HUD.restart=Restart;

        infoText.enabled=false;
        buffs = player.GetComponent<buffs_script>();
        
        player.GetComponent<player_script>().dieEvent+=(GameObject)=>EndGame();

        //inventory_script inventory = player.GetComponent<inventory_script>();
        foreach (Item item in playerStartEquipment){
            Instantiate(item).Take(player);
        }
        
        gameProcess=StartCoroutine(Spawning());
        StartCoroutine(Bonuses());
        Instantiate(load);
        Time.timeScale=1;
    }
    bool isPaused=false;
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){ //pause on Esc
            if(!isPaused) Pause();
            else Resume();
        }
    }

    void AddScore(int points){
        score+=points;
        Score.text=score.ToString();
    }

    public void Exit(){
        SceneManager.LoadScene("MainMenu");
    }
    public void Restart(){
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex);
    }
    void Pause(){
        isPaused=true;
        Time.timeScale=0;
        buffs.UpdateStatsText();
        pauseMenu.SetActive(true);
    }
    public void Resume(){
        isPaused=false;
        HUD.Settings.gameObject.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale=1;
    }
    public void End(){
        Time.timeScale=0;
        buffs.UpdateStatsText();
        pauseMenu.SetActive(true);
        GameObject.Find("Resume").SetActive(false);
    }
    [SerializeField] bonus_script[] bonuses;
    [SerializeField] Transform center;
    [SerializeField] LayerMask ground;
    IEnumerator Bonuses(){
        while(true){

            yield return new WaitForSeconds(20f);
            //yield return new WaitWhile(()=>Boost_panel.active);
            while (true){
                center.position+=new Vector3(Random.Range(0,10),5,Random.Range(0,10));
                Ray ray=new Ray(center.position, Vector3.down);
                if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
                    if (hit.collider.gameObject.layer==6) break;
                }
            }
            Instantiate(bonuses[Random.Range(0, bonuses.Length)], center.position, new Quaternion());
        }
    }
    int waveNumber=1;
    //[SerializeField] int[] waveEnemyCount;
    IEnumerator Spawning(){
        foreach (wave wave in Waves){
            infoText.enabled=true;
            infoText.text="Wave # "+waveNumber++;//.ToString();
            yield return new WaitForSeconds(3f);
            infoText.enabled=false;
            
            foreach(enemys type in wave.value){
                SpawnEnemy(type.type, type.count, type.points, wave.scale);
                yield return new WaitForSeconds(0.5f);
            } 

            yield return new WaitWhile(()=>aliveEnemies.Count>0);
            ShowBoosts();
            yield return new WaitWhile(()=>Boost_panel.active);
            Time.timeScale=1;
        }
        Win();
    }

    void SpawnEnemy(enemy_script enemy, int count, int points, float multiply){
        for (int i=0;i<count;i++){
            enemy_script _enemy = Instantiate(enemy,spawnPoints[Random.Range(0,spawnPoints.Length-1)].position, new Quaternion());
            aliveEnemies.Add(_enemy);
            _enemy.dieEvent+=(GameObject)=>{aliveEnemies.Remove(_enemy); };
            _enemy.SetStartAim(player.transform.position);
        }
    }
    void EndGame() => StartCoroutine(endGame());
    [SerializeField] PostProcess_script post;
    IEnumerator endGame(){
        StopCoroutine(gameProcess);
        foreach(enemy_script enemy in aliveEnemies){
            enemy.Activate(false);
        }
        infoText.enabled=true;
        infoText.text="You died";
        post.DieEffect(0.02f);
        yield return new WaitForSeconds(4f);
        End();
        //SceneManager.LoadScene("EndGame");
    }
    void Win(){}
    boost_str[] list;
    void AddBoost(){ 
        list = new boost_str[]{
        new boost_str(Stats.MaxHP, "buffs/MaxHP","увеличить макс.хп на 20%", .2f),
        new boost_str(Stats.damage, "buffs/damage","увеличить весь урон на 10%", .1f),
        new boost_str(Stats.fireDamage, "buffs/fireDamage","увеличить урон от огня на 25%", .25f),
        new boost_str(Stats.physicalDamage, "buffs/physicalDamage", "увеличить физ.урон на 25%", .25f),
        new boost_str(Stats.thunderDamage, "buffs/thunderDamage", "увеличить урон от молнии на 25%", .25f),
        new boost_str(Stats.MaxMana, "buffs/MaxMana", "увеличить макс.ману на 20%", .2f),
        //new boost_str(Stats.HpRegen, "buffs/HpRegen", "увеличить регенерацию хп на 50%", .5f),
        //new boost_str(Stats.ManaRegen, "buffs/ManaRegen", "увеличить регенерацию маны на 50%", .5f),
        new boost_str(Stats.speed, "buffs/speed", "увеличить скорость на 10%", .1f),
        //new boost_str(Stats.projectileDamage, "buffs/projectileDamage", "увеличить урон снарядов на 20%", .2f),
        new boost_str(Stats.repulsion, "buffs/repulsion", "усилить отталкивание на 20%", .15f)
    };
    }
    [SerializeField] GameObject boost;
    void ShowBoosts(){
        Boost_panel.SetActive(true);
        Time.timeScale=0;
        List<boost_str> set=new List<boost_str>(list);
        foreach(Transform child in Boost_panel.transform) {
        Destroy(child.gameObject);
        }
        for(int i=0;i<3;i++){
            boost_script inst = Instantiate(boost, Boost_panel.transform).GetComponent<boost_script>();
            int index = Random.Range(0,set.Count);
            inst.Set(set[index]);
            set.RemoveAt(index);
        }
    }
}

