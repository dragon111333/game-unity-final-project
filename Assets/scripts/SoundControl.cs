using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{

    public AudioClip monsterDie;
    public AudioClip friendDie;

    private AudioSource soundPlayer;
    

    void Start()
    {
        soundPlayer = this.gameObject.AddComponent<AudioSource>();
        soundPlayer.spatialBlend = 1f;
        soundPlayer.volume = 1000f;

    }

    public void PlayMonsterDie()
    {
        soundPlayer.clip = monsterDie;
        soundPlayer.loop = false;
        soundPlayer.Play();
    }

    public void PlayFriendDie()
    {
        soundPlayer.clip = friendDie;
        soundPlayer.loop = false;
        soundPlayer.Play();
    }
}
