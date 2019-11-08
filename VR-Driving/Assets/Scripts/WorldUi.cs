using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class WorldUi : MonoBehaviour
{

    public GameObject _WorldUi;

    Valve.VR.InteractionSystem.Hand _Hand;
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _Hand = GetComponent< Valve.VR.InteractionSystem.Hand>();
        _Hand.handType = SteamVR_Input_Sources.RightHand;
    }
    
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(_Hand.grabGripAction.GetChanged(_Hand.handType)){
            _WorldUi.SetActive(!_WorldUi.activeSelf);
        }
    }
    
}
