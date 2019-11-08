/** Spawning and controllering pool of incomming Obstacle prefabs 
*	!If you use VSCODE, get "Better Comments" Extention
*	!2nd Draft for spwaner
*
*	* Spawning desired amout of obstacles
*	* Resetting them according to lane coords
*	* Controlling obstacle parameters
*	
*	?Questions to Git Issues, Kanban or on Trello
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawnerV2l : MonoBehaviour {

	/** External object refferences 
	*	* Gamecontroller routing all script commincation & supplying assets
	*	* Obstacle Prefab obtained through Gamecontroller
	*/
	GameController _GameController;
	List<GameObject> _ObstaclePrefabs; 

	/**	Private references & Variables
	*	Set of variables to controll obstacle spawn, respawn and movement behaviour
	*	Mainly taken from GameController
	*/

	public float spawnHeight = 1f;

	//* Spawner required variables */
	int obstacleCount = 5; //* Amount of desired obstacles in game
	int currentObstacleCount = 0; //* tracker for obstacles spawned
	float spawnTimer = 0f; //* timer for spawns
	float spawnDelay = 2f; //* delay between spawns

	//* Lists maintaining active & inactive obstacles */
	List<GameObject> obstacleHolder = new List<GameObject>();
	List<GameObject> activeObstacleHolder = new List<GameObject>();

	//* Obstacle handling required variables */
	float endOfLane = 70f; //* Maximum tracklengh  [70 -> Placeholder to prevent fatal errors]
	float playerLengthBuffer = 5f; //* Bufferzone representing player car length

	bool moveIndependent = true; //* Setting whether obtacles move towards player
	float moveDir = 5f; //* Speed of incomming obstacles

	bool obstacleDirection = true;

	// Use this for initialization
	void Start () {
		_GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		
		if(_GameController != null){ // * Only if Gamecontroller found

			_ObstaclePrefabs = _GameController.assetPack.Obstacles; //Fetching prefab from GameController

			//* Fetching all required variables from Gamecontroller */
			obstacleCount = _GameController.obstacleCount;
			spawnDelay = _GameController.spawnDelay;
			moveIndependent = _GameController.moveIndependent;
			obstacleDirection = _GameController.obstacleDirection;

			//_GameController.switchDirection += AdjustCondition;
			
			AdjustCondition();
		}

		

	}
	
	// Update is called once per frame
	void Update () {
		if(_GameController.finishedIntro){
			HandleObstacles();
		}
		
	}

	/// <summary> Method handeling Obstacle instantiation and pooling
	///	Utililzing prefab and amount passed form Gamecontroller
	/// </summary>
	void HandleObstacles(){
		// *Instantiating desired number of Obstacles in desired intervalls

		if(Time.time > spawnTimer){ //Checking for time trigger

			Vector3 randomLaneCoords = _GameController.laneCoords[Mathf.FloorToInt(Random.Range(0, 3))]; //Picking random spawn Lane

			Vector3 spawnPosition = new Vector3();

			if(obstacleDirection){
				spawnPosition = new Vector3(randomLaneCoords.x, spawnHeight, endOfLane); // Calculating actual spawn position
			} else {
				// _GameController.assetPack.TrackElement[0].transform.localScale.z * _GameController.trackElementsAmount
				
				spawnPosition = new Vector3(randomLaneCoords.x, spawnHeight, endOfLane); // Calculating actual spawn position
				//Debug.Log("Spawn is: " + spawnPosition);
			}

			

			if(currentObstacleCount < obstacleCount){ // If full amount of obstacles has not yet been spawned
				
				//* instantiate new Obstacles */

				currentObstacleCount ++; //increasing obstacle count to keep track of spawned obstacles
				spawnTimer = Time.time + spawnDelay; // Refreshing spawn timer
				StartCoroutine(SpawnObstacle(spawnPosition)); // starting obstacle spawn coroutine

			} else { // If desired amount of obstacles exists in game

				spawnTimer = Time.time + spawnDelay; // Refreshing spawn timer

				if(obstacleHolder.Count != 0){ // If there are inactive obstacles
					
					StartCoroutine(ReuseObstacle(spawnPosition)); // Starting re-use Coroutine

				} 
			}
			
		} 


		
		
		//* Triggering obstacle movement and wrapping */
		MoveObstacles();
	}

	/// <summary> Spawning new obstacle at desired location
	///	Instantiating new Obstacle from prefab taken from GameController Assetpack
	/// Naming Obstacle and placing it in holder list
	/// </summary>
	IEnumerator SpawnObstacle(Vector3 spawnPosition){
		
		// * Instantiating obstacle and parenting it to the transform this script is attached to
		GameObject obstacle = Instantiate(_ObstaclePrefabs[Mathf.FloorToInt(Random.Range(0, _ObstaclePrefabs.Count))], spawnPosition, Quaternion.identity, transform);
		obstacle.transform.Rotate(new Vector3(0, 180, 0)); 
		obstacle.name = "Obstacle_" + currentObstacleCount; // Naming obstacle according to count
		activeObstacleHolder.Add(obstacle); // *Adding Obstacle to list to for later Check sequences
		

		//Debug.Log("Spawn: " + obstacle.name + " at " + spawnPosition);

		yield break;
	}

	/// <summary> "Re-spawning" inactive obstacle at desired postion
	///	Moving first elelemt of inactive obstacles to desired position
	/// Setting it back to active and adding it to corrosponding active obstacle list
	/// </summary>
	IEnumerator ReuseObstacle(Vector3 spawnPosition){
		
		GameObject freeObstacle = obstacleHolder[0]; // Getting refference to first element in inactive Osbtacles

		freeObstacle.SetActive(true); // Reactivating unused obstacle
		freeObstacle.transform.position = spawnPosition; // Moving it[unused obstacle] to desired position

		obstacleHolder.RemoveAt(0); // Removing it from inactive list
		activeObstacleHolder.Add(freeObstacle); // Adding it to active list
		
		yield break;;
	}

	/// <summary> Moving and Reusing obstacles
	///	Moving obsacles independent of road if desired
	/// Wrapping obstacles if out of sight
	/// </summary>
	void MoveObstacles(){
		for (int i = 0; i < activeObstacleHolder.Count; i++){

			if(moveIndependent){	
				/** Obstacles moving towoards player alternative 
				*	* activeObstacleHolder[i].transform.Translate(0, 0, speed * Time.deltaTime);
				*/

				float currentObstacleSpeed = currentObstacleSpeed = -activeObstacleHolder[i].GetComponent<ObstacleAI>().speed;

				//Debug.Log("moveing: " + moveDir);	

				activeObstacleHolder[i].transform.Translate(0, 0, (moveDir * currentObstacleSpeed * Time.deltaTime)); // obstacles moving with player
			}

			//Debug.Log(wrapCondition(i));
			
			//* If Obstacles reach end of track - reuse
			if(wrapCondition(i)){ 

				activeObstacleHolder[i].transform.position = new Vector3(0, -300, -300); // moving now inactive obstacle to unused and invisable space
				activeObstacleHolder[i].SetActive(false); // Setting unused Obstacle inactive

				obstacleHolder.Add(activeObstacleHolder[i]); // Keeping track by moving to correct list and removing from active list
				activeObstacleHolder.RemoveAt(i);

				/////Debug.Log(activeObstacleHolder.Count + " // " + obstacleHolder.Count);

			}
		}
	}

	bool wrapCondition(int i){

		if(obstacleDirection){
			return activeObstacleHolder[i].transform.position.z > _GameController.assetPack.TrackElement[0].transform.localScale.z * _GameController.trackElementsAmount;
		} else {	//* If obstacle "passed" player
			//Debug.Log(activeObstacleHolder[i].transform.position.z + " behind " + _GameController.playerPos.z);
			return activeObstacleHolder[i].transform.position.z <  _GameController.playerPos.z - playerLengthBuffer;
		}
	}


	void AdjustCondition(){
			
		if(!obstacleDirection){ /** Cars towards player alternative */
			moveDir = -1; 
		} else {
			
			moveDir = 1;
			
		}
		// * setting spawnpoint for obstacles at 1/2 (or 50%) of the maximum track length
		if(!obstacleDirection){ /**  Cars towards player alternative */
			endOfLane = (_GameController.assetPack.TrackElement[0].transform.localScale.z * _GameController.trackElementsAmount) * .5f;
		} else { endOfLane = -(_GameController.assetPack.TrackElement[0].transform.localScale.z) * .5f; }
	}
}
