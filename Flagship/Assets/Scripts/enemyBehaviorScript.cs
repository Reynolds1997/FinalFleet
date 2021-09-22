using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyBehaviorScript : MonoBehaviour
{

    public GameObject targetShip;
    public GameObject selectionRing; 

    public float shipsTargetingThisShip = 0;

    public NavMeshAgent shipNavMeshAgent;

    public GameObject[] shipWeapons;

    public GameObject torpedoPrefab;
    public GameObject[] torpedoLaunchers;

    public float torpedoRange = 20f;

    public float weaponFireRate = 0.5f;
    private float lastFired;

    public bool shipDefensiveFire = false;

    public GameObject podCommander;

    // Start is called before the first frame update
    void Start()
    {
        lastFired = Time.time;
        shipNavMeshAgent = this.GetComponent<NavMeshAgent>();
        setTarget(targetShip);
    }

    // Update is called once per frame
    void Update()
    {

        if (shipsTargetingThisShip > 0)
        {

            // this.GetComponent<Outline>().enabled = true;
            selectionRing.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            // this.GetComponent<Outline>().enabled = false;
            selectionRing.gameObject.GetComponent<MeshRenderer>().enabled = false;

        }

        if (targetShip != null)
        {
            if (targetShip.GetComponent<shipStatsManagerScript>().jumpedOut)
            {
                requestTarget();
            }

            pursueTarget(targetShip);
        }
        else if (this.gameObject.GetComponentInChildren<EnemyVisionRadiusScript>().currentAlertLevel >= podCommander.GetComponent<EnemyPodScript>().redAlertThreshold)
        {
            requestTarget();
        }
        else
        {
            setTarget(null);
            pursueTarget(null);
        }
       

        

                

    }


    //Pursues the target. Does not try and anticipate its destination.
    void pursueTarget(GameObject target)
    {
        if(targetShip != null)
        {
            shipNavMeshAgent.SetDestination(target.transform.position);

            //shipNavMeshAgent.destination = target.transform.position;
        }
        else
        {
            //shipNavMeshAgent.velocity = new Vector3(0,0,0);
        }
    }

    //Sets the target for the ship's weapons systems
    public void setTarget(GameObject target)
    {
        foreach (GameObject turret in shipWeapons)
        {
            turret.GetComponent<shipCannonScript>().targetShip = target;
        }

        foreach (GameObject launcher in torpedoLaunchers)
        {
            launcher.GetComponent<torpedoLauncherScript>().targetShip = target;
        }
    }

    public void standDown()
    {
        //print("STANDING DOWN");
        targetShip = null;
        GameObject target = null;
        foreach (GameObject turret in shipWeapons)
        {
            turret.GetComponent<shipCannonScript>().targetShip = target;
        }

        foreach (GameObject launcher in torpedoLaunchers)
        {
            launcher.GetComponent<torpedoLauncherScript>().targetShip = target;
        }
    }

    void requestTarget()
    {
        targetShip = null;
        setTarget(targetShip);

        print("This gameobject:" + this.gameObject);
        podCommander.GetComponent<EnemyPodScript>().findNewTargetForUnit(gameObject);
        setTarget(targetShip);
        //Ping the fleet commander and request a new target. 
        //The fleet commander can change the active target at any time.
    }


    


    
}
