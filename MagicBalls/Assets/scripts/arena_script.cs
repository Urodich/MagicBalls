using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class arena_script : MonoBehaviour
{
    [SerializeField] GameObject[] enemyTypes;
    List<GameObject[]> waves=new List<GameObject[]>();
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] UnityEngine.UI.Text infoText;
    GameObject player;
    List<GameObject> aliveEnemies = new List<GameObject>();
    Coroutine gameProcess;
    [SerializeField] GameObject pauseMenu; 
    [SerializeField] Text timer;
    float time;
    buffs_script buffs_Script;
    void Awake(){
        pauseMenu=GameObject.Find("pause menu");
    }
    void Start()
    {
        
        infoText.enabled=false;
        player = GameObject.Find("Player");
        buffs_Script = player.GetComponent<buffs_script>();
        waves.Add(new GameObject[]{enemyTypes[0], enemyTypes[1], enemyTypes[2], enemyTypes[0]});
        waves.Add(new GameObject[]{enemyTypes[0], enemyTypes[0], enemyTypes[0], enemyTypes[0], enemyTypes[1], enemyTypes[1]});
        waves.Add(new GameObject[]{enemyTypes[0], enemyTypes[0], enemyTypes[2], enemyTypes[2], enemyTypes[1], enemyTypes[1]});
        player.GetComponent<player_script>().dieEvent+=(GameObject)=>{StopCoroutine(gameProcess);EndGame();};
        gameProcess=StartCoroutine(Spawning());
        StartCoroutine(Bonuses());
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(!pauseMenu.active){
                Time.timeScale=0;
                buffs_Script.ResetStats();
                pauseMenu.SetActive(true);
                }
            else Resume();
        }
    }

    void FixedUpdate(){
        time+=Time.fixedDeltaTime;
        timer.text=((int)(time/60)).ToString()+":"+ ((int)(time%60)).ToString();
    }
    public void Exit(){
        Application.Quit();
    }
    public void Restart(){
        SceneManager.LoadScene("SampleScene");
        Time.timeScale=1;
    }
    public void Resume(){
        pauseMenu.SetActive(false);
        Time.timeScale=1;
    }
    public void End(){
        Time.timeScale=0;
        pauseMenu.SetActive(true);
        GameObject.Find("Resume").SetActive(false);
    }
    [SerializeField] bonus_script bonus;
    [SerializeField] Transform center;
    IEnumerator Bonuses(){
        while(true){

            yield return new WaitForSeconds(10f);
            yield return new WaitWhile(()=>Boost_panel.active);
            center.position+=new Vector3(Random.Range(0,5),0,Random.Range(0,5));
            Instantiate(bonus, center.position, new Quaternion());
            //Debug.Log("bonus spawned");
        }
    }
    int waveNumber=1;
    IEnumerator Spawning(){
        foreach(GameObject[] wave in waves){
            infoText.enabled=true;
            infoText.text="Wave # "+waveNumber++.ToString();
            yield return new WaitForSeconds(3f);
            infoText.enabled=false;
            foreach(GameObject enemy in wave){
                GameObject unit = Instantiate(enemy,spawnPoints[Random.Range(0,spawnPoints.Length-1)].position, new Quaternion());
                //Debug.Log("spawn enemy");
                aliveEnemies.Add(unit);
                enemy_script _enemy = unit.GetComponent<enemy_script>();
                _enemy.dieEvent+=(GameObject)=>aliveEnemies.Remove(GameObject);
                //_enemy.SetAim(player.transform.position);
                //Debug.Log("1 step");
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitWhile(()=>aliveEnemies.Count>0);
            ShowBoosts();
            yield return new WaitWhile(()=>Boost_panel.active);
        }
    }
    IEnumerator EndGame(){
        foreach(GameObject enemy in aliveEnemies){
            enemy.GetComponent<enemy_script>().Stun(10f);
        }
        infoText.enabled=true;
        infoText.text="You died";
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("EndGame");
    }
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
    [SerializeField] GameObject Boost_panel;
    [SerializeField] GameObject boost;
    void ShowBoosts(){
        Boost_panel.SetActive(true);

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
