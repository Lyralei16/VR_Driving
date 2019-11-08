using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed = 10;
    private float waitTime;
    public float StartWaitTime = 2;
    public Transform moveSpots;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    
    // Start is called before the first frame update
    void Start()
    {
        moveSpots.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        waitTime = StartWaitTime;
       
    }

    // Update is called once per frame
    void Update()
    {
        CheckPos();
       transform.position = Vector2.MoveTowards(transform.position, moveSpots.position, speed * Time.deltaTime);
        if(Vector2.Distance(transform.position, moveSpots.position) < 0.2f){
            if(waitTime <= 0){
                 moveSpots.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                 waitTime = StartWaitTime;

            } else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
    void CheckPos(){
        Debug.Log("Enemy moved to" + moveSpots.position);
    }
}
