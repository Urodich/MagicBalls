using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerControl_script : MonoBehaviour
{
    //public float distance;
    //public float rotateSpeed;
    //public float speed = 5f;
    public Vector3 mousePos;
    [SerializeField] LayerMask layer;
    Spells_script spells_Script;
    public NavMeshAgent navMesh;
    [SerializeField] Animator animator;
    player_script player;
    public GameObject mouseTarget;
    [SerializeField] LayerMask enemies;
    [SerializeField] Transform sphereParent;
    Queue<int> Spheres = new Queue<int>(3);
    
    void Start()
    {
        spells_Script = gameObject.GetComponent<Spells_script>();
        navMesh = gameObject.GetComponent<NavMeshAgent>();
        player = gameObject.GetComponent<player_script>();
    }
    bool moonwalk=false;
    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //выбор цели
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, enemies)){
            if(mouseTarget!=hit.collider.gameObject) {
                if(mouseTarget!=null)mouseTarget.GetComponent<unit_script>().Lighting(false);
                mouseTarget=hit.collider.gameObject;
                mouseTarget.GetComponent<unit_script>().Lighting(true);
            }
        }
        else{
            if(mouseTarget!=null)mouseTarget.GetComponent<unit_script>().Lighting(false);
            mouseTarget=null;
        }

        if(player.isStunned) {
            return;
            }
        //передвижение
        if(Physics.Raycast(ray, out RaycastHit groundHit, float.MaxValue, layer)){
            mousePos= groundHit.point;
            transform.LookAt(mousePos);
        }
        if(Input.GetMouseButton(0)) navMesh.destination = mousePos; 
        if (navMesh.velocity==Vector3.zero) {
            animator.SetBool("moonwalk", false);
            animator.SetBool("run", false);
            }
        else{
            animator.SetBool("run", true);
        }
        if (Vector3.Angle(navMesh.velocity, gameObject.transform.forward)>90) {
            if(!moonwalk){
                navMesh.speed=player.speed*.5f;
                animator.SetBool("moonwalk", true);
                }
            moonwalk=true;
            }
        else {
            if(moonwalk){
                navMesh.speed=player.speed;
                animator.SetBool("moonwalk", false);
                }
            moonwalk=false;
            }
    }
    //
    //CASTS
    //
    void Update(){
        if(Input.GetKeyDown("q")) CastSphere(0); //fire 1
        if(Input.GetKeyDown(KeyCode.W)) CastSphere(1); //water 4
        if(Input.GetKeyDown("e")) CastSphere(2); //air 13
        if(Input.GetKeyDown(KeyCode.A)) CastSphere(3); //earth 40
        if(Input.GetKeyDown(KeyCode.S)) CastSphere(4); //life 121
        if(Input.GetKeyDown(KeyCode.D)) CastSphere(5); //thunder 364
//{if (Spheres.Count == 3) Spheres.Dequeue(); Spheres.Enqueue(364);}
        if(Input.GetMouseButtonDown(1)){
            int sum = 0;
            while (Spheres.Count !=0 ){
                sum+=Spheres.Dequeue();
                Destroy(spheres.Dequeue().gameObject);
                }
            spells_Script.CastSpell(sum);
            sum=0;
        }
    }
    [SerializeField]List<ParticleSystem> sphere = new List<ParticleSystem>();
    Queue<ParticleSystem> spheres = new Queue<ParticleSystem>();
    void CastSphere(int i){
        if (Spheres.Count == 3) {
            Spheres.Dequeue();
            Destroy(spheres.Dequeue().gameObject);
            } 
        switch(i){
            case 0:{
                Spheres.Enqueue(1);
                spheres.Enqueue(Instantiate(sphere[i],sphereParent));
                break;
                }
            case 1:{
                Spheres.Enqueue(4);
                spheres.Enqueue(Instantiate(sphere[i],sphereParent));
                break;
                }
            case 2:{
                Spheres.Enqueue(13);
                spheres.Enqueue(Instantiate(sphere[i],sphereParent));
                break;
                }
            case 3:{
                Spheres.Enqueue(40);
                spheres.Enqueue(Instantiate(sphere[i],sphereParent));
                break;
                }
            case 4:{
                Spheres.Enqueue(121);
                spheres.Enqueue(Instantiate(sphere[i],sphereParent));
                break;
                }
            case 5:{
                Spheres.Enqueue(364);
                spheres.Enqueue(Instantiate(sphere[i],sphereParent));
                break;
                }
        }
    }
}
