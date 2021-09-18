using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningBeamManager : MonoBehaviour
{

    
    public string resourcesTag = "resourceNode";
    public string resourceType = "hull";

    private float lastMined;
    public float mineRate = 5;
    public int mineAmount = 1;

    public int resourcesGathered = 0;

    public GameObject fleetManager;

    // Start is called before the first frame update
    void Start()
    {
        lastMined = Time.time;   
    }

    // Update is called once per frame
    void Update()
    {
        if(resourcesGathered > 0 && fleetManager.GetComponent<fleetManagerScript>().isInCombat == false)
        {
            print("TRANSFERRING TO FLEET POOL");
            fleetManager.GetComponent<ResourcesManager>().addToPool(resourceType,resourcesGathered);
            resourcesGathered = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(resourcesTag))
        {
            //print("RESOURCES IN RANGE");
            if (resourceType == (other.GetComponent<ResourcesNodeScript>().resourceType))
            {
               // print("RESOURCE MATCH");
                if (Time.time > lastMined + mineRate)
                {
                 //   print("MINING");
                    lastMined = Time.time;
                    resourcesGathered += other.GetComponent<ResourcesNodeScript>().depleteResources(mineAmount);
                }
            }
            
            
        }
    }
}
