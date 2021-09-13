using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPodScript : MonoBehaviour
{

    public List<GameObject> targetObjectsList;
    public List<EnemyTarget> targetList = new List<EnemyTarget>();
    public List<GameObject> patrolPointList;
    public List<GameObject> podUnitList;

    public Dictionary<GameObject, int> targetDictionary = new Dictionary<GameObject, int>();

    public int shipsPerTarget = 2;

    public int alertLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject target in targetObjectsList)
        {
            addTargetToList(target);
        }
        foreach (GameObject unit in podUnitList)
        {
            print(unit);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //Assign shipsPerTarget ships to each target in the list, until you're out of ships. 
    void assignTargets()
    {
        //For each ship in the list, assign them a target and increment the targetingCounter for that target.

        foreach(EnemyTarget target in targetList)
        {
            for(int i = 0; i < shipsPerTarget; i++)
            {
                //Get the first X ships on the list that don't have a target assigned to them
            }
        }

        foreach(GameObject unit in podUnitList)
        {
            findNewTargetForUnit(unit);
        }
    }

    public void findNewTargetForUnit(GameObject unit)
    {
        //Find a target that doesn't currently have 
        foreach(EnemyTarget target in targetList)
        {
            print(target);
        }
        foreach(EnemyTarget target in targetList) //ISSUE HERE
        {
            //Next I need to set up a counter system so that the pod knows how many of its ships have been assigned to each target.
            if(target.assignedUnits.Count < shipsPerTarget)
            {
                unit.GetComponent<enemyBehaviorScript>().targetShip = target.targetObject;
                target.assignedUnits.Add(unit);
                break;
            }

        }

    }

    void addTargetToList(GameObject newTarget)
    {
        //Not set to instance of object?
        targetList.Add(new EnemyTarget
        {
            targetObject = newTarget,
            assignedUnits = new List<GameObject>()
        });
    }

    void removeTargetFromList(GameObject removeTarget)
    {
        //TODO
        //FIX THIS LATER
        //targetList.Remove(removeTarget);
    }

    void addUnitToList(GameObject newUnit)
    {
        podUnitList.Add(newUnit);
    }

    void removeUnitFromList(GameObject removeUnit)
    {
        podUnitList.Remove(removeUnit);
    }

    

    void patrol()
    {

    }

    void returnToPatrol()
    {

    }
}
