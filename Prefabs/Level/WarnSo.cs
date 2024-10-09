using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WarnData", menuName = "ScriptableObjects/WarnData", order = 4)]
public class WarnSo : ScriptableObject
{

    public List<WarnData> WarnDatas = new();
}

[Serializable]
public class WarnData
{
    public Sprite WarnIcon;
    public string WarnText;
}