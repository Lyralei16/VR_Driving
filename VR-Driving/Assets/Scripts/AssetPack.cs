 /** Object containing all public game Object refferences
*	!If you use VSCODE, get "Better Comments" Extention
*
*	*This is utilized so all refferences can be manged via script instead of the editor
*	*Creat Lisits for Objcet collections like [Exampel: Trees, Cars, etc..]
*	*Only derives from MonoBehaviour to be attachable to GameObjects => saveable as Prefab
*	
*	Todo: Prevent Destroy on load for Menu configuration etc	
*
*	?Questions to Git Issues, Kanban or on Trello
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetPack : MonoBehaviour{

	[Header("Nested Prefabs")]
	public List<GameObject> TrackElement;

	public GameObject Obstacle;

	[Header("Gameplay Presets")]
	public Vector2 roadBounds;

	[Header("Prefab Lists")]
	public List<GameObject> RoadElements;
	public List<GameObject> EnviromentSpawns;
	public List<GameObject> Obstacles;

	[Header("VFX")]
	public GameObject blackSmoke;


}
