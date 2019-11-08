/*  This script is responsible for Player's view change in the state of Crash
*    
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotation : MonoBehaviour
{ 



    /* public float speed = Time.deltaTime * 1f; 
    !  A constructor can not use Time because it is exectued before even awaking the script
    */

    // Added public Vector2s to externaly controll random number generation more comfortably
    public Vector2 minMaxX = new Vector2(-10, 10); // for sideways movement
    public Vector2 minMaxY = new Vector2(3, 15); // for vertical movement
    public Vector2 minMaxZ = new Vector2(1, 15); // for forward-backwards movement


    /* 
    * Excluded these due to replecement with customization function and Vector2s
    float xFloat; // left and right
    float YFloat; // for height
    float zFloat; // forrward and backward */
    
    void Start() {
        
        /*  YFloat = Random.Range(3, 15); 
        xFloat = Random.Range(-10, 10);
        zFloat = Random.Range(1, 15); 
        */
        
    }



    // Update is called once per frame
    void Update()
    {
        
        //transform.Rotate(xFloat * speed, YFloat * speed, zFloat * speed);
    }

    /* Function to be called from other scripts executing the crash shake over time
    ?  Eventually change to Coroutine
     */
    public void CrashShake(float maxIntesety, float duration){
        
        StartCoroutine(RotationCrash(maxIntesety, duration));


    }

    IEnumerator RotationCrash(float maxIntesety, float duration){

        Debug.Log("Shaking crash " + transform.name);

        //float currentIntesety = .01f;

        float endTime = Time.time + duration;

        //Debug.Log("current time: " + Time.time + " & endTime: " + endTime);

        Vector3 crashRotation = new Vector3(GetRandomV3().x, GetRandomV3().y, GetRandomV3().z);

    	/* //Exponentially increasing towards maximum specified intensety
        while(currentIntesety < maxIntesety && Time.time < endTime){
            //Debug.Log("current: " + currentIntesety +" crashRotation "+ crashRotation +" rotation: " + transform.rotation.eulerAngles);
            
            transform.Rotate(crashRotation.x * currentIntesety * Time.deltaTime, crashRotation.y * currentIntesety * Time.deltaTime, crashRotation.z * currentIntesety * Time.deltaTime);            
            currentIntesety *= 1.1f;
            yield return null;

            
        } */

        // Decreasing rotation Intensity over time
        while(Time.time < endTime){

            transform.Rotate(crashRotation.x * maxIntesety * Time.deltaTime, crashRotation.y * maxIntesety * Time.deltaTime, crashRotation.z * maxIntesety * Time.deltaTime);

            //Debug.Log(endTime - Time.time);

            yield return null;
            //Debug.Log("Max: " + maxIntesety);
        }

        
    }

    /* Generating a random Vector3 with the script specific min-max values
    *
    */
    Vector3 GetRandomV3(){

        Vector3 rv3 = new Vector3( Random.Range(minMaxX.x, minMaxX.y), Random.Range(minMaxY.x, minMaxY.y), Random.Range(minMaxZ.x, minMaxZ.y));

        return rv3;

    }



}
