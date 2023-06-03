using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.Events;

public class Spells_script : MonoBehaviour
{
    public  Dictionary<int, System.Action> Spells;
    GameObject player;
    player_script stats;
    public playerControl_script control;
    NavMeshAgent navMesh;
    buffs_script buffs;
    public LayerMask enemies, ground, friends;

    GameObject spellPanel;
    [SerializeField] GameObject spellCooDown;
    [SerializeField] public bool GodMod {get;set;}=false;
    public Animator animator;

    //current action
    public Coroutine currentCast;
    public SpellBase currentSpell;
    UnityEvent BreakCastEvent;
    [SerializeField] GameObject SPELLS;
    void Start(){
        spellPanel=GameObject.Find("SpellPanel");
        player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<player_script>();
        navMesh = player.GetComponent<NavMeshAgent>();
        buffs = player.GetComponent<buffs_script>();
        control=player.GetComponent<playerControl_script>();
        _flame.Stop();
        BreakCastEvent = new UnityEvent();
        animator=stats.GetAnimator();
        Spells = new Dictionary<int, System.Action>(){
        [0]=castEmpty,
        [1] = ()=>SPELLS.GetComponent<Flame>().Cast(1),
        [2] = ()=>SPELLS.GetComponent<Flame>().Cast(2),
        [3] = ()=>SPELLS.GetComponent<Flame>().Cast(3),
        [4] = ()=>SPELLS.GetComponent<Wave>().Cast(1),
        [5] = ()=>SPELLS.GetComponent<Smoke>().Cast(1,1),
        [6] = ()=>SPELLS.GetComponent<Smoke>().Cast(2,1),
        [8] = ()=>SPELLS.GetComponent<Wave>().Cast(2),
        [9] = ()=>SPELLS.GetComponent<Smoke>().Cast(1,2),
        [12] = ()=>SPELLS.GetComponent<Wave>().Cast(3),
        [13] = ()=>SPELLS.GetComponent<Wind>().Cast(1),
        [14] = ()=>SPELLS.GetComponent<Fireball>().Cast(1,1),//Fireball(1,1),
        [15] = ()=>SPELLS.GetComponent<Fireball>().Cast(1,2),
        [17] = ()=>SPELLS.GetComponent<Torrent>().Cast(1,1),
        [18] = ()=>SPELLS.GetComponent<FireStorm>().Cast(),
        [21] = ()=>SPELLS.GetComponent<Torrent>().Cast(1,2),
        [26] = ()=>SPELLS.GetComponent<Wind>().Cast(2),
        [27] = ()=>SPELLS.GetComponent<Fireball>().Cast(2,1),
        [30] = ()=>SPELLS.GetComponent<Torrent>().Cast(2,1),
        [39] = ()=>SPELLS.GetComponent<Wind>().Cast(3),
        [40] = ()=>SPELLS.GetComponent<Earthquake>().Cast(1),
        [41] = ()=>SPELLS.GetComponent<Lava>().Cast(1,1),
        [42] = ()=>SPELLS.GetComponent<Lava>().Cast(1,2),
        [44] = ()=>SPELLS.GetComponent<Mud>().Cast(1,1),
        [48] = ()=>SPELLS.GetComponent<Mud>().Cast(2,1),
        [53] = ()=>SPELLS.GetComponent<Wall>().Cast(1,1),
        [54] = ()=>SPELLS.GetComponent<Meteor>().Cast(),
        [66] = ()=>SPELLS.GetComponent<Wall>().Cast(2,1),
        [80] = ()=>SPELLS.GetComponent<Earthquake>().Cast(2),
        [81] = ()=>SPELLS.GetComponent<Lava>().Cast(2,1),
        [84] = ()=>SPELLS.GetComponent<Mud>().Cast(1,2),
        [93] = ()=>SPELLS.GetComponent<Wall>().Cast(1,2),
        [120] = ()=>SPELLS.GetComponent<Earthquake>().Cast(3),
        [121] = ()=>SPELLS.GetComponent<Wisp>().Cast(1),
        [122] = ()=>SPELLS.GetComponent<Phoenix>().Cast(1,1),
        [123] = ()=>SPELLS.GetComponent<Phoenix>().Cast(1,2),
        [125] = ()=>SPELLS.GetComponent<ManaRegen>().Cast(1,1),
        [126] = ()=>SPELLS.GetComponent<Drain>().Cast(),
        [129] = ()=>SPELLS.GetComponent<ManaRegen>().Cast(2,1),
        [134] = ()=>SPELLS.GetComponent<Bird>().Cast(1,1),
        [147] = ()=>SPELLS.GetComponent<Bird>().Cast(2,1),
        [161] = ()=>SPELLS.GetComponent<Golem>().Cast(1,1),
        [165] = ()=>SPELLS.GetComponent<Shield>().Cast(),
        [174] = ()=>SPELLS.GetComponent<Tornado>().Cast(),
        [201] = () => SPELLS.GetComponent<Golem>().Cast(2, 1),
        [242] = ()=>SPELLS.GetComponent<Wisp>().Cast(2),
        [243] = ()=>SPELLS.GetComponent<Phoenix>().Cast(2,1),
        [246] = ()=>SPELLS.GetComponent<ManaRegen>().Cast(1,2),
        [255] = ()=>SPELLS.GetComponent<Bird>().Cast(1,2),
        [282] = () => SPELLS.GetComponent<Golem>().Cast(1, 2),
        [363] = ()=>SPELLS.GetComponent<Wisp>().Cast(3),
        [364] = ()=>SPELLS.GetComponent<Chain>().Cast(1),
        [365] = ()=>SPELLS.GetComponent<Blast>().Cast(1,1),
        [366] = ()=>SPELLS.GetComponent<Blast>().Cast(2,1),
        [368] = ()=>SPELLS.GetComponent<Pool>().Cast(1,1),
        [369] = ()=>SPELLS.GetComponent<MindControl>().Cast(),
        [372] = ()=>SPELLS.GetComponent<Pool>().Cast(2,1),
        [377] = ()=>SPELLS.GetComponent<Haste>().Cast(1,1),
        [378] = ()=>SPELLS.GetComponent<BlackHole>().Cast(),
        [388] = ()=>SPELLS.GetComponent<Haste>().Cast(1,2),
        [404] = ()=>SPELLS.GetComponent<Blink>().Cast(1,1),
        [444] = ()=>SPELLS.GetComponent<Blink>().Cast(2,1),
        [485] = ()=>SPELLS.GetComponent<Zombie>().Cast(1,1),
        [486] = ()=>SPELLS.GetComponent<Reincarnation>().Cast(),
        [489] = ()=>SPELLS.GetComponent<Illusion>().Cast(),
        [525] = ()=>SPELLS.GetComponent<Tomb>().Cast(),
        [606] = ()=>SPELLS.GetComponent<Zombie>().Cast(2,1),
        [728] = ()=>SPELLS.GetComponent<Chain>().Cast(2),
        [729] = ()=>SPELLS.GetComponent<Blast>().Cast(1,2),
        [732] = ()=>SPELLS.GetComponent<Pool>().Cast(1,2),
        [741] = ()=>SPELLS.GetComponent<Haste>().Cast(2,1),
        [768] = ()=>SPELLS.GetComponent<Blink>().Cast(1,2),
        [849] = ()=>SPELLS.GetComponent<Zombie>().Cast(1,2),
        [1092] = ()=>SPELLS.GetComponent<Chain>().Cast(3)
        };
    
    }

#region ///////UTILS///////
    public void CastSpell(int hex){ //Call hex function
        if (Spells.ContainsKey(hex))
            Spells[hex]();
        else
            Spells[0]();
    }
    public void NotEnoughtMana(){
        Debug.Log("NotMana");
    }
    public void CoolDawn(){
        Debug.Log("CoolDown");
    }
    void castEmpty(){
        Debug.Log("empty");
        animator.SetTrigger("");
    }
    //create icon
    public void CastSpell(float CDTime, String imagePath, Action func){ //Add CD icon
        Sprite image=Resources.Load<Sprite>(imagePath);
        if(image==null) Debug.Log("no Sprite loaded");

        GameObject obj = Instantiate(spellCooDown);
        obj.transform.SetParent(spellPanel.transform,false);
        Timer spell = obj.GetComponent<Timer>();
        spell.func=func;
        spell.SrcImage=image;
        spell.SetTime(CDTime,true);
        
        currentSpell=null;
    }
    public void BreakCast(){
        animator.SetBool("casting", false);
        if(currentCast!=null)StopCoroutine(currentCast);
        if(currentCast!=null) currentSpell.Break();
    }

    public void StopMoving(bool value){
        if(value){
            navMesh.isStopped=true;
            control.isStopped=true;
        }
        else{
            if(navMesh.enabled)navMesh.isStopped=false;
            control.isStopped=false;
        }
    }

    public GameObject[] projectileCast(int count, GameObject projectile, float scale){
        List<GameObject> prjs = new List<GameObject>();
        for(int i = 0; i < count; i++){
                GameObject obj = Instantiate(projectile, player.transform.position+player.transform.forward+player.transform.right*i*0.3f, new Quaternion());
                obj.transform.localScale.Scale(new Vector3(scale,1,scale));
                prjs.Add(obj);
                if(count==1) break;
                obj = Instantiate(projectile, player.transform.position+player.transform.forward+player.transform.right*i*0.3f, new Quaternion());
                obj.transform.localScale.Scale(new Vector3(scale,1,scale));
                prjs.Add(obj);
            }
        return prjs.ToArray();
    }

    //Векторное применение
    public Vector3 vectorCastDirection;
    public IEnumerator VectorCast(){
        Vector3 mousePos1 = new Vector3(), mousePos2 = new Vector3();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground))
            mousePos1=hit.point;
        mousePos2=mousePos1+Vector3.forward;

        yield return new WaitUntil(()=> Input.GetMouseButtonUp(1));

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit2, float.MaxValue, ground)){
            mousePos2=hit2.point;
        }
        vectorCastDirection = (mousePos2-mousePos1);
        Debug.Log(vectorCastDirection);
        vectorCastDirection.Scale(new Vector3(1,0,1));
        vectorCastDirection.Normalize();
        Debug.DrawLine(mousePos1+Vector3.up, mousePos2+Vector3.up);
    }
    
#endregion

#region ////////OLD SPELLS/////////
    //FIREBALL
    [SerializeField] GameObject fireball;
    bool fireballCoolDown=false;
    float fireballManaCost=10f;
    float fireballDelay=0.5f;
    void Fireball(float damage, float speed){
        if(!GodMod){
            if(fireballCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<fireballManaCost) {NotEnoughtMana(); return;}
        }
        currentCast=StartCoroutine(fireball_core());
        IEnumerator fireball_core(){
            Debug.Log("fireball");
            animator.SetTrigger("cast8");
            yield return new WaitForSeconds(fireballDelay);
            if(!GodMod){
                stats.CurMana-=fireballManaCost;
                fireballCoolDown=true;
                CastSpell(2f,"FireBall",()=>fireballCoolDown=false);
            }
            foreach (GameObject i in projectileCast((int)buffs.GetStats(Stats.projectiles),fireball, damage)){
                fireball_script fb = i.GetComponent<fireball_script>();
                fb.damage *=damage;
                fb.speed *=speed;
            }
        }
        
    }
    //PHOENIX
    [SerializeField] GameObject phoenix;
    bool phoenixCollDown = false;
    float phoenixManaCost=20f;
    float phoenixDelay=0.2f;
    GameObject phoen;
    void Phoenix(int time, int strength){
        if(!GodMod){
            if(phoenixCollDown) {CoolDawn(); return;}
            if(stats.CurMana<phoenixManaCost) {NotEnoughtMana(); return;}
        }
        currentCast=StartCoroutine(phoenix_core());
        IEnumerator phoenix_core(){
            animator.SetTrigger("cast1");
            yield return new WaitForSeconds(phoenixDelay);
            if(!GodMod){
                stats.CurMana-=phoenixManaCost;
                phoenixCollDown=true;
                CastSpell(30f,"Phoenix",()=>phoenixCollDown=false);
            }
            if(phoen) Destroy(phoen);
            Debug.Log("phoenix");
            
            StartCoroutine(Damage(10*time));

            IEnumerator Damage(float time){
                while (time>0)
                {
                    Collider[] aims = Physics.OverlapSphere(gameObject.transform.position, 3, enemies);
                    foreach(Collider i in aims){
                        if(i.tag!="enemy") continue;
                        i.gameObject.GetComponent<unit_script>().TakeDamage(5*buffs.GetStats(Stats.damage)*buffs.GetStats(Stats.fireDamage)*strength, DamageType.Fire);
                    }
                    yield return new WaitForSeconds(0.5f);
                    time-=.5f;
                }
            }
        }
    }
    //WAVE
    [SerializeField] GameObject waveOrig;
    bool waveCollDown=false;
    float waveManaCost=10f;
    float waveDelay=0.5f;
    void Wave(int scale){
        if(!GodMod){
            if(waveCollDown) {CoolDawn(); return;}
            if(stats.CurMana<waveManaCost*scale) {NotEnoughtMana(); return;}
        }
        currentCast=StartCoroutine(wave_core());
        IEnumerator wave_core(){
            animator.SetTrigger("cast9");
            yield return new WaitForSeconds(waveDelay);
            if (!GodMod){
                stats.CurMana-=waveManaCost*scale;
                waveCollDown=true;
                CastSpell(5f*scale,"Wave",()=>waveCollDown=false);
            }
                Debug.Log("wave");

            projectileCast((int)buffs.GetStats(Stats.projectiles),waveOrig, scale);
        }
    }
    //Earthquake
    bool EarthqukeCoolDowm=false;
    float EarhtquakeManaCost=15f;
    float earhtquakeDelay=0.7f;
    [SerializeField] ParticleSystem _earthquake;
    void Earhtquake(int strength){
        if(!GodMod){
            if(EarthqukeCoolDowm) {CoolDawn(); return;}
            if(stats.CurMana<EarhtquakeManaCost*strength) {NotEnoughtMana(); return;}
        }
        currentCast=StartCoroutine(earthquake_core());
        IEnumerator earthquake_core(){
            Debug.Log("earthquake");
            animator.SetTrigger("cast6");
            StopMoving(true);
            yield return new WaitForSeconds(earhtquakeDelay);
            StopMoving(false);
            if(!GodMod){
                stats.CurMana-=EarhtquakeManaCost*strength;
                EarthqukeCoolDowm=true;
                CastSpell(7f*strength,"Earthquake",()=>EarthqukeCoolDowm=false);
            }
            _earthquake.Play();
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, 2*strength, enemies);
            foreach (Collider collider in colliders){
                if (collider.tag!="enemy") continue;
                unit_script enemy = collider.gameObject.GetComponent<unit_script>();
                if(enemy.isFlying) continue;
                enemy.Stun(0.5f*strength);
                enemy.TakeDamage(strength*20*buffs.GetStats(Stats.damage)*buffs.GetStats(Stats.physicalDamage), DamageType.Physical);
            }
        }
    }
    //BLINK
    bool blinkCollDown=false;
    float blinkManaCost=15f;
    float blinkDelay=0.6f;
    [SerializeField] ParticleSystem _blink;
    void Blink(int earth, int thunder){
        Vector3 mousePos;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
            if(!GodMod){
                if(blinkCollDown) {CoolDawn(); return;}
                if(stats.CurMana<blinkManaCost*thunder) {NotEnoughtMana(); return;}
            }
            animator.SetTrigger("jump");
            currentCast=StartCoroutine(blink_core());
        }
        IEnumerator blink_core(){
            yield return new WaitForSeconds(blinkDelay);
            if(!GodMod){
                stats.CurMana-=blinkManaCost*thunder;
                blinkCollDown=true;
                CastSpell(5f,"Blink",()=>blinkCollDown=false);
            }
            Debug.Log("blink");
            mousePos= hit.point;
            navMesh.enabled=false;
            player.transform.position=mousePos;
            navMesh.enabled=true;
            navMesh.destination=mousePos;
            _blink.Play();
            Collider[] colliders = Physics.OverlapCapsule(player.transform.position + new Vector3(0,1,0), player.transform.position, 2*earth, enemies.value);
            foreach (Collider collider in colliders){
                if (collider.tag!="enemy") continue;
                unit_script enemy = collider.gameObject.GetComponent<unit_script>();
                if(enemy.isFlying) continue;
                enemy.Stun(0.5f*earth);
                enemy.TakeDamage(thunder*20*buffs.GetStats(Stats.thunderDamage)*buffs.GetStats(Stats.damage), DamageType.Thunder);
            }
        }
    }
    //WIND
    [SerializeField] GameObject windOrig;
    bool windCoolDown=false;
    float windManaCost=20f;
    void Wind(int strength){
        if(!GodMod){
            if(windCoolDown) return;
            if(stats.CurMana<windManaCost*strength) return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground))
            StartCoroutine(Wind_core());

        IEnumerator Wind_core(){
            yield return StartCoroutine(VectorCast());
            animator.SetTrigger("cast9");
            wind_script wind = Instantiate(windOrig,hit.point,Quaternion.LookRotation(vectorCastDirection)).GetComponent<wind_script>();
            wind.strength=strength;
            wind.direction=vectorCastDirection;
            if(!GodMod){
                stats.CurMana-=windManaCost*strength;
                windCoolDown=true;
                CastSpell(15f*strength,"Wind",()=>windCoolDown=false);
            }
        }
    }

    //TORRENT
    [SerializeField] GameObject torrentOrig;
    bool torrentCoolDown = false;
    float torrentManaCost = 20f;
    float torrentDelay=0.3f;
    void Torrent(int air, int water){
        if(!GodMod){
            if(torrentCoolDown) return;
            if(stats.CurMana<torrentManaCost*water) return;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
            animator.SetTrigger("cast1");  
            currentCast=StartCoroutine(torrent_core());
        }
        IEnumerator torrent_core(){
            yield return new WaitForSeconds(torrentDelay);
            Debug.Log("torrent");
            if(!GodMod){
                stats.CurMana-=torrentManaCost*water;
                torrentCoolDown=true;
                CastSpell(10f*air,"Torrent",()=>torrentCoolDown=false);
            }
            torrent_script torrent = Instantiate(torrentOrig, hit.point, new Quaternion()).GetComponent<torrent_script>();
            torrent.air=air;
            torrent.water=water;
        }
    }

    //MUD
    [SerializeField] GameObject mudOrig;
    bool mudCoolDown = false;
    float mudManaCost = 40f;
    void Mud(int water, int earth){
        if(!GodMod){
            if(mudCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<mudManaCost*earth) {NotEnoughtMana(); return;}
            stats.CurMana-=mudManaCost*earth;
            mudCoolDown=true;
            CastSpell(7f*earth,"Mud",()=>mudCoolDown=false);
        }
        animator.SetTrigger("cast6");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
            mud_script mud = Instantiate(mudOrig,hit.point,new Quaternion()).GetComponent<mud_script>();
            mud.earht = earth;
            mud.water = water;
        }
    }

    //LAVA
    [SerializeField] GameObject lavaOrig;
    bool lavaCoolDown=false;
    float lavaManaCost=30f;
    void Lava(int earht, int fire){
        if(!GodMod){
            if(lavaCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<lavaManaCost*fire) {NotEnoughtMana(); return;}
            stats.CurMana-=lavaManaCost*fire;
            lavaCoolDown=true;
            CastSpell(15f*earht,"Lava",()=>lavaCoolDown=false);
        }
        animator.SetTrigger("cast6");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
            lava_script lava = Instantiate(lavaOrig, hit.point, new Quaternion()).GetComponent<lava_script>();
            lava.fire=fire;
            lava.earht=earht;
        }
    }

    //ELECTRIC POOL
    [SerializeField] GameObject EPoolOrig;
    bool ElectricPoolCoolDown=false;
    float ElectricPoolManaCost=20f;
    void ElectricPool(int water, int thunder){
        if(!GodMod){
            if(ElectricPoolCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<ElectricPoolManaCost*((thunder-1)/2+1)) {NotEnoughtMana(); return;}
            stats.CurMana-=ElectricPoolManaCost*((thunder-1)/2+1);
            ElectricPoolCoolDown=true;
            CastSpell(15f*water,"Pool",()=>ElectricPoolCoolDown=false);
        }
        animator.SetTrigger("cast6");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
            electric_pool_script EPool = Instantiate(EPoolOrig, hit.point, new Quaternion()).GetComponent<electric_pool_script>();
            EPool.thunder=thunder;
            EPool.water=water;
        }
    }

    //HASTE
    bool HasteCoolDown=false;
    float HasteManaCost=15f;
    float value; //try to fix it
    void Haste(int thunder, int air){
        if(!GodMod){
            if(HasteCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<HasteManaCost*thunder) {NotEnoughtMana(); return;}
            stats.CurMana-=HasteManaCost*thunder;
            HasteCoolDown=true;
            CastSpell(10f*air,"Haste",()=>HasteCoolDown=false);
        }
        value=.5f*thunder;
        player.GetComponent<unit_script>().ChangeSpeed(value);
        animator.SetBool("haste",true);
        Invoke("Remove", 5*air);
    }
    void Remove(){player.GetComponent<unit_script>().ChangeSpeed(-value);animator.SetBool("haste",false);} //mb change later
    
    //Lightning chain
    bool chainCoolDown=false;
    float chainManaCost=15f;
    float chainDelay=0.3f;
    [SerializeField] ParticleSystem _chain;
    void Chain(int strength){
        if(!GodMod){
            if(chainCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<chainManaCost*strength) {NotEnoughtMana(); return;}
        }
        currentCast=StartCoroutine(chain_core());
        IEnumerator chain_core(){
            animator.SetTrigger("cast8");
            yield return new WaitForSeconds(chainDelay);
            if(!GodMod){
                stats.CurMana-=chainManaCost*strength;
                chainCoolDown=true;
                CastSpell(3f,"Chain",()=>chainCoolDown=false);
            }
            
            float radius = 2f;
            int steps=5+strength*2;
            Vector3 oldPoint;
            ParticleSystem particleSystem;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
                particleSystem = Instantiate(_chain, gameObject.transform);
                StartCoroutine(ChainStep(hit.point, radius, null));

            }

            IEnumerator ChainStep(Vector3 position, float radius, Collider last){
                while(steps>0){
                    bool hit=false;
                    Collider[] colliders = Physics.OverlapSphere(position, radius, enemies);
                    foreach(Collider col in colliders){
                        if(col.tag!="enemy")continue;
                        if(col==last) continue;
                        colliders=null;
                        ParticleSystem.EmitParams param = new ParticleSystem.EmitParams();
                        param.position = col.transform.position;
                        particleSystem.Emit(param, 1);
                        //particleSystem.Emit(col.transform.position, Vector3.zero, 0, 2f,Color.white);
                        unit_script enemy = col.GetComponent<unit_script>();
                        enemy.TakeDamage(15*buffs.GetStats(Stats.thunderDamage)*buffs.GetStats(Stats.damage), DamageType.Thunder);
                        last=col;
                        hit=true;
                        steps--;
                        oldPoint=col.gameObject.transform.position;
                        break;
                    }
                    if(!hit) break;
                    yield return new WaitForSeconds(.1f);
                }
                Destroy(particleSystem.gameObject,2f);
            }
        }
    }
    
    //ILLUSION
    [SerializeField] GameObject _illusion;
    bool illusionCoolDown=false;
    float illusionManaCost=20f;
    void Illusion(){
        if(!GodMod){
            if(illusionCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<illusionManaCost) {NotEnoughtMana(); return;}
            stats.CurMana-=illusionManaCost;
            illusionCoolDown=true;
            CastSpell(10f,"Illusion",()=>illusionCoolDown=false);
        }
        animator.SetTrigger("cast1");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
            Debug.Log("illusion");
            Instantiate(_illusion, player.transform.position+player.transform.forward, player.transform.rotation).GetComponent<illusion_script>().destination=hit.point;
        }
    }
    
    //REINCARNATION
    bool reincarnationCoolDown=false;
    float reincarnationManaCost=80f;
    void Reincarnation(){
        if(!GodMod){
            if(reincarnationCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<reincarnationManaCost) {NotEnoughtMana(); return;}
            stats.CurMana-=reincarnationManaCost;
            reincarnationCoolDown=true;
            CastSpell(90f,"Reincarnation",()=>reincarnationCoolDown=false);
        }
        Debug.Log("reincarnation");
        animator.SetTrigger("cast1");
        player.GetComponent<player_script>().reincarnation=true;
    }

    //METEOR
    [SerializeField] GameObject _meteor;
    bool meteorCoolDown=false;
    float meteorManaCost=45f;
    void Meteor(){
        if(!GodMod){
            if(meteorCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<meteorManaCost) {NotEnoughtMana(); return;}
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
            StartCoroutine(meteor_core());
        }

        IEnumerator meteor_core(){
            yield return StartCoroutine(VectorCast());
            meteor_script meteor = Instantiate(_meteor,hit.point+new Vector3(0,4,0),Quaternion.LookRotation(vectorCastDirection)).GetComponentInChildren<meteor_script>();
            meteor.direction=vectorCastDirection;
            animator.SetTrigger("cast4");
            if(!GodMod){
                stats.CurMana-=meteorManaCost;
                meteorCoolDown=true;
                CastSpell(20f,"Meteor",()=>meteorCoolDown=false);
            }
        }
    }

    //MANA REGEN
    bool ManaRegenCoolDown=false;
    float ManaRegenManaCost=5f;
    [SerializeField] ParticleSystem _manaRegen;
    void ManaRegen(int water, int life){
        if(!GodMod){
            if(ManaRegenCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<ManaRegenManaCost*life) {NotEnoughtMana(); return;}
            stats.CurMana-=ManaRegenManaCost*life;
            ManaRegenCoolDown=true;}
        
        Debug.Log("ManaRegen");
        currentCast=StartCoroutine(cast());
        BreakCastEvent.AddListener(()=>_manaRegen.Stop());
        IEnumerator cast(){
            animator.SetBool("casting", true);
            animator.SetTrigger("cast_blast");
            _manaRegen.Play();
            StopMoving(true);
            player_script ps = player.GetComponent<player_script>();
            ps.manaRegen+=5f*water;
            float ownArmor=ps.armor;
            ps.armor+=15*life;
            yield return new WaitUntil(()=>Input.GetMouseButtonUp(1));
            ps.manaRegen-=5f*water;
            if(ps.armor>ownArmor) ps.armor=ownArmor;
            StopMoving(false);
            CastSpell(2f,"Mana Regen",()=>ManaRegenCoolDown=false);
            _manaRegen.Stop(true);
            BreakCastEvent.RemoveListener(()=>_manaRegen.Stop());
            animator.SetBool("casting", false);
        }
    }
    
    //BIRD
    bool BirdCoolDown=false;
    float BirdManaCost=10f;
    float birdDelay=0.3f;
    [SerializeField] ParticleSystem _bird;
    void Bird(int air, int life){
        if(!GodMod){
            if(BirdCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<BirdManaCost) {NotEnoughtMana(); return;}
        }
        
        StartCoroutine(bird_core());
        IEnumerator bird_core(){
            animator.SetTrigger("cast6");
            yield return new WaitForSeconds(birdDelay);
            if(!GodMod){
                stats.CurMana-=BirdManaCost;
                BirdCoolDown=true;
                CastSpell(10f,"Bird",()=>BirdCoolDown=false);
            }
            _bird.Play();
            yield return new WaitForSeconds(.6f);
            Debug.Log("Bird");
            
            Collider[] enemy = Physics.OverlapSphere(player.transform.position, 3f*life,enemies);
            foreach(Collider i in enemy){
                if(i.tag!="enemy") continue;
                unit_script unit = i.gameObject.GetComponent<unit_script>();
                unit.TakeDamage(1f,DamageType.Physical);
                Vector3 dir=(i.gameObject.transform.position-player.transform.position);
                dir.Scale(new Vector3(1,0,1));
                unit.Move(dir.normalized, 2f*air*buffs.GetStats(Stats.repulsion), 1f, false);
            }
        }
        
    }
    
    //WISP
    bool WispCoolDown=false;
    float WispManaCost=15f;
    void Wisp(int strength){
        if(!GodMod){
            if(windCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<windManaCost*strength) {NotEnoughtMana(); return;}
            stats.CurMana-=windManaCost*strength;
            windCoolDown=true;
            CastSpell(15*strength,"Wisp",()=>windCoolDown=false);
        }
        Debug.Log("Wisp");
        animator.SetTrigger("cast1");
        StartCoroutine(heal(5*strength));

        IEnumerator heal(float time){
            while(time>0){
                yield return new WaitForSeconds(.5f);
                player.GetComponent<player_script>().AddHP(5f*strength);
                time-=.5f;
            }
        }
    }

    //TORNADO
    bool tornadoCoolDown=false;
    float tornadoManaCast=65f;
    [SerializeField] GameObject _tornado;
    void Tornado(){
        if(!GodMod){
            if(tornadoCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<tornadoManaCast) {NotEnoughtMana(); return;}
            stats.CurMana-=tornadoManaCast;
            tornadoCoolDown=true;
            CastSpell(20f,"Tornado",()=>tornadoCoolDown=false);
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
            Debug.Log("Tornado");
            animator.SetTrigger("cast9");
            Instantiate(_tornado, hit.point, player.transform.rotation).GetComponent<tornado_script>();
        }
    }

    //BLAST
    bool blastCoolDown=false;
    float blastManaCost=30f;
    [SerializeField] ParticleSystem blast;
    [SerializeField] ParticleSystem blast_expl;
    void Blast(int fire, int thunder){
        if(!GodMod){
            if(blastCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<blastManaCost) {NotEnoughtMana(); return;}
            stats.CurMana-=blastManaCost;
            blastCoolDown=true;}
        
        Debug.Log("blast");
        float time=0f;
        StartCoroutine(cast());

        IEnumerator cast(){
            animator.SetTrigger("cast_blast");
            animator.SetBool("casting", true);
            StopMoving(true);
            ParticleSystem bl = Instantiate(blast,gameObject.transform);
            while(Input.GetMouseButton(1)){
                time+=Time.deltaTime;
                if(time>=5f) break;
                yield return new WaitForEndOfFrame();
            }
            StopMoving(false);
            Destroy(bl.gameObject);
            Destroy(Instantiate(blast_expl,gameObject.transform).gameObject,2);
            animator.SetBool("casting", false);    
            Collider[] enemy = Physics.OverlapSphere(player.transform.position, 1f*thunder*time, enemies);
            foreach(Collider elem in enemy){
                unit_script en=elem.gameObject.GetComponent<unit_script>();
                en.TakeDamage(5*fire*buffs.GetStats(Stats.fireDamage)*buffs.GetStats(Stats.damage)*time, DamageType.Fire);
                en.Move(elem.gameObject.transform.position-player.transform.position, 2f*fire*time, .5f, false);
            }
            if(time>=3){
                player_script player_=player.GetComponent<player_script>();
                player_.TakeDamage(5*fire*buffs.GetStats(Stats.fireDamage)*buffs.GetStats(Stats.damage)*time, DamageType.Fire);
                player_.Stun(2f);
            }
            if(!GodMod)CastSpell(10f,"Blast",()=>blastCoolDown=false);
        }
        
    }

    //ZOMBIE
    bool zombieCoolDown=false;
    float zombieManaCost=15f;
    float zombieDelay=0.3f;
    [SerializeField] GameObject _zombie;
    void Zombie(int life, int thunder){
        if(control.mouseTarget==null) return;
        if(!GodMod){
            if(zombieCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<zombieManaCost) {NotEnoughtMana(); return;}
        }
        currentCast=StartCoroutine(zombie_core());
        IEnumerator zombie_core(){
            animator.SetTrigger("cast4");
            yield return new WaitForSeconds(zombieDelay);
            if(!GodMod){
                stats.CurMana-=zombieManaCost;
                zombieCoolDown=true;
                CastSpell(15f,"Zombie",()=>zombieCoolDown=false);
            }
            Debug.Log("zombie");

            for(int i=0;i<2*life;i++){
                Instantiate(_zombie, control.mouseTarget.transform.position, new Quaternion()).GetComponent<zombie>().SetZombieAim(control.mouseTarget);
            }
        }
    }

    //MIND CONTROL
    bool mindCoolDown=false;
    float mindManaCost=50f;
    float controlDelay=0.4f;
    void MindControl(){
        if(control.mouseTarget==null) return;
        if(!GodMod){
            if(mindCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<mindManaCost) {NotEnoughtMana(); return;}
        }
        currentCast=StartCoroutine(control_core());
        IEnumerator control_core(){
            animator.SetTrigger("cast4");
            yield return new WaitForSeconds(controlDelay);
            if(!GodMod){
                stats.CurMana-=mindManaCost;
                mindCoolDown=true;
                CastSpell(80f,"Mind Control",()=>mindCoolDown=false);
            }
            
            Debug.Log("mind control");

            enemy_script enemy=control.mouseTarget.GetComponent<enemy_script>();
            enemy.enemies=enemies;
            enemy.aims.Clear();
        }
    }

    //BLACKHOLE
    bool BlackHoleCoolDown=false;
    float BlackHoleManaCost=70f;
    [SerializeField] GameObject _blackHole;
    void BlackHole(){
        if(!GodMod){
            if(BlackHoleCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<BlackHoleManaCost) {NotEnoughtMana(); return;}
            stats.CurMana-=BlackHoleManaCost;
            BlackHoleCoolDown=true;
            CastSpell(50f,"Black Hole",()=>BlackHoleCoolDown=false);
        }
        Debug.Log("black hole");
        animator.SetTrigger("cast6");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
            Instantiate(_blackHole, hit.point, new Quaternion());
        }
    }
    
    //FIRESTORM
    bool FireStormCoolDown=false;
    float FireStormManaCost=80f;
    [SerializeField] GameObject _fireStorm;
    void FireStorm(){
        if(!GodMod){
            if(FireStormCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<FireStormManaCost) {NotEnoughtMana(); return;}
            stats.CurMana-=FireStormManaCost;
            FireStormCoolDown=true;
        }
        
        Debug.Log("Fire Storm");
        StartCoroutine(cast());
        IEnumerator cast(){
            animator.SetTrigger("cast_global");
            animator.SetBool("casting", true);
            Coroutine fire;
            StopMoving(true);
            fire=StartCoroutine(damage());
            yield return new WaitWhile(()=>Input.GetMouseButton(1));
            StopCoroutine(fire);
            StopMoving(false);
            animator.SetBool("casting", false);
            if(!GodMod)CastSpell(20f,"Fire Storm",()=>FireStormCoolDown=false);
        }
        IEnumerator damage(){
            while(true){
                Vector3 ofset=new Vector3(UnityEngine.Random.Range(-10,10),6,UnityEngine.Random.Range(-10,10));
                Instantiate(_fireStorm, player.transform.position+ofset, new Quaternion());
                yield return new WaitForSeconds(.2f);
            }
        }
    }
    
    //TOMB
    bool TombCoolDown=false;
    float TombManaCost=45f;
    [SerializeField] GameObject _tomb;
    void Tomb(){
        if(!GodMod){
            if(TombCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<TombManaCost) {NotEnoughtMana(); return;}
            stats.CurMana-=TombManaCost;
            TombCoolDown=true;
            CastSpell(20f,"Tomb",()=>TombCoolDown=false);
        }
        Debug.Log("Tomb");
        animator.SetTrigger("cast6");
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
            Instantiate(_tomb, hit.point, new Quaternion());
        }
    }
    
    //SMOKE
    bool SmokeCoolDown = false;
    float SmokeManaCost=20f;
    [SerializeField] GameObject _smoke;
    void Smoke(int fire, int water){
        if(!GodMod){
            if(SmokeCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<SmokeManaCost) {NotEnoughtMana(); return;}
            stats.CurMana-=SmokeManaCost;
            SmokeCoolDown=true;
            CastSpell(30f,"Smoke",()=>SmokeCoolDown=false);
        }
        Debug.Log("Smoke");
        animator.SetTrigger("cast4");
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
            smoke_script smoke=Instantiate(_smoke, hit.point, new Quaternion()).GetComponent<smoke_script>();
            smoke.fire=fire;
            smoke.water=water;
        }
    }
    
    //FLAME
    bool FlameCoolDown=false;
    float FlameManaCost=5f;
    float flameDelay=0.2f;
    [SerializeField] ParticleSystem _flame;
    void Flame(int strength){
        if(!GodMod){
            if(FlameCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<FlameManaCost) {NotEnoughtMana(); return;}
        }
        Debug.Log("Flame");
        StopMoving(true);
        currentCast=StartCoroutine(flame_core());
        BreakCastEvent.AddListener(()=>{
            _flame.Stop();
            if(!GodMod)CastSpell(5f,"Flame",()=>FlameCoolDown=false);
        });

        IEnumerator flame_core(){
            animator.SetTrigger("cast_flame");
            animator.SetBool("casting", true);
            yield return new WaitForSeconds(flameDelay);
            FlameCoolDown=true;
            _flame.Play();
            while(Input.GetMouseButton(1)){
                if(stats.CurMana<FlameManaCost) break;
                stats.CurMana-=FlameManaCost;
                Collider[] colliders = Physics.OverlapSphere(player.transform.position+player.transform.forward, 1.5f*strength, enemies);
                foreach(Collider enemy in colliders){
                    enemy.GetComponent<unit_script>().TakeDamage(5f*buffs.GetStats(Stats.fireDamage)*buffs.GetStats(Stats.damage)*strength, DamageType.Fire);
                }
                yield return new WaitForSeconds(.5f);
            }
            StopMoving(false);
            _flame.Stop();
            BreakCastEvent.RemoveListener(()=>{
                _flame.Stop();
                if(!GodMod)CastSpell(5f,"Flame",()=>FlameCoolDown=false);
            });
            animator.SetBool("casting", false);
            if(!GodMod)CastSpell(5f,"Flame",()=>FlameCoolDown=false);
        }
    }

    //LIFE DRAIN
    bool LifeDrainCoolDown=false;
    float LifeDrainManaCost=40f;
    [SerializeField] ParticleSystem _blood;
    void LifeDrain(){
        if(!GodMod){
            if(LifeDrainCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<LifeDrainManaCost) {NotEnoughtMana(); return;}
            stats.CurMana-=LifeDrainManaCost;
            LifeDrainCoolDown=true;
            CastSpell(45f,"Life Drain",()=>LifeDrainCoolDown=false);
        }
        Debug.Log("Life Drain");
       
        bool casting=true;
        float _damage=buffs.GetStats(Stats.damage)+buffs.GetStats(Stats.physicalDamage)*5;
        StartCoroutine(cast());

        IEnumerator cast(){
            animator.SetBool("casting", true);
            animator.SetTrigger("cast_blast");
            StopMoving(true);
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, 5f, enemies);
            foreach (Collider i in colliders){
                if(i.tag!="enemy") continue;
                StartCoroutine(bloodTail(Instantiate(_blood, i.transform, false)));
            }
            while(Input.GetMouseButton(1)){
                foreach (Collider i in colliders){
                    if(i==null) continue;
                    if(i.tag!="enemy") continue;
                    unit_script enemy =  i.gameObject.GetComponent<unit_script>();
                    enemy.TakeDamage(_damage, DamageType.Physical);
                    stats.AddHP(_damage);
                }
                stats.CurMana-=5;
                if(stats.CurMana<=0) break;
                yield return new WaitForSeconds(.5f);
            }
            StopMoving(false);
            casting=false;
            animator.SetBool("casting", false);
        }
        

        IEnumerator bloodTail(ParticleSystem blood){
            while(casting && blood!=null){
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[blood.particleCount];
                blood.GetParticles(particles);
                for(int i=0;i<blood.particleCount;i++){
                    particles[i].velocity=(player.transform.position+Vector3.up*.5f-particles[i].position).normalized*2;
                    if((player.transform.position-particles[i].position).sqrMagnitude<.5f) particles[i].remainingLifetime=.1f;
                }
                blood.SetParticles(particles);
                yield return new WaitForSeconds(.2f);
            }
            if(blood!=null)blood.Stop(false);
        }
    }
    
    //SHIELD
    bool ShieldCoolDown=false;
    float ShieldManaCost=40f;
    float ShieldDelay=0.3f;
    [SerializeField] GameObject shield;
    void Shield(){
        if(!GodMod){
            if(ShieldCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<ShieldManaCost) {NotEnoughtMana(); return;}
        }
        Debug.Log("Shield");
        navMesh.isStopped=true;
        currentCast=StartCoroutine(shield_core2());

        IEnumerator shield_core(){
            animator.SetBool("casting", true);
            animator.SetTrigger("cast_global");
            yield return new WaitForSeconds(ShieldDelay);
            if(!GodMod) stats.CurMana-=ShieldManaCost;
            
            GameObject _shield=Instantiate(shield,transform);
            while(Input.GetMouseButton(1)){
                if(stats.CurMana<0) break;
                if(!GodMod) stats.CurMana-=10*Time.fixedDeltaTime;
                yield return new WaitForSeconds(0.1f);
            }
            ShieldCoolDown=true;
            animator.SetBool("casting", false);
            if(!GodMod)CastSpell(20f,"Shield",()=>ShieldCoolDown=false);
            Destroy(_shield,0.5f);
        }
        IEnumerator shield_core2(){
            animator.SetTrigger("cast_global");
            yield return new WaitForSeconds(ShieldDelay);
            if(!GodMod) stats.CurMana-=ShieldManaCost;
            
            GameObject _shield=Instantiate(shield,transform.position, new Quaternion());
            ShieldCoolDown=true;
            if(!GodMod)CastSpell(20f,"Shield",()=>ShieldCoolDown=false);
            Destroy(_shield,10f);
        }
    }

    //WALL
    bool WallCoolDown=false;
    float WallManaCost=40f;
    float WallDelay=0.3f;
    [SerializeField] GameObject wall;

    void Wall(int air, int earth){
        if(!GodMod){
            if(WallCoolDown) {CoolDawn(); return;}
            if(stats.CurMana<WallManaCost) {NotEnoughtMana(); return;}
        }
        Debug.Log("wall");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
            currentCast=StartCoroutine(wall_core(hit.point));
        }
        IEnumerator wall_core(Vector3 pos){
            animator.SetTrigger("");
            yield return new WaitForSeconds(WallDelay);
            stats.CurMana-=WallManaCost;
            WallCoolDown=true;
            CastSpell(30f,"Wall",()=>WallCoolDown=false);
            Transform tf = Instantiate(wall, pos, new Quaternion()).transform;
            tf.LookAt(gameObject.transform);
            tf.eulerAngles = new Vector3(0,tf.eulerAngles.y, 0);
        }
    }
#endregion
}
