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

    LineRenderer lineRenderer;
    public float beamStartWidth = 1;
    public float beamEndWidth = 1;
    public Color beamStartColor;
    public Color beamEndColor;
    public bool isMining = false;
    public GameObject miningTarget;
    public List<GameObject> miningTargets;

    public bool isPulse = true;
    public float beamDuration = 2.5f;
    

    

    // Start is called before the first frame update
    void Start()
    {
        lastMined = Time.time;
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
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
        if (miningTargets.Count > 0)
        {
            // print("RESOURCE MATCH");
            if (Time.time > lastMined + mineRate)
            {
                //   print("MINING");
                lastMined = Time.time;
                resourcesGathered += miningTargets[0].GetComponent<ResourcesNodeScript>().depleteResources(mineAmount);
                isMining = true;

            }
            if (miningTargets[0] != null)
            {
                drawBeam(this.transform.position, miningTargets[0].transform.position, beamStartColor, beamEndColor);
            }
            else
            {
                miningTargets.Remove(miningTargets[0]);
            }

            if (isPulse && Time.time > beamDuration + lastMined)
            {
                
                lineRenderer.enabled = false;
            }

        }
        else
        {
            lineRenderer.enabled = false;
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(resourcesTag))
        {
            //print("RESOURCES IN RANGE");
            if (resourceType == (other.GetComponent<ResourcesNodeScript>().resourceType))
            {
                
                if (!miningTargets.Contains(other.gameObject)){
                    miningTargets.Add(other.gameObject);

                }
                
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(resourcesTag))
        {
            //print("RESOURCES IN RANGE");
            if (resourceType == (other.GetComponent<ResourcesNodeScript>().resourceType))
            {
                miningTargets.Remove(other.gameObject);
            }
        }


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
}
