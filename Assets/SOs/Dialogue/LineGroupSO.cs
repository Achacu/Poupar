using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LineGroupSO", menuName = "SOs/LineGroupSO", order = 1)]
public class LineGroupSO : ScriptableObject
{
    public LineSO[] lines;
}
