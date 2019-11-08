/** Controlling Turns / Laneswitches & speed
*	!If you use VSCODE, get "Better Comments" Extention
*
*	* Making turns by fixed degree interpolation
*	* Can pass speed to GameController
*	* Performing turns corrosponding to steering wheel rotation
* 	
*	?Qustions on Git Issues, Kanban or on Tello
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarMovement : MonoBehaviour {

	/** Gameplay Refferences
	*	* Connection to overseeing gamecontroller
	*	* Refferencing own rigidbody for manipulation
	*/
	GameController _GameController;
	CameraController _CameraController;
	Rigidbody myRb;
	[HideInInspector]
	public Animator myAnim;



	/* Outgoing Variables
	*	refferenced from camera scripts
	*/
	public bool crashing;

	/** function required variables
	*	* refferenced from gamecontroller
	*/
	float steeringForce = 1f;
	float speed = 10f;
	
	float oldsteeringDir = .0234f;
	/* ScriptCommunication
	*
	*/
	[HideInInspector]
	public UnityEvent onPlayerCrash = new UnityEvent();

	// Use this for initialization
	void Start () {
		_GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		steeringForce = _GameController.steeringForce;
		speed = _GameController.speed;

		_GameController.playerPos = transform.position;
		_GameController.shiftGear.AddListener(AdjustSpeed);

		_CameraController = GetComponent<CameraController>();

		myRb = GetComponent<Rigidbody>();
		//myRb.constraints = RigidbodyConstraints.FreezePositionZ;

		myAnim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {

		

		Turn(_GameController.steeringDir);
		//Debug.Log(_GameController.steeringDir);

		//!Only for crsah testing
		if(Input.GetKeyDown("q")){

			Debug.Log("crash");
			Die();
			
			
			
		}


	}

	/// <summary> Controlling speed by input	
	/// Set global speed by player input
	/// Getting current speed from gamecontroller
	///</summary>
	void AdjustSpeed(){
		
		speed = _GameController.speed;

 	}

	/// <summary> Turning the car	
	/// Move towards target dir
	/// Rotate Car rot.z by percentage of turn completion 0 -> 90 -> 0
	///</summary>
	void Turn( float dir){

		//Debug.Log(dir);

		if(_GameController.moving){ //* Allow steering only while moving*/

			
			//Debug.Log(transform.position + " & rotation: " + transform.rotation.eulerAngles + " & dir: " + dir);

			//Debug.Log("moving");

			//transform.Rotate(0,(dir*steeringForce)*Time.deltaTime, 0, Space.Self); //* Adjust car rotation by steering force in input direction*/

			//Debug.Log("isMoving");

			


			//ransform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + _GameController.steeringDir, 0 );
			

			/* 	Move car on x axis by percentage of desired maxium car rotation 
			*/

			

			//transform.position = new Vector3(transform.position.x + (dir * Time.deltaTime),0,0);

			//transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + (dir * (steeringForce * Time.deltaTime)), 0);
			//Debug.Log(transform.position + " & rotation: " + transform.rotation.eulerAngles);

			//transform.Rotate(0, dir*steeringForce*Time.deltaTime, 0); //* Adjust car rotation by steering force in input direction*/

			//Debug.Log("Dir: " + dir + " & TY: " + transform.localEulerAngles.y);


			/* Debug.Log(transform.rotation.eulerAngles.y + " old Y");
			Debug.Log(dir +" old DIR"); */


			/* if( dir > transform.rotation.eulerAngles.y){

				//Debug.Log(dir-transform.rotation.eulerAngles.y + "should be right");

				//Debug.Log("Steering right");

				transform.Rotate(0, (steeringForce * ( dir -  transform.rotation.eulerAngles.y))  * Time.deltaTime , 0);
				//transform.rotation = Quaternion.Euler(0,dir,0);
				transform.Translate((dir - transform.rotation.eulerAngles.y)* Time.deltaTime, 0,0);
			} else if ( dir < transform.rotation.eulerAngles.y) {

				//Debug.Log(dir-transform.rotation.eulerAngles.y + "should be left");

				//Debug.Log("Steering left");
				transform.Rotate(0, (steeringForce * (transform.rotation.eulerAngles.y - dir))  * Time.deltaTime, 0);
				transform.Translate((transform.transform.rotation.eulerAngles.y - dir )* Time.deltaTime, 0,0);
			} else {
				//Debug.Log("isequal? : " + (dir == transform.rotation.eulerAngles.y) + " & is: " + dir + " & " + transform.rotation.eulerAngles.y);
			} */

			if(dir != oldsteeringDir){
				//Debug.Log("steering" + dir);
				//transform.Rotate(0, (steeringForce * dir) * Time.deltaTime, 0);
				transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + ((steeringForce * dir) * Time.deltaTime),0);
				transform.Translate( (steeringForce * dir) * Time.deltaTime, 0, 0);
			}

			

			oldsteeringDir = dir;

			//transform.rotation = Quaternion.Euler(0,dir,0);
			////Debug.Log(transform.rotation.y);

			/* Debug.Log(transform.rotation.eulerAngles.y + " new Y");
			Debug.Log(dir +"  new DIR"); */

			/* transform.position = new Vector3 (Mathf.Clamp( //Limiting x position to road bounderies
												transform.position.x + (transform.rotation.y/45) * steeringForce * speed * Time.deltaTime,
												_GameController.laneBounds.x, _GameController.laneBounds.y
											), transform.position.x + (transform.rotation.y - 180) * steeringForce * speed * Time.deltaTime, 
											transform.position.y, transform.position.z
											); */

			/* transform.position = new Vector3 ( Mathf.Clamp( //Limiting x position to road bounderies
												transform.position.x + (transform.rotation.y/45) * steeringForce * speed * Time.deltaTime,
												_GameController.laneBounds.x, _GameController.laneBounds.y
			), transform.position.x + dir * steeringForce * speed * Time.deltaTime, 
											transform.position.y, transform.position.z
											); */

			//if(dir == 0){
			//	transform.rotation = Quaternion.identity;
			//}

		}

	}

/* 	/// <summary> Checking collisons with other cars or walls
	/// OnCollisionEnter is called when this collider/rigidbody has begun
	/// touching another rigidbody/collider.
	/// </summary>
	/// <param name="other">The Collision data associated with this collision.</param>
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.CompareTag("Obstacle")){
			//Crash with car scenario
		} else if (other.gameObject.CompareTag("Wall")){
			//Crash into wall scenario
		}
	} */


	/// <summary>
	/// OnCollisionEnter is called when this collider/rigidbody has begun
	/// touching another rigidbody/collider.
	/// </summary>
	/// <param name="other">The Collision data associated with this collision.</param>
	void OnCollisionEnter(Collision other)
	{																						//! Excluding walls if desired for testing
		if(other.transform.gameObject.layer != 10 && other.transform.gameObject.layer != 11 /* && other.transform.gameObject.layer != 18 */ &&other.transform.gameObject.layer != 26 && _GameController.finishedIntro &&!crashing){
			Debug.Log("die due to crash with: " + other.gameObject.name + " on layer: " + other.gameObject.layer);
			crashing = true;
			if(_GameController.playerCanCrash){
				Die();
			}
		}
		
	}


	public void Die(){

		Instantiate(_GameController.assetPack.blackSmoke, transform.position, Quaternion.identity);
		
		onPlayerCrash.Invoke();
		Debug.Log("Die");
		StartCoroutine(Deathanimation());
		//_CameraController.SpinCrash();
		
	}

	IEnumerator Deathanimation(){
		myAnim.SetTrigger(Animator.StringToHash("shake"));
		
		yield return new WaitForSeconds(2);
		
		_CameraController.Fade(.25f);

		yield return new WaitForSeconds(5);

		// Put player back in blackbox
		//? Reposition differently if desired
		_GameController._PlayerTransform.parent = _GameController.transform.parent;
		_GameController._PlayerTransform.position = _GameController._BlackBoxCentre.position;

		myAnim.SetTrigger(Animator.StringToHash("shake"));

		
	}

	

	
}


