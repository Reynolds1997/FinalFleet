using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyVisionRadiusScript : MonoBehaviour
{

    public GameObject alertnessIndicator;
    public Material greenAlertMaterial;
    public Material yellowAlertMaterial;
    public Material redAlertMaterial;

    public float currentAlertLevel = 0;
    public float yellowAlertThreshold = 1;
    public float redAlertThreshold = 5;

    public List<GameObject> targetShipsList;
    public GameObject pod;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetShipsList = targetShipsList.Where(item => item != null).ToList();
        //TODO: This is horribly inefficient. Clean this up.
        if (targetShipsList.Count > 0)
        {
            currentAlertLevel += Time.deltaTime;
            if(currentAlertLevel >= redAlertThreshold)
            {
                alertnessIndicator.GetComponent<MeshRenderer>().material = redAlertMaterial;
            }
            else if(currentAlertLevel >= yellowAlertThreshold)
            {
                alertnessIndicator.GetComponent<MeshRenderer>().material = yellowAlertMaterial;
            }
        }
        else
        {
            alertnessIndicator.GetComponent<MeshRenderer>().material = greenAlertMaterial;
            currentAlertLevel -= Time.deltaTime;
            
        }

        currentAlertLevel = Mathf.Clamp(currentAlertLevel, 0, 5);
    }


    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject);
        if (other.gameObject.CompareTag("playerShip"))
        {
            targetShipsList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print(other.gameObject);
        if (other.gameObject.CompareTag("playerShip"))
        {
            targetShipsList.Remove(other.gameObject);
        }
    }


}
