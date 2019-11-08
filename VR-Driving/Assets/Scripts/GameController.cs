/** Overseeing Element
*	!If you use VSCODE, get "Better Comments" Extention
*
*	* overall Game functionality and script communcation
*	* Refferenced by most (interactive) Objects
* 	
*	?Qustions on Git Issues, Kanban or on Tello
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class GameController : MonoBehaviour {
	/** Public refferences 
	*	* Ability to be controlled from editor or unity Remote
	*	* Hosts Values and objects required for script communication [Ideally solved with privates & get / set functions]
	*/

	//*  Current driving speed setting declaration
	public enum DrivingMode {Park, Drive, DriveFast, Race};

	[Header("Asset Pack used:")]
	[Tooltip("AssetPack Prefab GameObject" + "\n" + "Containing all Assets used" + "\n" +  "Possibly contains additional configuration info")]
	public AssetPack assetPack; //	*Required due to other script-dependencies 

	[Header("General Settings")]
	[Tooltip("Decideing if camera and controlls are controlled by the Vive or Keyboard, Mouse and Screen")]
	public bool VR = true; //true by default

	[Tooltip("Skips the intro sequence and replaces it with a 2 second timer")]
	public bool skipIntro = true; //true for Testing

	[Header("Player crashing")]
	[Tooltip("Deciding whether the player will crash on interaction")]
	public bool playerCanCrash = true;

	[Header("Balancing & Tweaking")] 

	[Tooltip("Current mode of the automated gear")]
	public DrivingMode currentSpeed;

	[Tooltip("Default movement Speed for player")]
	[Range(0, 100)] //	! 10 -> Placeholder for balancing
	public float maxSpeed = 100f; // Default value to prevent errors

	[Tooltip("Default speed to switch lanes turning the car left or right")]
	[Range(0,1)]
	public float steeringForce = 1f; // 1 Default value to prevent errors

	[Tooltip("Amount of desired RoadElemements reused over the course of the Game")]
	[Range(3, 15)]
	public int trackElementsAmount = 3; // Default value to prevent errors

	[Header("Obstacle Spawn & Interaction")]

	[Tooltip("Total amount of obstacles in the scene")]
	[Range(3, 100)] //	! 10 -> Placeholder for balancing
	public int obstacleCount = 5; // Default value to prevent errors

	[Tooltip("Delay between Obstacle spawns & re-spawns in seconds")]
	[Range(1, 10)] //	! 10 -> Placeholder for balancing
	public float spawnDelay = 1f; // Default value to prevent errors

	[Tooltip("Setting whether obtacles move towards player \n independent of player movement")]
	public bool moveIndependent = true; // Default value to prevent errors

	[Tooltip("Setting whether obtacles move towards or with the player")]
	public bool obstacleDirection = true; // True = move with the player

	[Tooltip("Obstacle AI trigger")]
	public bool obstacleAvoidanceAI = true; 

	[Tooltip("Speed of incomming obstacles \n Only works if moveIndependent")]
	[Range(1, 100)] //	! 100 -> Placeholder for balancing
	public float obstacleSpeed = 20f; // Default value to prevent errors

	[Tooltip("The players Audiosource for cutscenes")]
	public AudioSource _PlayerAudio;

	public List<AudioClip> Cutscenes = new List<AudioClip>();

	[Tooltip("Engine Sounds")]
	public AudioSource _EngineAudio;
	public List<AudioClip> EngineSounds = new List<AudioClip>();

	[Tooltip("Radio sound source")]
	public AudioSource _RadioAudio;
	public List<AudioClip> RadioSounds = new List<AudioClip>();

	public Transform _BlackBoxCentre;
	public Transform _PlayerTransform;
	Vector3 initalPlayerPos = new Vector3();



	/** Hidden in inspector, only utilized for scriptcommunication*/
	[HideInInspector]
	public float speed = 0f; // Communicating current speed to other scripts
	[HideInInspector]
	public bool moving = false; // Determining whether car is driving
	[HideInInspector]
	public bool finishedIntro = false;
	[HideInInspector]
	public float steeringDir = 0.01f; // routing movement direction
	[HideInInspector]
	public Vector2 laneBounds; // routing road bouderies (left and right max) to limit road throug script
	[HideInInspector]
	public Vector3 playerPos = new Vector3(0,0,-5); // -> Placeholder for balancing errorprevention
	[HideInInspector]
	public List<Vector3> laneCoords; // public list of all possible lane coordinates

	/** Script Communication for gearshift & directional change */
	[HideInInspector]
	public UnityEvent shiftGear = new UnityEvent();
	[HideInInspector]
	public UnityEvent switchDirection = new UnityEvent();
	[HideInInspector]
	public UnityEvent playerLookedAtPhone = new UnityEvent();
	[HideInInspector]
	public UnityEvent onPlayerDeath = new UnityEvent();

	/** Debug UI ref */
	GameObject _debugUI;


	/** Phone related */
	GameObject _Phone;
	PhoneScript _PhoneScript;

	CarMovement _CarController;
	Krash _Krash; //Accessing the script to enable effects and animations

	/// <summary> Awake method (unity)
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake(){

		/**	Require AssetPack
		*	* Since all interactivity and controlls rely on this, Check for assetPack
		*	* Check for errors and attempt all possible solutions
		*/
		if(assetPack != null){ // GameObject assetPack successfully linked in editor

			if(assetPack.TrackElement != null || assetPack.Obstacle != null){ //Asset-Pack Prefab default Prefabs successfully configured
				
				/** Asset-Pack & Globals successfully linked and configured
				*	!Comment out Debug before build;
				*/
				//Debug.Log("Asset-Pack successfully configured");
				laneBounds = assetPack.roadBounds;
				laneCoords = GetLaneCoords();
							
			} else {
				/** Error Cases & solution attempts:
				*	!Prefab AssetPack utilizing GameObject Lists only
				*	Todo: 	Communicate case to relevent scripts
				*	?		Use unity event 	
				*
				*	!Prefab	AssetPack is empty
				*	!Prefab AssetPack using flawd Elements
				*	Todo: Load Default assetPack Prefab
				*
				*	!AssetPack script missing public object
				*	Todo: 	Critical Error, manually populate alternative and communicate case to other script
				*	?		Dynamically attempt to prevent crashes whilst logging errors
				*/
			}

		} else {
			/** Error cases & solution attempts:
			*	!Asset Pack not linked in editor
			*	Todo: Attack asset pack with default components
			*/
		}

	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	/// </summary>
	void Start(){
		_debugUI = GameObject.FindGameObjectWithTag("DebugUI");
		_debugUI.SetActive(false);
		
		_Phone = GameObject.FindGameObjectWithTag("Phone");
		_PhoneScript = _Phone.GetComponent<PhoneScript>();

		_PhoneScript.triggerPhoneCrash.AddListener(playerLookedAtPhone.Invoke);
		_PhoneScript.triggerPhoneCrash.AddListener(crashPlayer);

		_CarController = GameObject.FindGameObjectWithTag("PlayerCar").GetComponent<CarMovement>();
		_CarController.onPlayerCrash.AddListener(onPlayerDeath.Invoke);
		_CarController.onPlayerCrash.AddListener(crashPlayer);

		currentSpeed = DrivingMode.Park;
		AdjustSpeed();
		
		initalPlayerPos = _PlayerTransform.position;
		_PlayerTransform.position = _BlackBoxCentre.position;

		StartCoroutine(PlayIntroSequence());
	}


	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update(){
		if(Input.GetKeyDown("p")){
			if(_debugUI.activeSelf){
				_debugUI.SetActive(false);
			} else {
				_debugUI.SetActive(true);
			}
		}

		if(Input.GetKeyDown(KeyCode.UpArrow) ||Input.GetKeyDown(KeyCode.W)){
			GearUp();
		} else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)){
			GearDown();
		} else if( Input.GetKeyDown(KeyCode.Space)){
			EmergencyBreak();
		}

		if( Input.GetKeyDown(KeyCode.R)){
			obstacleDirection = !obstacleDirection;
			//switchDirection();
		}
	}

	/// <summary> function to get coordinates of all lanes at all times
	/// TODO: Still needs commenting
	/// </summary>
	public List<Vector3> GetLaneCoords(){

		List<Vector3> laneCoords = new List<Vector3>();

		GameObject _trackElement;

		if(assetPack.TrackElement != null){
			_trackElement = assetPack.TrackElement[0];
		} else if(assetPack.RoadElements[0] != null){
			_trackElement = assetPack.RoadElements[0];
		} else {
			//return laneCoords;
			_trackElement = assetPack.TrackElement[0];
		}
		

		List<Transform> lanes = new List<Transform>();
		searchUtility(_trackElement.transform, "Lane", lanes);
		
		foreach (Transform lane in lanes){
			laneCoords.Add(lane.localPosition);
		}

		return laneCoords;
	}
	
	/// <summary> utility function to traverse objects for children with desired tag
	/// recursive function, optionally outputting results in list 
	/// TODO: Still needs commenting
	/// </summary>
	void searchUtility(Transform root, string tag, List<Transform> results = null){ //List is optional

		foreach (Transform child in root){
			
			if(child.gameObject.CompareTag(tag)){
			
				if(results != null){
					results.Add(child);
				}
			}
			searchUtility(child, tag, results);		
		}

	}

	void crashPlayer(){
		currentSpeed = DrivingMode.Park;
		AdjustSpeed();

		_EngineAudio.Stop();
		_RadioAudio.Stop();

		StartCoroutine(PlayDeathSequence());

		//trigger 

	}

//! >>>>>>>>>>>>>>>>>>>>>>>>>> GEARSHIFT CONTROLLER <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


	public void GearUp(){
		ShiftGear(1);
		_EngineAudio.PlayOneShot(EngineSounds[1]);
	}

	public void GearDown(){
		ShiftGear(0);
		_EngineAudio.PlayOneShot(EngineSounds[2]);
	}

	public void EmergencyBreak(){
		currentSpeed = DrivingMode.Park;
		ShiftGear(-1);
		_EngineAudio.PlayOneShot(EngineSounds[0]);
		AdjustSpeed();
	}


	/// <summary> function controlling the gearshift
	///	
	/// TODO: Still needs commenting
	/// </summary>
	public void ShiftGear(int upDown){

		switch (upDown)
		{
			
			case 1: //* Shift gear up */
				if(currentSpeed == DrivingMode.Park){
					currentSpeed = DrivingMode.Drive;
					AdjustSpeed();
					break;
				} else if(currentSpeed == DrivingMode.Drive){
					currentSpeed = DrivingMode.DriveFast;
					AdjustSpeed();
					break;
				} else if(currentSpeed == DrivingMode.DriveFast){
					currentSpeed = DrivingMode.Race;
					AdjustSpeed();
					break;
				} else if(currentSpeed == DrivingMode.Race){
					currentSpeed = DrivingMode.Race;
					break;
				} else { break; }

			case 0: //* Shift gear down */
				if(currentSpeed == DrivingMode.Race){
					currentSpeed = DrivingMode.DriveFast;
					AdjustSpeed();
					break;
				} else if(currentSpeed == DrivingMode.DriveFast){
					currentSpeed = DrivingMode.Drive;
					AdjustSpeed();
					break;
				} else if(currentSpeed == DrivingMode.Drive){
					currentSpeed = DrivingMode.Park;
					AdjustSpeed();
					break;
				} else if(currentSpeed == DrivingMode.Park){
					currentSpeed = DrivingMode.Park;
					break;
				} else { break; }

			case -1:
				currentSpeed = DrivingMode.Park;
				break;

			default:
				currentSpeed = DrivingMode.Park;
				break;
		}

		shiftGear.Invoke();

	}


	/// <summary> function controlling the speed according to current gear state
	///	
	/// TODO: Still needs commenting
	/// </summary>
	public void AdjustSpeed(){
		switch (currentSpeed)
		{
			
			case DrivingMode.Park:
				
				speed = 0;
				Debug.Log("Parking at: " + speed + " km/h");
				break;

			case DrivingMode.Drive:
				
				speed = maxSpeed * .75f;
				Debug.Log("Driving at: " + speed + " km/h");
				break;

			case DrivingMode.DriveFast:
				
				speed = maxSpeed;
				Debug.Log("Driving fast at: " + speed + " km/h");
				break;

			case DrivingMode.Race:
				
				speed = maxSpeed * 1.5f;
				Debug.Log("Racing at: " + speed + " km/h");
				break;

			default:
				
				speed = 0f;
				Debug.Log("Wrong gear input at: " + speed + " km/h");
				break;
		}
	}
//! >>>>>>>>>>>>>>>>>>>>>>>>>> END GEARSHIFT CONTROLLER <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


//! >>>>>>>>>>>>>>>>>>>>>>>>>> START SEQUENCER <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
IEnumerator PlayIntroSequence(){


	//! Enable Intro sequence and intro wait time before deploy

	if(!skipIntro){
		_PlayerAudio.PlayOneShot(Cutscenes[0]);	
		yield return new WaitForSeconds(Cutscenes[0].length * .9f);
	} else{
		yield return new WaitForSeconds(2);
	}

	_PlayerTransform.position = initalPlayerPos; // Move player into car

	currentSpeed = DrivingMode.Drive;
	GearUp();
	moving = true;
	AdjustSpeed();

	shiftGear.Invoke();

	finishedIntro = true;

	_EngineAudio.PlayOneShot(EngineSounds[2]);
	_EngineAudio.loop = true;

	_RadioAudio.PlayOneShot(RadioSounds[0]);

	yield return null;
}

IEnumerator PlayDeathSequence(){
	_PlayerAudio.PlayOneShot(Cutscenes[1]);


	finishedIntro = false;

	currentSpeed = DrivingMode.Park;
	AdjustSpeed();

	shiftGear.Invoke();

	//Cut screen

	yield return new WaitForSeconds(Cutscenes[1].length);

	yield return null;
}




}
