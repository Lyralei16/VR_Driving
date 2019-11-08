using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evironment : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
   // private Transform randSpot;
    private float enemyCount;
    private int rand;
    public GameObject[] enemy;
    Vector2 spotPos;

    // Start is called before the first frame update
    void Start()
    {
       // randSpot.position = new Vector2(Random.Range(minX, minY), Random.Range(maxX, maxY));
        enemyCount = Random.Range(2, 5);
        rand = Random.Range(1, enemy.Length);
        spotPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyCount > 0){
            enemyCount--;
            Instantiate(enemy[rand], spotPos, Quaternion.identity);
            spotPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        } else{
            Debug.Log("Coordinates are:  " + spotPos);
        }
    }
}
