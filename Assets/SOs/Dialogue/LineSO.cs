using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LineSO", menuName = "SOs/LineSO", order = 1)]
public class LineSO : ScriptableObject
{
    public string line;
    public float duration;
}
