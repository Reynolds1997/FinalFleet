using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torpedoLauncherScript : MonoBehaviour
{
    public GameObject targetShip;
    public GameObject torpedoPrefab;
    public float launcherFireRate = 5f;
    public float lastFired;
    public float torpedoRange = 20f;

    public bool defensiveFire = false;

    private GameObject[] enemies;

    public GameObject parentShip;

    public string enemyTag = "enemyShip";
    // Start is called before the first frame update
    void Start()
    {
        lastFired = Time.time;
        parentShip = this.transform.parent.gameObject;

        InvokeRepeating("updateTarget", 0f, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (targetShip != null && parentShip.GetComponent<shipStatsManagerScript>().currentHull > 0)
        {
            if (Vector3.Distance(targetShip.transform.position, this.transform.position) <= torpedoRange && parentShip.GetComponent<shipStatsManagerScript>().currentTorpedoes > 0)
            {
                launchTorpedo();
            }
        }

        
    }


    void launchTorpedo()
    {
        if (Time.time > launcherFireRate + lastFired)
        {
            GameObject launchedTorpedo = Instantiate(torpedoPrefab, this.transform.position, this.transform.rotation);
            launchedTorpedo.GetComponent<torpedoScript>().targetShip = targetShip;
            parentShip.GetComponent<shipStatsManagerScript>().currentTorpedoes--;

            lastFired = Time.time;

        }
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
                
                if(nearestEnemy.GetComponent<shipStatsManagerScript>().isTorpedo == false)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
                    
            }
        }

        if (nearestEnemy != null && shortestDistance <= torpedoRange && defensiveFire == true)
        {
            targetShip = nearestEnemy;
        }

    }
}
