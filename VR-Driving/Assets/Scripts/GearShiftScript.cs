using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearShiftScript : MonoBehaviour
{

    public Transform start;
    public Transform end;

    float range = 1;
    
    GameController _GameController;

    // Start is called before the first frame update
    void Start()
    {
          _GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

          range = end.localPosition.x - start.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void ShiftGears(){
        
        if(transform.localPosition.x > end.transform.localPosition.x * .5f){
            //_GameController.GearUp();
        }



    }
}
