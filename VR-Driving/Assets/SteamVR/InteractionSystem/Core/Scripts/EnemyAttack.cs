using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float speed = 3;
    public float stoppingDistance = 5;
    public float retreatDistance = 3;
    public float TimeBtwShots = 2; 
    public float startTimeBtwShots; 
    private Transform player;
    private Transform enemy;
    public GameObject projectile;
    public bool attack = true;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        TimeBtwShots = startTimeBtwShots;
        enemy = GameObject.FindGameObjectWithTag("Respawn").transform; 
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, player.position) > stoppingDistance){
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        } else if(Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance){
            transform.position = this.transform.position;

        } else if(Vector2.Distance(transform.position, player.position) < retreatDistance){
            
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);

        } else if(Vector2.Distance(transform.position, enemy.position) < retreatDistance){
            
            transform.position = Vector2.MoveTowards(transform.position, enemy.position, -speed * Time.deltaTime);

        }
        if(TimeBtwShots <= 0 && attack == true){
            Instantiate(projectile, transform.position, Quaternion.identity);
            TimeBtwShots = startTimeBtwShots;
            

        }
        else{
            TimeBtwShots -= Time.deltaTime;
        }
    }
}
