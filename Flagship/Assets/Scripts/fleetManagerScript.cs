using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float jumpInWaitSeconds = 5;

    public int jumpedShipsCounter = 0;

    public GameObject[] fleetShips;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerObject = GameObject.Find("GameManager");
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
            jumpFleet();
        }
    }

    public void jumpFleet()
    {
        gameManagerObject.GetComponent<sceneManagerScript>().loadScene();


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

    void FleetStatusUpdate()
    {
        fleetShips = GameObject.FindGameObjectsWithTag(playerShipTag);

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
}
