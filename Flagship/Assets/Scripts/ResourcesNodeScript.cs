using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesNodeScript : MonoBehaviour
{

    public string resourceType;
    public int resourceAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int depleteResources(int depletionRate)
    {
        if(resourceAmount >= depletionRate)
        {
            resourceAmount -= depletionRate;
            return depletionRate;
        }
        else
        {
            resourceAmount -= depletionRate;
            return resourceAmount;
        }

    }


}
