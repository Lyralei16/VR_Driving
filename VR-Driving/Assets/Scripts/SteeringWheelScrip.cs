/** Communication for the actual Steering Wheel Object
*	!If you use VSCODE, get "Better Comments" Extention
*
*	* Monitoring interaction with the attached mesh collider
*	* triggering Wheel Holder Objec to achieve proper rotation of the wheel
*	
*	?Questions to Git Issues, Kanban or on Trello
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringController))]
public class SteeringWheelScrip : MonoBehaviour {

	/** Refferences to corrosponding Gameobjects 
	*	* SteeringWheelHolder which ensures consistant manipulation
	*	* SteeringWheelControlelr attached to holder, handling all actions triggerd through attached meshcollider
	*/
	GameObject SteeringWheelHolder;

	GameController _GameCon;
	SteeringController _StCon;

	// Use this for initialization
	void Start () {

		_GameCon = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

		SteeringWheelHolder = transform.parent.gameObject;
		_StCon = GetComponent<SteeringController>();

		//Debug.Log("stcon: " + _StCon.transform.name);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// OnMouseDrag is called when the user has clicked on a GUIElement or Collider
	/// and is still holding down the mouse.
	/// </summary>
	void OnMouseDrag()
	{	
		//Debug.Log("dragging");
		if(!_GameCon.VR){
			
			transform.Rotate(0,0,Input.GetAxisRaw("Mouse X"));

			_StCon.Steer(Input.GetAxisRaw("Mouse X")); // *Triggering controller steering through dragging mouse

			
		}
		
	}

	/// <summary>
	/// Called every frame while the mouse is over the GUIElement or Collider.
	/// </summary>
	void OnMouseOver()
	{
		//Debug.Log("hover");
	}
}

