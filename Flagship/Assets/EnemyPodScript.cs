using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyPodScript : MonoBehaviour
{

    public List<GameObject> detectedShipsList;
    public List<EnemyTarget> targetList = new List<EnemyTarget>();
    public List<GameObject> patrolPointList;
    public List<GameObject> podUnitList;

    public float alertLevel = 0;
   // public List<GameObject> alertedUnits;
    public float yellowAlertThreshold = 1;
    public float redAlertThreshold = 5;

    public Dictionary<GameObject, int> targetDictionary = new Dictionary<GameObject, int>();

    public int shipsPerTarget = 2;

    
   // public int detectedShipsCount = 0;
    public int enemyTargetCount = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject target in detectedShipsList)
        {
            addTargetToList(target);
        }
        foreach (GameObject unit in podUnitList)
        {
            //print(unit);
        }
    }

    // Update is called once per frame
    void Update()
    {




        //detectedShipsCount = detectedShipsList.Count();
        enemyTargetCount = targetList.Count();
        
        if (enemyTargetCount > 0)
        {
            alertLevel += Time.deltaTime;

        }
        else
        {
            alertLevel -= Time.deltaTime;
        }

        alertLevel = Mathf.Clamp(alertLevel, 0, redAlertThreshold);

        
        if (targetList.Count <= 0 && alertLevel > 0)
        {
            standDown();
        }

        if (alertLevel >= redAlertThreshold)
        {
            targetList = targetList.Where(item => item.targetObject != null).ToList();


            /*foreach (GameObject targetShip in detectedShipsList) //ship.GetComponentInChildren<EnemyVisionRadiusScript>().targetShipsList)
            {
                
                if(targetShip != null)
                {
                    targetList.Add(new EnemyTarget
                    {
                        targetObject = targetShip,
                        assignedUnits = new List<GameObject>()
                    });
                }
                
            }
            */
        }


        //Remove null items from the lists
        targetList = targetList.Where(item => item != null).ToList();
        






        /*
        float newAlertLevel = 0;
        foreach (GameObject unit in alertedUnits) //|| podManager.GetComponent<EnemyPodScript>().alertedUnits.Count > 0
        {
            if (unit.GetComponentInChildren<EnemyVisionRadiusScript>().currentAlertLevel > newAlertLevel)
            {
                newAlertLevel = unit.GetComponentInChildren<EnemyVisionRadiusScript>().currentAlertLevel;
            }
            //print(newAlertLevel);
        }
        alertLevel = newAlertLevel;
        */

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
            //print(target);
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

    public void addTargetToList(GameObject newTarget)
    {
        //Not set to instance of object?
        EnemyTarget existingTarget = targetList.Find(x => x.targetObject == newTarget);
        if (existingTarget !=null)
        {
            existingTarget.watchingUnitsCount++;
        }
        else
        {
            targetList.Add(new EnemyTarget
            {
                targetObject = newTarget,
                assignedUnits = new List<GameObject>(),
                watchingUnitsCount = 1
            });
        }
        
        //targetList = targetList.Where(item => item != null).ToList();
    }

    public void removeTargetFromList(GameObject removeTarget)
    {
        //TODO
        //FIX THIS LATER
        //targetList.Remove(removeTarget);

        foreach(EnemyTarget target in targetList.ToArray())
        {
            if(target.targetObject == removeTarget)
            {
                target.watchingUnitsCount--;
                print(target.targetObject + " BEING WATCHED BY " + target.watchingUnitsCount);
                if(target.watchingUnitsCount <= 0)
                {
                    targetList.Remove(target);
                }

            }
        }


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

    public void redAlert()
    {
        

        print(targetList);
    }

    public void standDown()
    {
        alertLevel = 0;
        //alertedUnits.Clear();
        targetList.Clear();

        foreach(GameObject ship in podUnitList.ToArray())
        {
            if(ship!= null)
            {
                //ship.GetComponentInChildren<EnemyVisionRadiusScript>().currentAlertLevel = 0;
                ship.GetComponent<enemyBehaviorScript>().standDown();
            }
            else
            {
                podUnitList.Remove(ship);
            }
        }
    }
}
