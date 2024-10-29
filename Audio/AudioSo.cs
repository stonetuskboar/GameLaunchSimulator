using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDatas", menuName = "ScriptableObjects/AudioDatas", order = 6)]
public class AudioSo : ScriptableObject
{
    public List<AudioData> AudioList;
    public Dictionary<string, AudioClip> AudioDic = new();
    public void Init()
    {
        AudioDic.Clear();
        for(int i = 0; i < AudioList.Count; i++)
        {
            AudioDic.Add(AudioList[i].AudioName, AudioList[i].AudioClip);
        }
    }

    public AudioClip GetClipByName(string name)
    {
        if(AudioDic.ContainsKey(name))
        {
            return AudioDic[name];
        }
        else
        {
            return null;
        }
    }
}


[Serializable]
public class AudioData
{
    public string AudioName;
    public AudioClip AudioClip;
}