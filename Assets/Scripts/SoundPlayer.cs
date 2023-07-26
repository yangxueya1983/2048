using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundPlayer : MonoBehaviour
{
    public AudioClip moveAudioClip;
    public AudioClip matchAudioClip;
    public AudioClip successAudioClip;
    public AudioClip loseAudioClip;


    //public AudioSource testAudio;
    
    public void PlaySuccess()
    {
        AudioSource.PlayClipAtPoint(successAudioClip, Camera.main.transform.position);
    }
    
    public void PlayLose()
    {
        AudioSource.PlayClipAtPoint(loseAudioClip, Camera.main.transform.position);
    }
    
    public void PlayMove()
    {
        AudioSource.PlayClipAtPoint(moveAudioClip, Camera.main.transform.position);
    }
    
    public void PlayMerge()
    {
        AudioSource.PlayClipAtPoint(matchAudioClip, Camera.main.transform.position);
    }
}
