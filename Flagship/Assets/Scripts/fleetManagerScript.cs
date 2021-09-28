using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class fleetManagerScript : MonoBehaviour
{

    public string playerShipTag = "playerShip";
    private GameObject gameManagerObject;

    public List<GameObject> jumpedShips;

    public List<GameObject> selectedShips;


    public bool hasTanker;
    public bool hasEngineering;
    public bool hasMedical;
    public bool hasSWACS;

    private bool fleetJumped = false;
    private float jumpInWaitSeconds = 3;

    public int jumpedShipsCounter = 0;

    public GameObject[] fleetShips;

    public GameObject resourceUI;

    public bool isInCombat = true;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerObject = GameObject.Find("GameManager");
        FleetStatusUpdate();
        InvokeRepeating("FleetStatusUpdate", 0, 5);
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if(jumpedShips.Count == fleetShips.Length)
        {
            jumpFleet();
        }*/

        if (Input.GetButtonDown("FireMode"))
        {
             DefensiveFireOrder();
        }

        if(jumpedShipsCounter == fleetShips.Length)
        {
            StartCoroutine(jumpFleet());
        }
    }

    IEnumerator jumpFleet()
    {

        yield return new WaitForSeconds(jumpInWaitSeconds);

        gameManagerObject.GetComponent<sceneManagerScript>().LoadScene();


        StartCoroutine(jumpFleetCoroutine());
        jumpedShipsCounter = 0;

    }

    IEnumerator jumpFleetCoroutine()
    {
        yield return new WaitForSeconds(jumpInWaitSeconds);

        foreach (GameObject jumpedShip in jumpedShips)
        {
            jumpedShip.GetComponent<shipStatsManagerScript>().jumpIn();
        }
        jumpedShips.Clear();

    }

    public void FleetStatusUpdate()
    {

        GameObject[] fleetShipsNew = GameObject.FindGameObjectsWithTag(playerShipTag);

        //print("FLEETSHIPS");
        foreach(var ship in fleetShips)
        {
            //print(ship);
        }

        //print("FLEETSHIPSNEW");
        foreach (var ship in fleetShipsNew)
        {
            //print(ship);
        }

        //print(fleetShips.SequenceEqual(fleetShipsNew));

        
        if(!fleetShips.SequenceEqual(fleetShipsNew))
        {
            fleetShips = fleetShipsNew;
            this.GetComponent<ResourcesManager>().updateShips(fleetShips);
        }

        

        /*foreach (GameObject fleetShip in fleetShips)
        {
            fleetJumped = true;
            if (!fleetShip.GetComponent<shipStatsManagerScript>().jumpedOut)
            {
                fleetJumped = false;
            }
        }
        if (fleetJumped == true)
        {
            
          //  jumpFleet();
        }*/

    }



    public void AddShipToSelection(GameObject newShip)
    {
        selectedShips.Add(newShip);
    }

    public void RemoveShipFromSelection(GameObject newShip)
    {
        selectedShips.Remove(newShip);
    }

    public void DefensiveFireOrder()
    {

        if (selectedShips.Count > 0)
        {
            bool newSetting = false;
            //TODO
            //What happens if the ship is destroyed? Fix this!
            if (selectedShips[0].GetComponent<shipMovementScript>().defensiveFire == false)
            {
                newSetting = true;
            }
            foreach (GameObject ship in selectedShips)
            {
                ship.GetComponent<shipMovementScript>().ChangeFireMode(newSetting);
            }
        }

        
    }


    public void JumpOrder()
    {

        if (selectedShips.Count > 0)
        {
            //If the first ship in the list is not jumping, then every ship gets set to be jumping. Otherwise, every ship is set to not jump.
            bool newSetting = false;
            if (selectedShips[0].GetComponent<shipStatsManagerScript>().isJumpingWhenReady == false)
            {
                newSetting = true;
            }
            foreach (GameObject ship in selectedShips)
            {
                ship.GetComponent<shipStatsManagerScript>().ChangeJumpSetting(newSetting);
            }
        }

    }

    public void JumpOnce()
    {
        if(selectedShips.Count > 0)
        {
            foreach(GameObject ship in selectedShips)
            {
                ship.GetComponent<shipStatsManagerScript>().jumpOut();
            }
        }
    }
}
