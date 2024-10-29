using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource Bgm;
    public AudioSource SFX;
    public AudioSo audioSO;
    public static AudioManager Instance = null;

    public void Awake()
    {
        if(Instance == null)
        {
            audioSO.Init();
            Instance = this;
        }else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void StopBgm()
    {
        Bgm.Stop();
    }

    public void PlayBgm(string name)
    {
        Bgm.clip = audioSO.GetClipByName(name);
        Bgm.loop = true;
        Bgm.Play();
    }
    public void PlayHurtSfx()
    {
        int rand = Random.Range(1,6);
        if(rand == 1)
        {
            SFX.PlayOneShot(audioSO.GetClipByName("Damage1"));
        }else if(rand == 2) {
            SFX.PlayOneShot(audioSO.GetClipByName("Damage2"));
        }
        else if (rand == 3)
        {
            SFX.PlayOneShot(audioSO.GetClipByName("Damage3"));
        }else if (rand == 4)
        {
            SFX.PlayOneShot(audioSO.GetClipByName("Damage4"));
        }

    }
    public void PlaySfxByName(string name)
    {
        SFX.PlayOneShot(audioSO.GetClipByName(name));
    }


}
