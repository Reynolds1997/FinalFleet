using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget
{
    public GameObject targetObject { get; set; }
    public List<GameObject> assignedUnits { get; set; }
    public int watchingUnitsCount { get; set; }
}
