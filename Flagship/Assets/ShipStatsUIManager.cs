using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ShipStatsUIManager : MonoBehaviour
{

    public TMP_Text shipNameBar;
    private string shipLabel;

    public GameObject attachedShip;
        

    // Start is called before the first frame update
    void Start()
    {
        if(attachedShip!= null)
        {
            shipLabel = attachedShip.GetComponent<shipStatsManagerScript>().shipName.ToUpper();
        }
        //TODO: move th-is to a 
        if (shipNameBar != null)
        {
            shipNameBar.SetText(shipLabel);
            shipNameBar.enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (attachedShip != null && shipNameBar != null)
        {
            if (attachedShip.GetComponent<shipMovementScript>() != null)
            {
                if (attachedShip.GetComponent<shipMovementScript>().isSelected)
                {
                    shipNameBar.enabled = true;
                    
                }
                else
                {
                    shipNameBar.enabled = false;
                }
            }
        }
    }
    /*
    public void updateHullAndShieldBars()
    {
        if (hullBar != null)
        {
            hullBar.fillAmount = (float)currentHull / (float)maxHull;
        }

        if (shieldBar != null)
        {
            shieldBar.fillAmount = (float)currentShields / (float)maxShields;
        }
    }*/
}
