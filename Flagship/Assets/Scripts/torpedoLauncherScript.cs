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

    public GameObject parentShip;
    // Start is called before the first frame update
    void Start()
    {
        lastFired = Time.time;
        parentShip = this.transform.parent.gameObject;


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
}
