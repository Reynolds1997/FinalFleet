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

    

    //public List<GameObject> targetShipsList;
    public GameObject podManager;
    public GameObject connectedShip;

    public float currentAlertLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
        //targetShipsList = targetShipsList.Where(item => item != null).ToList();
        //TODO: This is horribly inefficient. Clean this up.

        currentAlertLevel = podManager.GetComponent<EnemyPodScript>().alertLevel;

        if (currentAlertLevel >= podManager.GetComponent<EnemyPodScript>().redAlertThreshold)
        {
            alertnessIndicator.GetComponent<MeshRenderer>().material = redAlertMaterial;
            //podManager.GetComponent<EnemyPodScript>().redAlert(currentAlertLevel);
        }
        else if (currentAlertLevel >= podManager.GetComponent<EnemyPodScript>().yellowAlertThreshold)
        {
            alertnessIndicator.GetComponent<MeshRenderer>().material = yellowAlertMaterial;
        }
        else
        {
            alertnessIndicator.GetComponent<MeshRenderer>().material = greenAlertMaterial;
           // podManager.GetComponent<EnemyPodScript>().alertedUnits.Remove(this.gameObject.transform.parent.gameObject);
        }


        //if(currentAlertLevel > podManager.GetComponent<EnemyPodScript>().alertLevel)
        //{
        //    podManager.GetComponent<EnemyPodScript>().alertLevel = currentAlertLevel;
        // }





        
    }


    private void OnTriggerEnter(Collider other)
    {
        //print(other.gameObject);
        if (other.gameObject.CompareTag("playerShip"))
        {
            //targetShipsList.Add(other.gameObject);
            podManager.GetComponent<EnemyPodScript>().addTargetToList(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //print(other.gameObject);
        if (other.gameObject.CompareTag("playerShip"))
        {
            podManager.GetComponent<EnemyPodScript>().removeTargetFromList(other.gameObject);
        }
    }


}
