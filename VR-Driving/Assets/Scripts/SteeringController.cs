/** Controlling all Steering Wheel related functions
*	!If you use VSCODE, get "Better Comments" Extention
*
*	* Simulating wheel roation
*	* Communication steering roation / direction to gamecontroller
*	* adjusting steering wheel if not in use
*	
*	?Questions to Git Issues, Kanban or on Trello
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringController : MonoBehaviour {

	/** External objcet refferences
	*	* Gamecontroller to rout all scrip interaction and communication
	*	* ^ also obtaining tweakable variables
	*/

	
	GameController _GameController;

	Valve.VR.InteractionSystem.CircularDrive _MyCircularDrive;

	//Valve.VR.InteractionSystem.Interactable VrInteraction;

	/** Locally scoped variables 
	*	* Steering speed obtained through Gamecontroller, setting the strengh of steering transmission
	*	* Steering direction communicated to Gamecontroller in order to router further to CarControlls
	*/
	float steeringSpeed = 5f;

	[HideInInspector]
	public float steeringDir = 0;

	float initalSteeringRotation = -.1f;
	float steeringRotation;

	//bool keeping track of interaction
	public bool ineracting = false;

	[Range(0,1)]
	public float steeringThreshhold = 1f;
	Vector2 steeringBounds;

	// Use this for initialization
	void Start () {
		
		// Track down GameController
		_GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		
		if(_GameController != null){ // * Only if Gamecontroller found
			steeringSpeed = _GameController.steeringForce * .1f;
			Debug.Log("GC found");
		}

		_MyCircularDrive = GetComponent<Valve.VR.InteractionSystem.CircularDrive>();
		//if(_MyCircularDrive != null){Debug.Log(_MyCircularDrive.outAngle);}

		//VrInteraction.onAttachedToHand += OnInteract;
	}
	
	// Update is called once per frame
	void Update () {
		//if(!Input.GetMouseButton(0)){ // *If there is no left mouse interaction, adjusting the wheel

		steeringBounds = new Vector2(steeringRotation - steeringThreshhold, steeringRotation - steeringThreshhold);

		if(_GameController.VR && (_MyCircularDrive.outAngle < steeringBounds.x || _MyCircularDrive.outAngle > steeringBounds.y)){
			//Debug.Log(transform.localEulerAngles.z);
			
			_GameController.steeringDir = _MyCircularDrive.outAngle;
			//Debug.Log("Drive" + _MyCircularDrive.outAngle + " &" + transform.localEulerAngles.z);
		}

		steeringRotation = _MyCircularDrive.outAngle;
		
		//Debug.Log(_GameController.steeringDir);
			
		//}
	}

	/// <summary> Triggered through wheel interaction
	/// simulating rotation and triggering direction adjustments
	///</summary>
	public void Steer(float _steeringRotation){ 
		
		//Debug.Log(initalSteeringRotation +" new: " +  _steeringRotation);

		//Debug.Log("Inital: " + initalSteeringRotation + "\n" + "Current: " + _GameController);

		/* transform.Rotate(0, 0, Input.GetAxis("Mouse X") * -steeringSpeed); // Rotating the wheel holder and with it the Wheel mesh according to mouse direction while dragged

		AdjustDir(Input.GetAxis("Mouse X")); // adjusting direction in corrosponding to wheel rotation */

		//! ADJUST TO OUTANGLE IF WE CAN GET ONE MORE VIVE SESSIOLN
		/* if(_GameController.VR){
			if( _steeringRotation > initalSteeringRotation ){
				Debug.Log("steer right");
				_GameController.steeringDir = 1;
			} else if ( _steeringRotation < initalSteeringRotation){
				_GameController.steeringDir = -1;
				Debug.Log("steer left");
			} else {
				CorrectWheel();
				_GameController.steeringDir = 0;
			}

			initalSteeringRotation = _steeringRotation;
		} else {
			
			_GameController.steeringDir = _steeringRotation;
			
		} */
		
		
		

	}

	/// <summary> Correcting wheel if idle
	/// Auto rotating the steering wheel back to idle position if not in use
	/// allways chooses the direction with the shortest path back to 0
	///	sets direction to 0 if wheel is back in position
	///</summary>
	void CorrectWheel(){

		//! Exclude adjust dir and instead just link car rotation to wheel rotation if desired

		if(transform.rotation.eulerAngles.z != 0){ // if local rotation is not yet 0
			if((transform.rotation.eulerAngles.z > 358 || transform.rotation.eulerAngles.z < 2) /* */){ //if close to 0, reset direction

					transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
					_GameController.steeringDir = 0;

			}else{

				if(transform.rotation.eulerAngles.z > 180 ){ // Rotate left
					
					transform.Rotate(0, 0, (360 /(transform.rotation.eulerAngles.z))*2);
					//AdjustDir(1);
					////Debug.Log("left");
				} else { // Rotate right 
					
					transform.Rotate(0, 0, -(((transform.rotation.eulerAngles.z + 180)/180)*2));
					//AdjustDir(-1);
					////Debug.Log("right");
				}
			}
		}
	}

	/// <summary> Commuicating wheel movement to Gamecontroller
	/// Calculating steering direction corrosponding to wheel rotation
	/// Limiting it between -1 and 1 to set fixed direction
	/// Taking in mouse movement direction to detering and maintain direction even in case of overturning wheel
	///</summary>
	void AdjustDir(float dir){

		steeringDir = Mathf.Clamp((transform.rotation.z) * -1, -1, 1); //Limit steering direction

		if(dir > 0){ //Ensuring direction changes trigger
			steeringDir = Mathf.Abs(steeringDir);
		}

		_GameController.steeringDir = steeringDir;
	}

	void OnInteract(){
		ineracting = !ineracting;
	}
}



/* Steering Concept

Wheel rotation:

	circual drive interaction just rotates the wheel, so 




Wheel gets direct z rotation from vive circual drive
Communicates that to gamecontroller float transmission
Transmission then 
*/