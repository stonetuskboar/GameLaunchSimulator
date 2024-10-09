using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TiebaData", menuName = "ScriptableObjects/TiebaData", order = 2)]
public class TiebaData : ScriptableObject
{
    public List<PostData> postData;
}
