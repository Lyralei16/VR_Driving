using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAI : MonoBehaviour
{
    GameController _GameController; 
    //public bool Activated;
    //float speed = 5; // driving speed

    [HideInInspector]
    public float speed = 5; // driving speed
    float raycastingHeight = 1f;
    private float raycastCooldown; // The time before we raycast again
    private Vector3 halfSize = new Vector3(1, 0.75f, 2); // half the size of the car collider
    private float checkFrequency = 0.1f;

    //Declaring all the variables from the other scripts
    void Start() {
        _GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        if (_GameController != null) { // if GameController found
        speed = _GameController.obstacleSpeed;
       // Activated = _GameController.obstacleAvoidanceAI;


        speed *= Random.Range(.95f, 1.05f); //Randomizing speed

        _GameController.shiftGear.AddListener(AdjustSpeed);

        }
    }

	// Update is called once per frame
	void Update () {
		//transform.Translate(transform.forward*Time.deltaTime*speed); ! Commented this out, since shouldn't be necessary
        
        

        


        //only if tagged in gamecontroller
        if(_GameController.obstacleAvoidanceAI){
            // We want to check only every 0.5-1 seconds
	        raycastCooldown -= Time.deltaTime;
	        if (raycastCooldown < 0 && !IsAvoiding)
	        {
	            raycastCooldown = Random.Range(checkFrequency, 2*checkFrequency);


                //Debug.Log("collision: " + CheckObstaclesInDirection(transform.forward, 10));

	             if(CheckObstaclesInDirection(transform.forward, 20)){
                    AvoidObstacle();
                };

	        }
        }
        
	}

    private bool IsAvoiding { get; set; }

    private bool CheckObstaclesInDirection(Vector3 direction, int distance)
    {

        bool hit = false;

        RaycastHit obstacle;
        
        Debug.DrawRay(new Vector3(transform.position.x,transform.position.y + raycastingHeight, transform.position.z), -direction * distance, Color.red, 10f);

        if(Physics.Raycast (new Vector3(transform.position.x,transform.position.y + raycastingHeight, transform.position.z), -direction, out obstacle, distance, -1)){

            Debug.Log("hit: " + obstacle.transform.name);

            hit = true;
        }

        return hit;
    }

    private void AvoidObstacle(){   

        Debug.Log("avoid");

        // Look right
        if (!CheckObstaclesInDirection(-transform.right, 12)) SwitchToLane(1f);
        else if (!CheckObstaclesInDirection(transform.right, 12)) SwitchToLane(-1f);
        else StartCoroutine(BrakeSequence(-speed));
    }

    private IEnumerator BrakeSequence(float offset){
        IsAvoiding = true;

        float duration = 0.3f;
        float totalDuration = duration;

        while (duration > 0)
        {
            duration -= Time.deltaTime;
            speed += Time.deltaTime * offset / totalDuration;
            yield return null;
        }

        IsAvoiding = false;
        raycastCooldown = checkFrequency * 4;
    }

    private void SwitchToLane(float distance)
    {
        // Store new x position
        StartCoroutine(SwitchLaneSequence(distance));
    }

    private IEnumerator SwitchLaneSequence(float distance)
    {
        IsAvoiding = true;
        float offset = distance * (10f - transform.localScale.x);

       
        float duration = 10/speed;
        float totalDuration = duration;

        //Debug.Log(totalDuration);

        while ( duration > 0 ){

            duration -= Time.deltaTime;
            transform.Translate(offset*Time.deltaTime/totalDuration, 0, 0);
            //transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime), transform.position.y, transform.position.z);

            yield return null;
        }

        IsAvoiding = false;
        raycastCooldown = checkFrequency*4;
    }

    /*
    *    Adjust speed according to player
    */
    public void AdjustSpeed(){
  
        speed *= (_GameController.speed / 100);

    }


}