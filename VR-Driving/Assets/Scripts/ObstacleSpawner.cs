using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public float speed;
    private float endZ = -3;
    public float startZ;


    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.back * speed * Time.deltaTime);
        speed = Random.Range(15, 45);
        startZ = Random.Range(70, 120); 

        if (transform.position.z <= endZ)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, startZ);
            transform.position = pos;
        }

    }
    public void OnCollisionEnter (Collision col) {
        if (col.gameObject.name == "PlayerCar") {
            Destroy(col.gameObject);
            Debug.Log("collision happened");
        }
        

    }
    
}

