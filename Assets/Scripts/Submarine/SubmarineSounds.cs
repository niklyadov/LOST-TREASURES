using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineSounds : MonoBehaviour
{
    [SerializeField] private AudioClip shot;
    [SerializeField] private AudioClip move;
    [SerializeField] private AudioClip chest;
    [SerializeField] private AudioClip pickup;
    [SerializeField] AudioSource audio;
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

    public void PlaySoundCol(float force, float maxForce) {
        float volume = 1;

        if (force <= maxForce)
        {
            volume = force / maxForce;
        }

        audio.PlayOneShot(shot, volume);
    }

    public void PlayChestSound(){
        audio.PlayOneShot(chest);
    }

    public void PlayPickupSound(){
        audio.PlayOneShot(pickup);
    }
}
