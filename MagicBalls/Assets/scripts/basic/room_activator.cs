using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class room_activator : MonoBehaviour
{
    [SerializeField] List<enemy_script> enemies = new List<enemy_script>();
    [SerializeField] UnityEvent<room_activator> action;

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.tag!="Player") return;

        foreach ( enemy_script enemy in enemies){
            enemy.Activate(true);
            enemy.dieEvent+=(en)=>RemoveEnemy(en);
        }
        action?.Invoke(this);
    }

    public void AddEnemy(enemy_script enemy){
        enemies.Add(enemy);
        enemy.dieEvent+=(en)=>RemoveEnemy(en);
    }

    void RemoveEnemy(GameObject enemy){
        enemies.Remove(enemy.GetComponent<enemy_script>());
        if(enemies.Count==0) {Loader.GetInstance().SaveWithDelay(); Destroy(gameObject);}
    }
}
