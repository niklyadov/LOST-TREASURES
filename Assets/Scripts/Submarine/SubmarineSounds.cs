using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineSounds : MonoBehaviour
{
    public AudioClip shot;
    public AudioClip move;
    public AudioClip chest;
    public AudioClip pickup;
    AudioSource audio;
    bool lastW = false;
    
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        shot = Resources.Load("Audio/shot_redacted") as AudioClip;
        move = Resources.Load("Audio/move_stable") as AudioClip;
        chest = Resources.Load("Audio/chest sound_edited") as AudioClip;
        pickup = Resources.Load("Audio/pickup_edited") as AudioClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)){
            if(!audio.isPlaying){
                audio.clip = move;
                audio.Play();
            }
            lastW = true;
        }
        else if(lastW){
            audio.Stop();
            lastW = false;
        }
    }

    void PlaySoundCol(float force, float maxForce){
        float volume = 1;

        if (force <= maxForce)
        {
            volume = force / maxForce;
        }

        audio.PlayOneShot(shot, volume);
    }

    void PlayChestSound(){
        audio.PlayOneShot(chest);
    }

    void PlayPickupSound(){
        audio.PlayOneShot(pickup);
    }
}
