using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerControl_script : MonoBehaviour
{
    public float distance;
    public float rotateSpeed;
    //public float speed = 5f;
    public Vector3 mousePos;
    [SerializeField]
    public LayerMask layer;
    Spells_script spells_Script;
    public NavMeshAgent navMesh;
    [SerializeField] Animator animator;
    player_script player;
    public GameObject mouseTarget;
    [SerializeField] LayerMask enemies;
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
        if(Input.GetKeyDown("q")) {if (Spheres.Count == 3) Spheres.Dequeue(); Spheres.Enqueue(1);} //fire
        if(Input.GetKeyDown(KeyCode.W)) {if (Spheres.Count == 3) Spheres.Dequeue(); Spheres.Enqueue(4);} //water
        if(Input.GetKeyDown("e")) {if (Spheres.Count == 3) Spheres.Dequeue(); Spheres.Enqueue(13);} //air
        if(Input.GetKeyDown(KeyCode.A)) {if (Spheres.Count == 3) Spheres.Dequeue(); Spheres.Enqueue(40);} //earth
        if(Input.GetKeyDown(KeyCode.S)) {if (Spheres.Count == 3) Spheres.Dequeue(); Spheres.Enqueue(121);} //life
        if(Input.GetKeyDown(KeyCode.D)) {if (Spheres.Count == 3) Spheres.Dequeue(); Spheres.Enqueue(364);} //thunder

        if(Input.GetMouseButtonDown(1)){
            int sum = 0;
            while (Spheres.Count !=0 )
                sum+=Spheres.Dequeue();
            if(spells_Script.Spells.ContainsKey(sum))
                spells_Script.Spells[sum]();
            else
                spells_Script.Spells[0]();
            sum=0;
        }
    }
}
