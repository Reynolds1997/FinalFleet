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

    GameObject[] fleetShipsArray;
    List<string> fleetShipsList = new List<string>();


    public GameObject shipHullText;
    public GameObject fleetHullText;

    int selectedShipCurrentHull;
    int selectedShipMaxHull;

    GameObject selectedShip;

    public bool isInCombat = false;

    // Start is called before the first frame update
    void Start()
    {
        resourcesUI.SetActive(true);
        changeShip(0);
        resourcesUI.SetActive(resourcesUIEnabled);
    }

    

    // Update is called once per frame
    void Update()
    {
                
    }

    public void toggleResourceUI()
    {
        if (!isInCombat)
        {
            if(resourcesUIEnabled == true)
            {
                disableResourceUI();
            }
            else
            {
                enableResourceUI();

            }
        }
        
    }

    public void enableResourceUI()
    {
        resourcesUIEnabled = true;
        resourcesUI.SetActive(resourcesUIEnabled);
    }

    public void disableResourceUI()
    {
        resourcesUIEnabled = false;
        resourcesUI.SetActive(resourcesUIEnabled);
    }

    public void updateShips(GameObject[] fleetShips)
    {
        fleetShipsArray = fleetShips;
        fleetShipsList.Clear();
        foreach (GameObject starship in fleetShips)
        {
            fleetShipsList.Add(starship.GetComponent<shipStatsManagerScript>().shipName);
        }
        resourcesUI.GetComponentInChildren<TMPro.TMP_Dropdown>().ClearOptions();
        resourcesUI.GetComponentInChildren<TMPro.TMP_Dropdown>().AddOptions(fleetShipsList);
        changeShip(0);
        

        //TODO
        //NOTE: The UI should really only update when there's a change in the fleet - usually when a ship is destroyed. Right now, it'll update away from the ship that's selected.
        //FIX THIS.

        //UPDATE: It's partially fixed. Still not perfect. See fleetManagerScript's updateShips method for changes.
    }


    public void changeShip(int shipValue)
    {
        TMPro.TMP_Dropdown dropDown = resourcesUI.GetComponentInChildren<TMPro.TMP_Dropdown>();

        string shipName = dropDown.options[dropDown.value].text;
        //print(shipName);

        foreach (GameObject ship in fleetShipsArray)
        {
            if(ship.GetComponent<shipStatsManagerScript>().shipName == shipName)
            {
                selectedShip = ship;
            }
        }

        
        selectedShipCurrentHull = selectedShip.GetComponent<shipStatsManagerScript>().currentHull;
        selectedShipMaxHull = selectedShip.GetComponent<shipStatsManagerScript>().maxHull;


        shipHullText.GetComponent<TMPro.TMP_Text>().SetText(selectedShipCurrentHull.ToString() + "/" + selectedShipMaxHull.ToString());
        fleetHullText.GetComponent<TMPro.TMP_Text>().SetText(hullPool.ToString());
    }


}
