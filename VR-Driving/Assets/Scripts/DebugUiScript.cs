using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugUiScript : MonoBehaviour {

	GameController _GameController;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	/// </summary>
	void Start(){
		_GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	public void SetSpeed(float _speed){
		_GameController.speed = _speed;
	}

	public void SetSteeringForce(float _steeringForce){
		_GameController.steeringForce = _steeringForce;
	}

	public void SetObstacleCount(float _obstacleCount){
		_GameController.obstacleCount = (int)_obstacleCount;
	}

	public void SetSpawnDelay(float _spawnDelay){
		_GameController.spawnDelay = _spawnDelay;
	}

	public void SetMoveIndependent(bool _moveIndependent){
		_GameController.moveIndependent = _moveIndependent;
	}

	public void SetObstacleSpeed(float _obstacleSpeed){
		_GameController.obstacleSpeed = _obstacleSpeed;
	}

	public void SetObstacleDirection(bool _obstacleDirection){
		_GameController.obstacleDirection = _obstacleDirection;
	}

	public void Restart(){
		
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}
