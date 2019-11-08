/** Object to controll Road
*	!If you use VSCODE, get "Better Comments" Extention
*
*	* Moving Road by speep to simulate movement
*	* Reuse Road Elements to achive endless feeling
*	* Supply Lane V3 coords
*	* => For PlayerMovement (Left <-> Right) and Obstacle Instantiation
*	
*	?Questions to Git Issues, Kanban or on Trello
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour {
	
	

	/** Editor Tweaks
	*	*Variables affecting gameplay, Tweakable from editor
	*	!Moved to Gamecontroller
	*/

	/**	Private references & Variables
	*	* References to required gameObjects
	*	* Script Scoped Variables
	*	* Locally set but otherwise obtained from gamecontroller
	*	* Misc
	*/
	GameController _GameController;
	List<GameObject> trackElement = new List<GameObject>();
	List<GameObject> trackElements = new List<GameObject>();

	int trackElementsAmount = 3; // Default value to prevent errors
	float moveSpeed = 1f; // Default value to prevent errors
	int lastWrappedIndex = -1; // Used to identify whether value was changed before, because List[index] & List.Count can never be -1
	float roadElementLength;

	bool playerHasCrashed = false;
	


	/// <summary> Start method (Unity)
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start() {
		// Track down GameController
		_GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

		if(_GameController != null){
			
			/** Console Log for Quick testing and debugging
			* 	!Comment out before build 
			*/
			////	Debug.Log("GameController found!");

			//	Get Track element Prefab from GameController
			trackElement = _GameController.assetPack.TrackElement;
			_GameController.shiftGear.AddListener(AdjustSpeed);

			
			if(trackElement != null){ // Successful checked for assetPack and Global Controller Variables

				/** Console Log for Quick testing and debugging
				* 	!Comment out before build 
				*/
				////	Debug.Log("Asset-Pack found!");

				/**	Get and assign required variables from GameController 
				*	?	Eventually Add more controll options or interactions for road
				*/
				trackElementsAmount = _GameController.trackElementsAmount;

				// *Getting Length of Road-Elements to properly adject them in succession
				roadElementLength = trackElement[0].transform.localScale.z;
				////	Debug.Log("All road elements have a lenght of: " + roadElementLength + " each");

				/**	Call InstantiateRoad method for inital spawn
				*	Todo: Trigger event to confirm successful enviroment spawn
				*/	
				InstantiateRoad(trackElementsAmount); 


			} else { //	Check for possible Errors and attempt solutions
				/** Error Cases
				*	!AssetPack in GameController empty
				*	!Gamecontroller Assetpack not assigned
				*	!AssetPack doesn not contain TrackElement
				*
				*	Todo:	=>	Check Assetpack for RoadELementList and if existent link cast into List<GameObject> Instead 
				*	Todo:	=> 	Manually search Default Prefab, assign and retry
				*/
				Debug.Log("Error: " + "Missing asset " + "in " + "GameController.assetPack");
			}


		} else { //	Check for possible Errors and attempt solutions
			/**	Error cases
			*	!GameController tag not assigned to GameController Object in Scene
			*	!GameController not used from Prefab
			*	!GameController Script Broken
			*	!GameControlelr not in Scene
			*
			*	Todo: => Manually Assign Variables and retry
			*/

			Debug.Log("Error: " + "GameController not found");
		}

		
	}
	
	/// <summary> Update Method (Unity)
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update(){
		
		//Debug.Log(_GameController.speed);


		if(_GameController.speed != 0 &&_GameController.finishedIntro){ //!Currently operating through keyboard input
			_GameController.moving = true; //Communicate to Gamecontroller

			if(playerHasCrashed){
				//moveSpeed *= .9f * Time.deltaTime;
			}

			Move(1);
		}else {
			_GameController.moving = false; //Communicate to Gamecontroller
		}
	}

	/// <summary> Controlling speed by input	
	/// Set global speed by player input
	/// Getting current speed from gamecontroller
	///</summary>
	void AdjustSpeed(){
		
		moveSpeed = _GameController.speed;

 	}
	
	/// <summary> InstantiateRoad method (used in start)
	/// * Utilized to instantiate desired amount of road elements 
	///	* Using TrackElement Gameobject defined in start
	/// </summar>
	/// <param name="roadElementCount" type="int"> 
	///	* Amount of desired RoadElemements to instantiate as int
	///	</param>
	void InstantiateRoad(int roadElementCount){
		
		/** Console Log for Quick testing and debugging
		* 	!Comment out before build 
		*/
		////	Debug.Log("Starting to instantiate " + roadElementCount + " Road-Elements");
		
		// *Instantiating desired number of Road-Elements
		for(int i = 0; i < roadElementCount; i++){
			
			// Adjusting spawn position by multplying length with number of previously instantiated elements
			Vector3 spawnPosition = new Vector3(0,0, roadElementLength * i);

			//Instantiating Prefab as Gameobject 
			GameObject roadElement = Instantiate(trackElement[Mathf.FloorToInt(Random.Range(0, trackElement.Count))], spawnPosition, Quaternion.identity, transform);
			roadElement.name = "Road_" + i;
			trackElements.Add(roadElement); // *Adding Element to list to for later Check sequences

			/** Console Log for Quick testing and debugging
			* 	!Comment out before build 
			*/
			////	Debug.Log("Added " + roadElement.name + " at " +  spawnPosition + "\n" + "Total RoadElements in Use: " + trackElements.Count);
		}

		_GameController.speed = 0;
	}

	/// <summary> Moving road and enviroment
	///	* Utilizied to create the illusion of movement by moving and wrapping roadElements
	/// * Iterating through during start populated trackElements list to first move and then positioncheck in order to apply wrapping
	/// * rE => roadElement
	///	</summary>
	void Move(float dir){
		
		////Debug.Log("move Gc Speed: " +  _GameController.speed + " & Local Speed : " + moveSpeed);

		if(trackElements.Count != 0){ //Only Execute if track elements successfully tracked in List

			for( int i = 0; i < trackElements.Count; i++){ //Iterating through track elements

				//Moving by translating postiong by global movement speed * time since last frame (if used in Update)
 				trackElements[i].transform.Translate(0,0, -dir * moveSpeed * Time.deltaTime);
				
				//If current iteration of List position is behind player, adject to most distant memeber of trackElements
				if(trackElements[i].transform.position.z < -(trackElements[i].transform.localScale.z)){
					
					////	Debug.Log("Wrapping: " + trackElements[i].name);

					Vector3 wrapperTargetPos; // Dynamically requested position to adjust to movement of total track

					if(lastWrappedIndex == -1){ // if this is the first time wrapping something and the most distant element is unknown
						
						/** Logs
						*	Debug.Log("First wrap ");
						*	Debug.Log(trackElements[trackElements.Count -1].transform.position); 
						*/
						
						// Add on to last instantiated Road Element
						wrapperTargetPos = trackElements[trackElements.Count -1].transform.position;

						////Debug.Log("Wrapped first time, setting Target to " + trackElements[trackElements.Count -1].name);

					} else {

						/** Logs 
						*	Debug.Log("Normal wrap ");
						*	Debug.Log(trackElements[lastWrappedIndex].transform.position); 
						*/

						// Add on to last moved Element
						wrapperTargetPos = trackElements[lastWrappedIndex].transform.position;

						////	Debug.Log("Wrapping " + trackElements[i].name + " behind " + trackElements[lastWrappedIndex].name);
					}

					wrapperTargetPos.z += roadElementLength; // Add Element length to z position for seamless connection
					trackElements[i].transform.position = wrapperTargetPos; // Move Element to new position (Lastitem(x, y, z + length))
					lastWrappedIndex = i; // Set last moved element index
				}
			}	
		}
	}




	/// <summary> Getting Lane center Coordinates for all lanes
	/// getting transfrom.postion.x from all roadPiece-Prefab children tagged lane || layer: lane
	///</summary>
	List<float> GetLanesX(){
		
		List<float> lanesX = new List<float>();

		return lanesX;

	}

	void PayerCrash(){
		playerHasCrashed = false;
	}
}
