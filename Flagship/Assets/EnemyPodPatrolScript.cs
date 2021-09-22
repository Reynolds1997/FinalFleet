using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPodPatrolScript : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private EnemyPodScript podManagerScript;
    public List<GameObject> patrolPointList;
    private List<GameObject> podUnitList;
    public float destinationArrivalDistance = 1f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        podManagerScript = GetComponent<EnemyPodScript>();
        podUnitList = podManagerScript.podUnitList;

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).


        foreach (GameObject ship in podUnitList.ToArray())
        {
            ship.GetComponent<NavMeshAgent>().autoBraking = false;
        }

        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        //agent.destination = points[destPoint].position;

        foreach (GameObject ship in podUnitList.ToArray())
        {
            //print(ship);
            if(ship != null)
            {
                if (ship.GetComponent<NavMeshAgent>() != null)
                {
                    ship.GetComponent<NavMeshAgent>().SetDestination(points[destPoint].position);
                }
                else
                {
                    print("NAVMESHAGENT NOT FOUND");
                }
            }
        }

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.


        if(podManagerScript.alertLevel < podManagerScript.redAlertThreshold)
        {
            bool goToNext = true;
            foreach (GameObject ship in podUnitList.ToArray())
            {
                agent = ship.GetComponent<NavMeshAgent>();
                if (agent.pathPending || agent.remainingDistance > destinationArrivalDistance)
                {
                    //print(agent.remainingDistance);
                    goToNext = false;
                }
            }
            if (goToNext)
            {
                //print("MOVING TO NEXT POINT");
                GotoNextPoint();
            }
            //if (!agent.pathPending && agent.remainingDistance < 0.5f)
        }


    }
}

