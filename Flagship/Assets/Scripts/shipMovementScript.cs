using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class shipMovementScript : MonoBehaviour
{

    private string shipName;

    private string playerShipTag = "playerShip";
    private string enemyShipTag = "enemyShip";

    public GameObject selectionRing;

    public GameObject thisShip;

    public GameObject targetShip;

    public NavMeshAgent shipNavMeshAgent;
    public Camera playerCamera;

    public bool isSelected = false;

    public Color lineColor = Color.green;
    public float lineWidth = 0.1f;

    private float regularStoppingDistance = 2f;
    public float engagementRange = 9f;

    private bool isEngaging = false;

    public GameObject recentTarget = null;

    public GameObject[] shipWeapons;
    public GameObject[] torpedoLaunchers;

    private GameObject fleetManagerObject;


    //Defensive fire stuff

    //public GameObject indicatorObject;
    public bool defensiveFire = false;
    public Image fireModeImage;
    public Sprite defensiveFireOnIcon;
    public Sprite focusFireOnIcon;


    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        this.GetComponent<Outline>().enabled = false;

        shipName = this.GetComponent<shipStatsManagerScript>().shipName;
        fleetManagerObject = GameObject.Find("FleetManager");

        //fireModeImage = indicatorObject.transform.Find("FocusFireIndicator").gameObject.GetComponent<Image>();
        ///defensiveFireOnIcon = 
    }




    // Update is called once per frame
    void Update()
    {
        
        //If there's a click, and the ship is selected, set a new destination.
        if (Input.GetMouseButtonDown(1) && isSelected)
        {
            //Unity cast a ray from the position of mouse cursor on screen toward the 3D scene
            Ray myRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit myRaycastHit;

            if(Physics.Raycast(myRay,out myRaycastHit))
            {
                if (myRaycastHit.transform != null)
                {

                    
                    GameObject hitObject = myRaycastHit.transform.gameObject;

                    //print("HIT OBJECT: " + hitObject);
                    //If object clicked on is an enemy ship or a friendly ship
                    if (hitObject.CompareTag(enemyShipTag) || hitObject.CompareTag(playerShipTag))
                    {
                        
                        targetShip = hitObject;


                        //If the most recently targeted ship is still alive, reduce that ship's attacker count.
                        if (recentTarget != null)
                        {
                            recentTarget.GetComponent<enemyBehaviorScript>().shipsTargetingThisShip -= 1;
                        }
                        recentTarget = hitObject;


                        //If object clicked on is an enemy ship, lock weapons and engage. 
                        if (hitObject.CompareTag(enemyShipTag))
                        {
                            SetTarget(hitObject);
                            isEngaging = true;

                            targetShip.GetComponent<enemyBehaviorScript>().shipsTargetingThisShip += 1;
                        }
                        //If target is a friendly, do not engage.
                        else
                        {
                            isEngaging = false;
                        }
                        
                        
                        
                        //targetShip.GetComponent<Outline>().enabled = true;
                    }

                    //If the object clicked on is the map itself
                    else
                    {
                        
                        targetShip = null;

                        if (recentTarget != null)
                        {
                            //print(recentTarget);
                            if (recentTarget.CompareTag(enemyShipTag)){
                                recentTarget.GetComponent<enemyBehaviorScript>().shipsTargetingThisShip -= 1;
                            }
                        }
                    }


                    

                }

                //If there is no targeted ship, set a new destination and remove the weapons target.
                if(targetShip == null)
                {
                    shipNavMeshAgent.SetDestination(myRaycastHit.point);

                    SetTarget(null);
                    
                    isEngaging = false;
                }
                
                
            }
        }

        if (isEngaging)
        {
            this.GetComponent<NavMeshAgent>().stoppingDistance = engagementRange;
            
        }
        else
        {
            this.GetComponent<NavMeshAgent>().stoppingDistance = regularStoppingDistance;
        }

        //If there's a click, and the ship is selected, check if the click is on the ship.
        //If the click is on a different ship, deselect it. 
        if (Input.GetMouseButtonDown(0) && isSelected)
        {

            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit myRaycastHit;

            if (Physics.Raycast(myRay, out myRaycastHit))
            {
                if (myRaycastHit.transform != null)
                {

                    GameObject hitObject = myRaycastHit.transform.gameObject;
                    //print(hitObject);
                    
                    //If the object is not this, and the object selected is a player ship, and shift is not held down, deselect this ship.
                    if (hitObject != this.gameObject && hitObject.CompareTag(playerShipTag) && !Input.GetButton("Shift"))
                    {
                        deselectShip();
                        print(this.name + " deselected");
                    }
                }
            }
        }

        //If there is a targetship, set it as the destination
        if(targetShip != null)
        {
            shipNavMeshAgent.SetDestination(targetShip.transform.position);
        }

        //If the ship hasn't jumped out, draw a line to its destination.
        if (!this.gameObject.GetComponent<shipStatsManagerScript>().jumpedOut)
        {
            this.gameObject.GetComponent<LineRenderer>().enabled = true;
            DrawLine(this.transform.position, shipNavMeshAgent.destination, lineColor, lineColor);

        }
        else
        {
            this.gameObject.GetComponent<LineRenderer>().enabled = false;
            SetTarget(null); //This is to disable the ship's targeting after jumping, so that the scaled-down gameobject doesn't keep firing.
        }



        // if (Input.GetButtonDown("FireMode") && isSelected)
        // {
        //     changeFireMode();
        // }

        

    }

    
    public void SetTarget(GameObject target)
    {
        if(shipWeapons != null)
        {
            foreach (GameObject weapon in shipWeapons)
            {
                weapon.GetComponent<shipCannonScript>().targetShip = target;
            }

            
        }

        if(torpedoLaunchers != null)
        {
            foreach (GameObject launcher in torpedoLaunchers)
            {
                launcher.GetComponent<torpedoLauncherScript>().targetShip = target;
            }
        }
        
    }
    public void toggleSelection()
    {
        if (isSelected)
        {
            deselectShip();
        }
        else
        {
            selectShip();
        }
    }


    public void selectShip()
    {
        if(this.gameObject.GetComponent<shipStatsManagerScript>().currentHull > 0) { 
            isSelected = true;
            
            
            //this.GetComponent<Outline>().enabled = true;
            
            selectionRing.GetComponent<MeshRenderer>().enabled = true;
            fleetManagerObject.GetComponent<fleetManagerScript>().AddShipToSelection(this.gameObject);

        }
    }
    public void deselectShip()
    {
        isSelected = false;
        
        //this.GetComponent<Outline>().enabled = false;
        selectionRing.GetComponent<MeshRenderer>().enabled = false;
        fleetManagerObject.GetComponent<fleetManagerScript>().RemoveShipFromSelection(this.gameObject);

    }

    public void ChangeFireMode(bool fireMode)
    {
        defensiveFire = fireMode;

        foreach(GameObject weapon in shipWeapons)
        {
            weapon.GetComponent<shipCannonScript>().defensiveFire = defensiveFire;
        }

        foreach(GameObject torpedoLauncher in torpedoLaunchers)
        {
            torpedoLauncher.GetComponent<torpedoLauncherScript>().defensiveFire = defensiveFire;
        }

        //If it isn't defensive fire, but we have a target selected, return to focusing fire on that target.
        if (!defensiveFire && targetShip != null)
        {
            foreach (GameObject weapon in shipWeapons)
            {
                weapon.GetComponent<shipCannonScript>().targetShip = targetShip;

            }

            
        }

        if(fireModeImage != null)
        {
            if (defensiveFire)
            {
                fireModeImage.sprite = defensiveFireOnIcon;
            }
            else
            {
                fireModeImage.sprite = focusFireOnIcon;
            }
        }
        

        print("Defensive fire: " + defensiveFire);
    }

    public void SetWeaponArmStatus(bool status)
    {
        foreach (GameObject weapon in shipWeapons)
        {
            weapon.GetComponent<shipCannonScript>().weaponArmed = status;
        }
    }


    void DrawLine(Vector3 start, Vector3 end, Color startColor, Color endColor)
    {
        LineRenderer lineRenderer = this.GetComponent<LineRenderer>(); // new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPosition(0, start); //x,y and z position of the starting point of the line
        lineRenderer.SetPosition(1, end);
    }

    public GameObject player;
    public float margin = 0;

    private Vector3[] pts = new Vector3[8];

   /* public void OnGUI()
    {


        Bounds b = thisShip.GetComponent<MeshRenderer>().bounds;
        Camera cam = Camera.main;

        //The object is behind us
        if (cam.WorldToScreenPoint(b.center).z < 0) return;

        //All 8 vertices of the bounds
        pts[0] = cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
        pts[1] = cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
        pts[2] = cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
        pts[3] = cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));
        pts[4] = cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
        pts[5] = cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
        pts[6] = cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
        pts[7] = cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));

        //Get them in GUI space
        for (int i = 0; i < pts.Length; i++) pts[i].y = Screen.height - pts[i].y;

        //Calculate the min and max positions
        Vector3 min = pts[0];
        Vector3 max = pts[0];
        for (int i = 1; i < pts.Length; i++)
        {
            min = Vector3.Min(min, pts[i]);
            max = Vector3.Max(max, pts[i]);
        }

        //Construct a rect of the min and max positions and apply some margin
        Rect r = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        r.xMin -= margin;
        r.xMax += margin;
        r.yMin -= margin;
        r.yMax += margin;

        //Render the box
        

        
       

        Color oldColor = GUI.color; // 1. save current color

        GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        GUI.Box(r,shipName);
        GUI.color = oldColor; // 3. reset to old color

    }

    */    

}

