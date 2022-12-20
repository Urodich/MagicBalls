using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


public class Spells_script : MonoBehaviour
{
    public  Dictionary<int, System.Action> Spells;
    GameObject player;
    player_script stats;
    playerControl_script control;
    NavMeshAgent navMesh;
    buffs_script buffs;
    [SerializeField]LayerMask enemies;
    [SerializeField]LayerMask ground;
    GameObject spellPanel;
    [SerializeField] GameObject spellCooDown;
    [SerializeField] public bool GodMod {get;set;}=false;
    [SerializeField] Animator animator;
    public Coroutine currentCast;
    void Start(){
        spellPanel=GameObject.Find("SpellPanel");
        player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<player_script>();
        navMesh = player.GetComponent<NavMeshAgent>();
        buffs = player.GetComponent<buffs_script>();
        control=player.GetComponent<playerControl_script>();
        _flame.Stop();
        Spells = new Dictionary<int, System.Action>(){
        [0]=castEmpty,
        [1] = ()=>Flame(1),
        [2] = ()=>Flame(2),
        [3] = ()=>Flame(3),
        [4] = ()=>Wave(1),
        [5] = ()=>Smoke(1,1),
        [6] = ()=>Smoke(2,1),
        [8] = ()=>Wave(2),
        [9] = ()=>Smoke(1,2),
        [12] = ()=>Wave(3),
        [13] = ()=>Wind(1),
        //[14] = Fireball(1,1),
        //[15] = Fireball(1,2),
        [17] = ()=>Torrent(1,1),
        [18] = ()=>FireStorm(),
        [21] = ()=>Torrent(1,2),
        [26] = ()=>Wind(2),
        //[27] = Fireball(2,1),
        [30] = ()=>Torrent(2,1),
        [39] = ()=>Wind(3),
        [40] = ()=>Earhtquake(1),
        [41] = ()=>Lava(1,1),
        [42] = ()=>Lava(1,2),
        [44] = ()=>Mud(1,1),
        [48] = ()=>Mud(2,1),
        [54] = ()=>Meteor(),
        [80] = ()=>Earhtquake(2),
        [81] = ()=>Lava(2,1),
        [84] = ()=>Mud(1,2),
        [120] = ()=>Earhtquake(3),
        [121] = ()=>Wisp(1),
        [122] = ()=>Phoenix(1,1),
        [123] = ()=>Phoenix(1,2),
        [125] = ()=>ManaRegen(1,1),
        [126] = ()=>LifeDrain(),
        [129] = ()=>ManaRegen(2,1),
        [134] = ()=>Bird(1,1),
        [147] = ()=>Bird(2,1),
        [174] = ()=>Tornado(),
        [242] = ()=>Wisp(2),
        [243] = ()=>Phoenix(2,1),
        [246] = ()=>ManaRegen(1,2),
        [255] = ()=>Bird(1,2),
        [363] = ()=>Wisp(3),
        [364] = ()=>Chain(1),
        [365] = ()=>Blast(1,1),
        [366] = ()=>Blast(2,1),
        [368] = ()=>ElectricPool(1,1),
        [369] = ()=>MindControl(),
        [372] = ()=>ElectricPool(2,1),
        [377] = ()=>Haste(1,1),
        [378] = ()=>BlackHole(),
        [388] = ()=>Haste(1,2),
        [404] = ()=>Blink(1,1),
        [444] = ()=>Blink(2,1),
        [485] = ()=>Zombie(1,1),
        [486] = ()=>Reincarnation(),
        [489] = ()=>Illusion(),
        [525] = ()=>Tomb(),
        [606] = ()=>Zombie(2,1),
        [728] = ()=>Chain(2),
        [729] = ()=>Blast(1,2),
        [732] = ()=>ElectricPool(1,2),
        [741] = ()=>Haste(2,1),
        [768] = ()=>Blink(1,2),
        [849] = ()=>Zombie(1,2),
        [1092] = ()=>Chain(3)
        };
    }

    public void CastSpell(int hex){
        Debug.Log("Casting" + hex+"  currentCor"+currentCast);
        if (Spells.ContainsKey(hex))
            Spells[hex]();
        else
            Spells[0]();
    }
    void NotEnoughtMana(){
        Debug.Log("NotMana");
    }
    void CoolDawn(){
        Debug.Log("CoolDown");
    }
    void castEmpty(){
        Debug.Log("empty");
        animator.SetTrigger("");
    }
    //create icon
    void CastSpell(float CDTime, String imagePath, Action func){
        Sprite image=Resources.Load<Sprite>(imagePath);
        if(image==null) Debug.Log("no Sprite loaded");
        GameObject obj = Instantiate(spellCooDown);
        obj.transform.SetParent(spellPanel.transform,false);
        Timer spell = obj.GetComponent<Timer>();
        spell.func=func;
        spell.SrcImage=image;
        spell.SetTime(CDTime,true);
    }

    //FIREBALL
    [SerializeField] GameObject fireball;
    bool fireballCollDown=false;
    float fireballManaCost=10f;
    float fireballDelay=0.5f;
    void Fireball(float damage, float speed){
        if(!GodMod){
            if(fireballCollDown) {CoolDawn(); return;}
            if(stats.CurMana<fireballManaCost) {NotEnoughtMana(); return;}
        }
        currentCast=StartCoroutine(fireball_core());
        IEnumerator fireball_core(){
            Debug.Log("fireball");
            animator.SetTrigger("cast8");
            yield return new WaitForSeconds(fireballDelay);
            if(!GodMod){
                stats.CurMana-=fireballManaCost;
                fireballCollDown=true;
                CastSpell(2f,"FireBall",()=>fireballCollDown=false);
            }
            fireball_script fb = Instantiate(fireball, player.transform.position+player.transform.forward, new Quaternion()).GetComponent<fireball_script>();
            fb.damage *=damage;
            fb.speed *=speed;
            for(int i = 1; i < buffs.projectile; i++){
                fb = Instantiate(fireball, player.transform.position+player.transform.forward-player.transform.right*i*0.3f, new Quaternion()).GetComponent<fireball_script>();
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
                        i.gameObject.GetComponent<enemy_script>().TakeDamage(5*buffs.damage*buffs.fireDamage*strength, DamageType.Fire);
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
            for(int i = 0; i < buffs.projectile; i++){
                GameObject wave = Instantiate(waveOrig, player.transform.position+player.transform.forward+player.transform.right*i*0.3f, new Quaternion());
                wave.transform.localScale.Scale(new Vector3(scale,1,scale));
                if(buffs.projectile==1) yield break;
                wave = Instantiate(waveOrig, player.transform.position+player.transform.forward+player.transform.right*i*0.3f, new Quaternion());
                wave.transform.localScale.Scale(new Vector3(scale,1,scale));
            }
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
            yield return new WaitForSeconds(earhtquakeDelay);

            if(!GodMod){
                stats.CurMana-=EarhtquakeManaCost*strength;
                EarthqukeCoolDowm=true;
                CastSpell(7f*strength,"Earthquake",()=>EarthqukeCoolDowm=false);
            }
            _earthquake.Play();
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, 2*strength, enemies);
            foreach (Collider collider in colliders){
                if (collider.tag!="enemy") continue;
                enemy_script enemy = collider.gameObject.GetComponent<enemy_script>();
                if(enemy.isFlying) continue;
                enemy.Stun(0.5f*strength);
                enemy.TakeDamage(strength*20*buffs.damage*buffs.physicalDamage, DamageType.Physical);
            }
        }
    }
    //BLINK
    bool blinkCollDown=false;
    float blinkManaCost=15f;
    [SerializeField] ParticleSystem _blink;
    void Blink(int earth, int thunder){
        Vector3 mousePos;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
            if(!GodMod){
                if(blinkCollDown) return;
                if(stats.CurMana<blinkManaCost*thunder) return;
                stats.CurMana-=blinkManaCost*thunder;
                blinkCollDown=true;}
            CastSpell(5f,"Blink",()=>blinkCollDown=false);

            Debug.Log("blink");
            animator.SetTrigger("jump");
            mousePos= hit.point;
            player.GetComponent<playerControl_script>().navMesh.enabled=false;
            player.transform.position=mousePos;
            player.GetComponent<playerControl_script>().navMesh.enabled=true;
            player.GetComponent<playerControl_script>().navMesh.destination=mousePos;
            _blink.Play();
            Collider[] colliders = Physics.OverlapCapsule(player.transform.position + new Vector3(0,1,0), player.transform.position, 2*earth, enemies.value);
            foreach (Collider collider in colliders){
                if (collider.tag!="enemy") continue;
                enemy_script enemy = collider.gameObject.GetComponent<enemy_script>();
                if(enemy.isFlying) continue;
                enemy.Stun(0.5f*earth);
                enemy.TakeDamage(thunder*20*buffs.thunderDamage*buffs.damage, DamageType.Thunder);
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
            stats.CurMana-=windManaCost*strength;
            windCoolDown=true;}
        
        
        Vector3 mousePos1 = new Vector3();
        Vector3 mousePos2;
        Vector3 direction = new Vector3();

        StartCoroutine(VectorCast());

        IEnumerator VectorCast(){
            yield return StartCoroutine(vectorCast());
            animator.SetTrigger("cast9");
            wind_script wind = Instantiate(windOrig,mousePos1,Quaternion.LookRotation(direction)).GetComponent<wind_script>();
            wind.strength=strength;
            wind.direction=direction;
            CastSpell(15f*strength,"Wind",()=>windCoolDown=false);
        }

        //векторное применение
        IEnumerator vectorCast(){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)) yield break;
            mousePos1=hit.point;
            mousePos2=mousePos1+Vector3.forward;
            yield return new WaitUntil(()=> Input.GetMouseButtonUp(1));
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out RaycastHit hit2, float.MaxValue, ground)){
                    mousePos2=hit2.point;
                }
            
            direction = (mousePos2-mousePos1);
            direction.Scale(new Vector3(1,0,1));
            direction.Normalize();
        }
    }

    //TORRENT
    [SerializeField] GameObject torrentOrig;
    bool torrentCoolDown = false;
    float torrentManaCost = 20f;
    void Torrent(int air, int water){
        if(!GodMod){
            if(torrentCoolDown) return;
            if(stats.CurMana<torrentManaCost*water) return;
            stats.CurMana-=torrentManaCost*water;
            torrentCoolDown=true;}
        CastSpell(10f*air,"Torrent",()=>torrentCoolDown=false);

        Debug.Log("torrent");
        animator.SetTrigger("cast1");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
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
            if(mudCoolDown) return;
            if(stats.CurMana<mudManaCost*earth) return;
            stats.CurMana-=mudManaCost*earth;
            mudCoolDown=true;}
        CastSpell(7f*earth,"Mud",()=>mudCoolDown=false);
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
            if(lavaCoolDown) return;
            if(stats.CurMana<lavaManaCost*fire) return;
            stats.CurMana-=lavaManaCost*fire;
            lavaCoolDown=true;}
        CastSpell(15f*earht,"Lava",()=>lavaCoolDown=false);
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
            if(ElectricPoolCoolDown) return;
            if(stats.CurMana<ElectricPoolManaCost*((thunder-1)/2+1)) return;
            stats.CurMana-=ElectricPoolManaCost*((thunder-1)/2+1);
            ElectricPoolCoolDown=true;}
        CastSpell(15f*water,"Pool",()=>ElectricPoolCoolDown=false);
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
            if(HasteCoolDown) return;
            if(stats.CurMana<HasteManaCost*thunder) return;
            stats.CurMana-=HasteManaCost*thunder;
            HasteCoolDown=true;}
        CastSpell(10f*air,"Haste",()=>HasteCoolDown=false);

        value=.5f*thunder;
        player.GetComponent<unit_script>().ChangeSpeed(value);
        animator.SetBool("haste",true);
        Invoke("Remove", 5*air);
    }
    void Remove(){player.GetComponent<unit_script>().ChangeSpeed(-value);animator.SetBool("haste",false);} //mb change later
    
    //Lightning chain
    bool chainCoolDown=false;
    float chainManaCost=15f;
    void Chain(int strength){
        if(!GodMod){
            if(chainCoolDown) return;
            if(stats.CurMana<chainManaCost*strength) return;
            stats.CurMana-=chainManaCost*strength;
            chainCoolDown=true;}
        CastSpell(3f,"Chain",()=>chainCoolDown=false);
        animator.SetTrigger("cast8");
        float radius = 2f;
        int steps=5+strength*2;
        Vector3 oldPoint;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
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
                    enemy_script enemy = col.GetComponent<enemy_script>();
                    enemy.TakeDamage(15*buffs.thunderDamage*buffs.damage, DamageType.Thunder);
                    last=col;
                    hit=true;
                    steps--;
                    oldPoint=col.gameObject.transform.position;
                    break;
                }
                if(!hit) break;
                yield return new WaitForSeconds(.1f);
            }
        }
    }
    
    //ILLUSION
    [SerializeField] GameObject _illusion;
    bool illusionCoolDown=false;
    float illusionManaCost=20f;
    void Illusion(){
        if(!GodMod){
            if(illusionCoolDown) return;
            if(stats.CurMana<illusionManaCost) return;
            stats.CurMana-=illusionManaCost;
            illusionCoolDown=true;}
        CastSpell(10f,"Illusion",()=>illusionCoolDown=false);
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
            if(reincarnationCoolDown) return;
            if(stats.CurMana<reincarnationManaCost) return;
            stats.CurMana-=reincarnationManaCost;
            reincarnationCoolDown=true;}
        CastSpell(90f,"Reincarnation",()=>reincarnationCoolDown=false);

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
            if(meteorCoolDown) return;
            if(stats.CurMana<meteorManaCost) return;
            stats.CurMana-=meteorManaCost;
            meteorCoolDown=true;}
        

        Vector3 mousePos1 = new Vector3();
        Vector3 mousePos2;
        Vector3 direction = new Vector3();

        StartCoroutine(VectorCast());

        IEnumerator VectorCast(){
            yield return StartCoroutine(vectorCast());
            meteor_script meteor = Instantiate(_meteor,mousePos1+new Vector3(0,4,0),Quaternion.LookRotation(direction)).GetComponent<meteor_script>();
            meteor.direction=direction;
            animator.SetTrigger("cast4");
            CastSpell(20f,"Meteor",()=>meteorCoolDown=false);
        }

        //векторное применение
        IEnumerator vectorCast(){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)) yield break;
            mousePos1=hit.point;
            mousePos2=mousePos1+Vector3.forward;
            yield return new WaitUntil(()=> Input.GetMouseButtonUp(1));
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out RaycastHit hit2, float.MaxValue, ground)){
                    mousePos2=hit2.point;
                }
            
            direction = (mousePos2-mousePos1);
            direction.Scale(new Vector3(1,0,1));
            direction.Normalize();
        }
    }

    //MANA REGEN
    bool ManaRegenCoolDown=false;
    float ManaRegenManaCost=5f;
    [SerializeField] ParticleSystem _manaRegen;
    void ManaRegen(int water, int life){
        if(!GodMod){
            if(ManaRegenCoolDown) return;
            if(stats.CurMana<ManaRegenManaCost*life) return;
            stats.CurMana-=ManaRegenManaCost*life;
            ManaRegenCoolDown=true;}
        
        Debug.Log("ManaRegen");
        StartCoroutine(cast());
        IEnumerator cast(){
            _manaRegen.Play();
            navMesh.isStopped=true;
            player_script ps = player.GetComponent<player_script>();
            ps.manaRegen+=5f*water;
            float ownArmor=ps.armor;
            ps.armor+=15*life;
            yield return new WaitUntil(()=>Input.GetMouseButtonUp(1));
            ps.manaRegen-=5f*water;
            if(ps.armor>ownArmor) ps.armor=ownArmor;
            navMesh.isStopped=false;
            CastSpell(2f,"Mana Regen",()=>ManaRegenCoolDown=false);
            _manaRegen.Stop(true);
        }
    }
    
    //BIRD
    bool BirdCoolDown=false;
    float BirdManaCost=10f;
    [SerializeField] ParticleSystem _bird;
    void Bird(int air, int life){
        if(!GodMod){
            if(BirdCoolDown) return;
            if(stats.CurMana<BirdManaCost) return;
            stats.CurMana-=BirdManaCost;
            BirdCoolDown=true;}
        CastSpell(10f,"Bird",()=>BirdCoolDown=false);
        StartCoroutine(cast());
        IEnumerator cast(){
            _bird.Play();
            yield return new WaitForSeconds(.6f);
            Debug.Log("Bird");
            animator.SetTrigger("cast6");
            Collider[] enemy = Physics.OverlapSphere(player.transform.position, 3f*life,enemies);
            foreach(Collider i in enemy){
                if(i.tag!="enemy") continue;
                enemy_script unit = i.gameObject.GetComponent<enemy_script>();
                unit.TakeDamage(1f,DamageType.Physical);
                Vector3 dir=(i.gameObject.transform.position-player.transform.position);
                dir.Scale(new Vector3(1,0,1));
                unit.Move(dir.normalized, 2f*air*buffs.repulsion, 1f, false);
            }
        }
        
    }
    
    //WISP
    bool WispCoolDown=false;
    float WispManaCost=15f;
    void Wisp(int strength){
        if(!GodMod){
            if(windCoolDown) return;
            if(stats.CurMana<windManaCost*strength) return;
            stats.CurMana-=windManaCost*strength;
            windCoolDown=true;}
        CastSpell(15*strength,"Wisp",()=>windCoolDown=false);

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
            if(tornadoCoolDown) return;
            if(stats.CurMana<tornadoManaCast) return;
            stats.CurMana-=tornadoManaCast;
            tornadoCoolDown=true;}
        CastSpell(2f,"Tornado",()=>tornadoCoolDown=false);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ground)){
            Debug.Log("Tornado");
            animator.SetTrigger("cast9");
            Instantiate(_tornado, hit.point, player.transform.rotation).GetComponent<illusion_script>();
        }
    }

    //BLAST
    bool blastCoolDown=false;
    float blastManaCost=30f;
    void Blast(int fire, int thunder){
        if(!GodMod){
            if(blastCoolDown) return;
            if(stats.CurMana<blastManaCost) return;
            stats.CurMana-=blastManaCost;
            blastCoolDown=true;}
        
        Debug.Log("blast");
        float time=0f;
        StartCoroutine(cast());

        IEnumerator cast(){
            animator.SetTrigger("cast_blast");
            animator.SetBool("casting", true);
            while(Input.GetMouseButton(1)){
                time+=Time.deltaTime;
                if(time>=3f) break;
                yield return new WaitForEndOfFrame();
            }
            animator.SetBool("casting", false);    
            Collider[] enemy = Physics.OverlapSphere(player.transform.position, 1f*thunder*time, enemies);
            foreach(Collider elem in enemy){
                enemy_script en=elem.gameObject.GetComponent<enemy_script>();
                en.TakeDamage(5*fire*buffs.fireDamage*buffs.damage*time, DamageType.Fire);
                en.Move(elem.gameObject.transform.position-player.transform.position, 2f*fire*time, .5f, false);
            }
            if(time>=3){
                player_script player_=player.GetComponent<player_script>();
                player_.TakeDamage(5*fire*buffs.fireDamage*buffs.damage*time, DamageType.Fire);
                player_.Stun(2f);
            }
            CastSpell(10f,"Blast",()=>blastCoolDown=false);
        }
        
    }

    //ZOMBIE
    bool zombieCoolDown=false;
    float zombieManaCost=15f;
    [SerializeField] GameObject _zombie;
    void Zombie(int life, int thunder){
        if(control.mouseTarget==null) return;
        if(!GodMod){
            if(zombieCoolDown) return;
            if(stats.CurMana<zombieManaCost) return;
            stats.CurMana-=zombieManaCost;
            zombieCoolDown=true;}
        Debug.Log("zombie");
        CastSpell(15f,"Zombie",()=>zombieCoolDown=false);
        animator.SetTrigger("cast4");

        for(int i=0;i<2*life;i++){
            Instantiate(_zombie, control.mouseTarget.transform.position, new Quaternion()).GetComponent<zombie>().SetZombieAim(control.mouseTarget);
        }
        
    }

    //MIND CONTROL
    bool mindCoolDown=false;
    float mindManaCost=50f;
    void MindControl(){
        if(control.mouseTarget==null) return;
        if(!GodMod){
            if(mindCoolDown) return;
            if(stats.CurMana<mindManaCost) return;
            stats.CurMana-=mindManaCost;
            mindCoolDown=true;}
        CastSpell(80f,"Mind Control",()=>mindCoolDown=false);
        animator.SetTrigger("cast4");
        Debug.Log("mind control");

        enemy_script enemy=control.mouseTarget.GetComponent<enemy_script>();
        enemy.enemies=enemies;
        enemy.aims.Clear();
    }

    //BLACKHOLE
    bool BlackHoleCoolDown=false;
    float BlackHoleManaCost=70f;
    [SerializeField] GameObject _blackHole;
    void BlackHole(){
        if(!GodMod){
            if(BlackHoleCoolDown) return;
            if(stats.CurMana<BlackHoleManaCost) return;
            stats.CurMana-=BlackHoleManaCost;
            BlackHoleCoolDown=true;}
        CastSpell(50f,"Black Hole",()=>BlackHoleCoolDown=false);
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
            if(FireStormCoolDown) return;
            if(stats.CurMana<FireStormManaCost) return;
            stats.CurMana-=FireStormManaCost;
            FireStormCoolDown=true;}
        
        Debug.Log("Fire Storm");
        StartCoroutine(cast());
        IEnumerator cast(){
            animator.SetTrigger("cast_global");
            animator.SetBool("casting", true);
            Coroutine fire;
            navMesh.isStopped=true;
            fire=StartCoroutine(damage());
            yield return new WaitWhile(()=>Input.GetMouseButton(1));
            StopCoroutine(fire);
            navMesh.isStopped=false;
            animator.SetBool("casting", false);
            CastSpell(20f,"Fire Storm",()=>FireStormCoolDown=false);
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
            if(TombCoolDown) return;
            if(stats.CurMana<TombManaCost) return;
            stats.CurMana-=TombManaCost;
            TombCoolDown=true;}
        Debug.Log("Tomb");
        animator.SetTrigger("cast6");
        CastSpell(20f,"Tomb",()=>TombCoolDown=false);
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
            if(SmokeCoolDown) return;
            if(stats.CurMana<SmokeManaCost) return;
            stats.CurMana-=SmokeManaCost;
            SmokeCoolDown=true;}
        Debug.Log("Smoke");
        animator.SetTrigger("cast4");
        CastSpell(30f,"Smoke",()=>SmokeCoolDown=false);
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
    [SerializeField] ParticleSystem _flame;
    void Flame(int strength){
        if(!GodMod){
            if(FlameCoolDown) return;
            if(stats.CurMana<FlameManaCost) return;
            FlameCoolDown=true;}
        Debug.Log("Flame");
        StartCoroutine(cast());
        IEnumerator cast(){
            animator.SetTrigger("cast_flame");
            animator.SetBool("casting", true);
            _flame.Play();
            while(Input.GetMouseButton(1)){
                if(stats.CurMana<FlameManaCost) break;
                stats.CurMana-=FlameManaCost;
                Collider[] colliders = Physics.OverlapSphere(player.transform.position+player.transform.forward, 1.5f*strength, enemies);
                foreach(Collider enemy in colliders){
                    enemy.GetComponent<enemy_script>().TakeDamage(5f*buffs.fireDamage*buffs.damage*strength, DamageType.Fire);
                }
                yield return new WaitForSeconds(.5f);
            }
            _flame.Stop();
            animator.SetBool("casting", false);
            CastSpell(5f,"Flame",()=>FlameCoolDown=false);
        }
    }

    //LIFE DRAIN
    bool LifeDrainCoolDown=false;
    float LifeDrainManaCost=40f;
    [SerializeField] ParticleSystem _blood;
    void LifeDrain(){
        if(!GodMod){
            if(SmokeCoolDown) return;
            if(stats.CurMana<SmokeManaCost) return;
            stats.CurMana-=SmokeManaCost;
            SmokeCoolDown=true;}
        Debug.Log("Smoke");
        CastSpell(45f,"Life Drain",()=>SmokeCoolDown=false);
        bool casting=true;
        Debug.Log("life drain");
        float _damage=buffs.damage+buffs.physicalDamage*5;
        StartCoroutine(cast());

        IEnumerator cast(){
            animator.SetBool("casting", true);
            animator.SetTrigger("cast_blast");
            navMesh.isStopped=true;
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, 5f, enemies);
            foreach (Collider i in colliders){
                if(i.tag!="enemy") continue;
                StartCoroutine(bloodTail(Instantiate(_blood, i.transform, false)));
            }
            while(Input.GetMouseButton(1)){
                foreach (Collider i in colliders){
                    if(i==null) continue;
                    if(i.tag!="enemy") continue;
                    enemy_script enemy =  i.gameObject.GetComponent<enemy_script>();
                    enemy.TakeDamage(_damage, DamageType.Physical);
                    stats.AddHP(_damage);
                }
                stats.CurMana-=5;
                if(stats.CurMana<=0) break;
                yield return new WaitForSeconds(.5f);
            }
            navMesh.isStopped=false;
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
    
}
