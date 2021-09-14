using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class shipCannonScript : MonoBehaviour
{
    public GameObject targetShip = null;

    public string enemyTag = "enemyShip";

    public float beamStartWidth = 0.2f;
    public float beamEndWidth = 0.1f;
    public Color beamStartColor = Color.cyan;
    public Color beamEndColor = Color.white;

    public float weaponRange = 10f;

    public bool defensiveFire = false;
    public bool weaponArmed = true;

    public float weaponFireRate = 0.5f;
    public float beamDuration = 0.25f;
    public bool isPulse = true;

    public int weaponDamage = 1;

    private float lastFired;

    LineRenderer lineRenderer;

    private bool beamFiring = false;

    public GameObject impactEffect;



    public GameObject[] enemies;


    // Start is called before the first frame update
    void Start()
    {
        lastFired = Time.time;
        lineRenderer = this.GetComponent<LineRenderer>(); // new GameObject("Line").AddComponent<LineRenderer>();
        InvokeRepeating("updateTarget", 0f, 0.25f);
        //impactEffect.Stop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //If enemy is in range and weapons are armed and not on cooldown, FIRE.
        //Two kinds of firing: targeted, or autofire (any targets in range)
        //Maybe later I can have cannons target independently. That could be HELLA cool.
        if (targetShip != null && this.transform.parent.gameObject.GetComponent<shipStatsManagerScript>().currentHull > 0)
        {
            //If target ship is in range, weapons are armed, and enemy ship is not currently exploding, fire a beam.
            if (weaponArmed && Vector3.Distance(this.transform.position, targetShip.transform.position) <= weaponRange && targetShip.GetComponent<shipStatsManagerScript>().currentHull >= 0)
            {
                beamFiring = true;
                lineRenderer.enabled = true;
            }
            else
            {
                beamFiring = false;
                lineRenderer.enabled = false;
            }

            //If enemy ship is within weapons range, fire beam.
            if (Vector3.Distance(this.transform.position, targetShip.transform.position) <= weaponRange)
            {
                if (beamFiring && targetShip != null)
                {
                    

                    drawBeam(this.transform.position, targetShip.transform.position, beamStartColor, beamEndColor);


                    fireCannon(targetShip);


                }
                if (isPulse && Time.time > beamDuration + lastFired)
                {
                    beamFiring = false;
                    lineRenderer.enabled = false;
                }
            }
            else
            {
                beamFiring = false;
                lineRenderer.enabled = false;
            }

        }
        else
        {
            beamFiring = false;
            lineRenderer.enabled = false;
        }

        //if(beamFiring == false)
        //  {
        //    impactEffect.Stop();
        //}



    }


    void fireCannon(GameObject target)
    {
        

        if (Time.time > weaponFireRate + lastFired)
        {
            lastFired = Time.time;
            target.GetComponent<shipStatsManagerScript>().takeDamage(weaponDamage);
        }

        


        //{
        //    beamFiring = false;
        //}

    }

    void drawBeam(Vector3 start, Vector3 end, Color startColor, Color endColor)
    {
        lineRenderer.enabled = true;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        lineRenderer.startWidth = beamStartWidth;
        lineRenderer.endWidth = beamEndWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPosition(0, start); //x,y and z position of the starting point of the line
        lineRenderer.SetPosition(1, end);
        
    }

    //Find the nearest enemy.
    void updateTarget()
    {
        enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance && enemy.gameObject.GetComponent<shipStatsManagerScript>().currentHull > 0)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if(nearestEnemy != null && shortestDistance <= weaponRange && defensiveFire == true)
        {
            targetShip = nearestEnemy;
        }

    }
    
}
