// This is a git comment

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Krash : MonoBehaviour
{
    public GameObject effect;

    CarMovement _CarMovement;

    void Start(){
        _CarMovement = FindObjectOfType<CarMovement>();

       
        _CarMovement.onPlayerCrash.AddListener(SpawnCrashFX);    
    }

     void SpawnCrashFX() {
        Instantiate(effect, transform.position, Quaternion.identity);
    }
}
