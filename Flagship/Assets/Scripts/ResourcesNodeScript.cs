using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesNodeScript : MonoBehaviour
{

    public string resourceType;
    public int resourceAmount;
    public ParticleSystem debrisParticles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(resourceAmount <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public int depleteResources(int depletionRate)
    {
        Instantiate(debrisParticles, this.transform);
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
