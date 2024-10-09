using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FileData", menuName = "ScriptableObjects/FileData", order = 3)]
public class FileSo : ScriptableObject
{
    public List<FileData> fileDataList;
}


[Serializable]
public class FileData
{
    public int fileId;
    public Sprite fileSprite;
    public string fileName;
    public string fileSize;
}