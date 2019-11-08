using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhoneScript : MonoBehaviour
{

    public bool triggerDeath = false;

    [HideInInspector]
    public int notifications = 0;

    [HideInInspector]
    public bool lookingAtPhone = false;

    float startingToLook;
    float endedToLook;

    [HideInInspector]
    public UnityEvent triggerPhoneCrash = new UnityEvent(); 


    float timeToDistract = 9000f;
    bool initalTimeSet = false;
    bool inCall = false;

    [HideInInspector]
    public List<AudioClip> _mySounds = new List<AudioClip>();

    AudioClip currentClip;

    AudioSource mySource;

    GameController _GameController;

    // Start is called before the first frame update
    void Start()
    {

        _GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        mySource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(isVisible());
        PlayerCheckedPhone();
        VibrateRandomly(8f, 16f);
    }





    public bool isVisible(){
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

    public void PlayerCheckedPhone(){
        
        if(!lookingAtPhone && isVisible()){
            lookingAtPhone = true;
            startingToLook = Time.time;
            Debug.Log("Player started checking phone at " + startingToLook);
        } else if(lookingAtPhone && !isVisible()){
            endedToLook = Time.time;
            lookingAtPhone = false;

            Debug.Log("Player stopped checking phone at " +  endedToLook);
            
            if(endedToLook - startingToLook > 3.5f && _GameController.finishedIntro && triggerDeath){
                triggerPhoneCrash.Invoke();
            }
        }


    }


    void VibrateRandomly(float minWaitSecondsBetween, float maxWaitSecondsBetween){


        if(_GameController.finishedIntro && !initalTimeSet ){
            timeToDistract = Time.time + Random.Range(minWaitSecondsBetween, maxWaitSecondsBetween);
            initalTimeSet = true;
        }

        if(Time.time > timeToDistract && _GameController.finishedIntro){

            if(notifications == 2){
                currentClip =_mySounds[1];
            }else if(notifications == 4){
                currentClip =_mySounds[2];   
            } else if(notifications == 6){
                StartCoroutine(PlayYouAreLate());
            } else {
                currentClip = _mySounds[0];
            }
            

            if(!inCall){
                mySource.PlayOneShot(currentClip);
            }
            
            notifications ++;
            timeToDistract = Time.time + Random.Range(minWaitSecondsBetween, maxWaitSecondsBetween);
        }


        
    }

    IEnumerator PlayYouAreLate(){
        
        timeToDistract = Time.time + 8 + _mySounds[3].length + _mySounds[4].length;

        inCall = true;

        currentClip =_mySounds[3];
        mySource.PlayOneShot(currentClip);

        yield return new WaitForSeconds(currentClip.length + 1);

        currentClip = _mySounds[4];
        mySource.PlayOneShot(currentClip);

        yield return new WaitForSeconds(currentClip.length);

        inCall = false;

    }

    
}
