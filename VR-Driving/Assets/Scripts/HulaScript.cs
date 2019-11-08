using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HulaScript : MonoBehaviour
{   

    public float force = 2f;
    public Vector2 maxScale = new Vector2(-75f, -105f);

    Animator myAnim;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {   

        //Debug.Log(transform.localScale.z);

        /* if(transform.localScale.z < maxScale.y){
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + (transform.localScale.z * (.03f * Time.deltaTime) ));
        } else if(transform.localScale.z > maxScale.x){
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - (transform.localScale.z * (.03f * Time.deltaTime) ));
        } */

        
    }
}
