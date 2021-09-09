using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour
{

    public GameObject resourcesUI;
    private bool resourcesUIEnabled = false;

    public int hullPool = 0;
    public int fuelPool = 0;
    public int torpedoPool = 0;
    public int personnelPool = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleResourceUI()
    {
        resourcesUIEnabled = !resourcesUIEnabled;
        resourcesUI.SetActive(resourcesUIEnabled);
    }

    public void updateShips(GameObject[] fleetShips)
    {
        List<string> fleetShipsList = new List<string>();

        foreach (GameObject starship in fleetShips)
        {
            fleetShipsList.Add(starship.GetComponent<shipStatsManagerScript>().shipName);
        }
        resourcesUI.GetComponentInChildren<TMPro.TMP_Dropdown>().ClearOptions();
        resourcesUI.GetComponentInChildren<TMPro.TMP_Dropdown>().AddOptions(fleetShipsList);
    }
}
