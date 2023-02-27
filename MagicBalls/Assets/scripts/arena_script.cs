using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Threading.Tasks;

public class arena_script : MonoBehaviour
{
    [SerializeField] GameObject[] enemyTypes;
    [SerializeField] Transform[] spawnPoints;
    UnityEngine.UI.Text infoText;
    List<GameObject> aliveEnemies = new List<GameObject>();
    Coroutine gameProcess;
    GameObject pauseMenu; 
    Text Score;
    int score;
    buffs_script buffs;
    GameObject Boost_panel;
    [SerializeField] GameObject player;
    [SerializeField] GameObject HUD;
    [SerializeField] Transform PlayerSpawnPoint;
    void Awake(){
        player=Instantiate(player, PlayerSpawnPoint.position, new Quaternion());
        HUD=Instantiate(HUD, PlayerSpawnPoint, true);
    }
    void Start()
    {
        pauseMenu=HUD.transform.Find("pause menu").gameObject;
        Score=HUD.transform.Find("arena timer").gameObject.GetComponent<Text>();
        infoText=HUD.transform.Find("arena info").gameObject.GetComponent<Text>();
        Boost_panel=HUD.transform.Find("boost panel").gameObject;
        infoText.enabled=false;
        //Boost_panel.SetActive(false);
        //pauseMenu.SetActive(false);
        buffs = player.GetComponent<buffs_script>();
        player.GetComponent<player_script>().dieEvent+=(GameObject)=>EndGame();
        //gameProcess=StartCoroutine(Spawning());
        StartCoroutine(Bonuses());
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
        Application.Quit();
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
        pauseMenu.SetActive(false);
        Time.timeScale=1;
    }
    public void End(){
        Time.timeScale=0;
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
    [SerializeField] int[] waveEnemyCount;
    IEnumerator Spawning(){
        while (true){
            infoText.enabled=true;
            infoText.text="Wave # "+waveNumber++;//.ToString();
            yield return new WaitForSeconds(3f);
            infoText.enabled=false;
            for(int i=0; i<waveEnemyCount[waveNumber-1]; i++){
                SpawnEnemy(enemyTypes[Random.Range(0, enemyTypes.Length)],waveNumber*10, 1+waveNumber/5);
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitWhile(()=>aliveEnemies.Count>0);
            ShowBoosts();
            yield return new WaitWhile(()=>Boost_panel.active);
            Time.timeScale=1;
        }
        Win();
    }

    void SpawnEnemy(GameObject enemy, int points, float multiply){
        GameObject unit = Instantiate(enemy,spawnPoints[Random.Range(0,spawnPoints.Length-1)].position, new Quaternion());
        aliveEnemies.Add(unit);
        enemy_script _enemy=unit.GetComponent<enemy_script>();
        _enemy.dieEvent+=(GameObject)=>{aliveEnemies.Remove(GameObject); };
        _enemy.SetStartAim(player.transform.position);
    }
    void EndGame() => StartCoroutine(endGame());
    IEnumerator endGame(){
        StopCoroutine(gameProcess);
        /*foreach(GameObject enemy in aliveEnemies){
            enemy.GetComponent<enemy_script>().Stun(10f);
        }*/
        infoText.enabled=true;
        infoText.text="You died";
        yield return new WaitForSeconds(2f);
        End();
        //SceneManager.LoadScene("EndGame");
    }
    void Win(){}
    boost_str[] list = {
        new boost_str(Stats.MaxHP, "увеличить макс.хп на 20%", .2f),
        new boost_str(Stats.damage, "увеличить весь урон на 10%", .1f),
        new boost_str(Stats.fireDamage, "увеличить урон от огня на 25%", .25f),
        new boost_str(Stats.physicalDamage, "увеличить физ.урон на 25%", .25f),
        new boost_str(Stats.thunderDamage, "увеличить урон от молнии на 25%", .25f),
        new boost_str(Stats.MaxMana, "увеличить макс.ману на 20%", .2f),
        new boost_str(Stats.HpRegen, "увеличить регенерацию хп на 50%", .5f),
        new boost_str(Stats.ManaRegen, "увеличить регенерацию маны на 50%", .5f),
        new boost_str(Stats.speed, "увеличить скорость на 10%", .1f),
        new boost_str(Stats.projectileDamage, "увеличить урон снарядов на 20%", .2f),
        new boost_str(Stats.repulsion, "усилить отталкивание на 20%", .15f)
    };
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
