using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    // Start is called before the first frame update

    AudioSource audio;


    void Awake()
    {

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;


            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    private void Start()
    {
        audio = GetComponent<AudioSource>();

        StartCoroutine(playEngine());
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found! Check audio name.");
            return;
        }
        s.source.Play();
    }

    public AudioClip getSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.clip;
    }



    IEnumerator playEngine()
    {
        /* audio.clip = getSound("Vibration");
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = getSound("Vibration");
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = getSound("I cant be late");
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = getSound("Vibration");
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = getSound("Meeting1");
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length); */
        
        audio.clip = getSound("Intro");
        audio.Play();


        yield return null;
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Play("Acceleration");
        }

    }

    void OnCollisionEnter(Collision other)
    {
        {
            if (other.transform.gameObject.layer != 26)
            {
                Play("Crash");
            }
        }


    }
}
