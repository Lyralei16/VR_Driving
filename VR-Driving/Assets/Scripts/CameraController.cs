/** Simulating VR view & Handlin camera interactions
*	!If you use VSCODE, get "Better Comments" Extention
*
*	* Utilizing mouse input to controll view
*	* Only serves Demo 
* 	
*	?Qustions on Git Issues, Kanban or on Tello
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


[RequireComponent(typeof(CamRotation))]
public class CameraController : MonoBehaviour {
	
	//Refferences to Vr & NonVr Cameras
	public Camera VrCam;
	public Camera NonVrCam;

	PostProcessVolume _FadeoutPost;

	Transform _roadView;
	//Transform targetTransform;

	GameController _GameController;
	CarMovement _carController;
	CamRotation _camInteractions;

	// Responsible for VR / NonVR
	bool isVr = true;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	/// </summary>
	void Start(){
		_GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		isVr = _GameController.VR;
		_carController = GetComponent<CarMovement>();
		_camInteractions = GetComponent<CamRotation>();
		//_roadView = GameObject.FindGameObjectWithTag("RoadView").transform;


		_FadeoutPost = FindObjectOfType<PostProcessVolume>();
		

		if(!isVr){
			VrCam.enabled = false;
			NonVrCam.enabled = true;
		} else{
			VrCam.enabled = true;
			NonVrCam.enabled = false;
		}

	}

	// Update is called once per frame
	void Update () {

		if(!_carController.crashing && !isVr){
			NonVrPlayer();
		}
			
	}


	/*	Refferencing the roation crash
	?	Eventually add public parameters
	*/
	public void SpinCrash(){

		//Debug.Log("Called Spin crash");

		_camInteractions.CrashShake(30, 3);
	}

	public void Fade(float fadeSpeed = 1){
		StartCoroutine(Fadeout(fadeSpeed));
	}

	IEnumerator Fadeout(float fadeSpeed){

		while(_FadeoutPost.weight < 1){
			_FadeoutPost.weight += (fadeSpeed * Time.deltaTime);
			yield return null;
		}

	}

	/*
	*	Utilizing mouse input to controll view
	*	Meant for demo and testing
	*/
	void NonVrPlayer(){
		if(Input.GetMouseButton(1) && !Input.GetMouseButton(0)){ //Only triggers if left mouse not in use and right mouse is held down
			NonVrCam.transform.Rotate(Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0, Space.Self); //Moving Camera angle according to mouse movement
		} else {
			NonVrCam.transform.LookAt(_roadView);
		}
	}
}
