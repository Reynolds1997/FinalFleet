using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class shipStatsManagerScript : MonoBehaviour
{

    NavMeshAgent agent;

    public string shipName;
    public bool isFlagship = false;

    public int maxHull = 10;
    public int maxShields = 5;
    public int maxTorpedoes = 10;
    public int maxFuel = 10;
    public int maxCrew = 10;
    public int maxMorale = 10;

    public int currentHull;
    public int currentShields;
    public int currentTorpedoes;
    public int currentFuel;
    public int currentCrew;
    public int currentMorale;

    public bool isAlive = true;
    public bool isDerelict = false;

    public float deathTime = 0.5f;

    public bool isTorpedo = false;

    public float lastHit;
    public float shieldRegenDelay = 3f;
    public float shieldUnitRegenDelay = 0.4f;
    public float lastRegen;

    public Image hullBar;
    public Image shieldBar;
    public TMP_Text shipNameBar;

    [Header("FTL Jump Variables")]
    public float maxJumpTimer = 60;
    public float minJumpTimer = 30;
    public float jumpTimerCounter;
    public bool isJumpingWhenReady = true;
    public bool jumpedOut = false;

    public GameObject fleetManagerObject;

    private Vector3 jumpPosition;

    public GameObject explosionEffect;
    public GameObject preExplosionEffect;

    public float shieldBubbleSize = 1f;
    public GameObject shieldImpactEffect;
    public GameObject hullImpactEffect;

    public GameObject jumpFlare;

    private Vector3 jumpFlareOffset = new Vector3(0, 2, 0);



    // Start is called before the first frame update
    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();

        currentHull = maxHull;
        currentShields = maxShields;
        currentTorpedoes = maxTorpedoes; 
        currentFuel = maxFuel;
        currentCrew = maxCrew;
        currentMorale = maxMorale;

        lastHit = Time.time;
        lastRegen = Time.time;

        fleetManagerObject = GameObject.Find("FleetManager");

        //shieldImpactEffect = GameObject.Find("Assets/Prefabs/shieldImpactEffect.prefab");
       // hullImpactEffect = GameObject.Find("Assets/Prefabs/hullImpactEffect.prefab");

        jumpTimerCounter = Random.Range(minJumpTimer, maxJumpTimer);



        
        //shipNameBar.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //If it's been more than X seconds since the ship last took damage, and the current shields are lower than the max shields, 
        if (Time.time > shieldRegenDelay + lastHit && currentShields < maxShields)
        {
            //Note: have shields regen more slowly
            //currentShields++;

            if(Time.time > shieldUnitRegenDelay + lastRegen)
            {
                currentShields++;
                shieldBar.fillAmount = (float)currentShields / (float)maxShields;
                lastRegen = Time.time;
            }

        }

        if(jumpTimerCounter > 0)
        {
            jumpTimerCounter -= Time.deltaTime;
        }

        if(jumpTimerCounter <= 0 && isJumpingWhenReady && !jumpedOut)
        {
            jumpOut();
        }

        
    }

    public void ChangeJumpSetting(bool newSetting)
    {
        isJumpingWhenReady = newSetting;
        print("Jumping out: " + isJumpingWhenReady);

        
    }

    public void jumpOut()
    {
        //Instantiate jump VFX that cover up the ship just disappearing
        Instantiate(jumpFlare, this.transform.position + jumpFlareOffset,this.transform.rotation);
        jumpedOut = true;

        this.gameObject.transform.parent.gameObject.transform.localScale = new Vector3(0, 0, 0);

        
        this.gameObject.GetComponent<shipMovementScript>().SetTarget(null);
        this.gameObject.GetComponent<shipMovementScript>().SetWeaponArmStatus(false);

        fleetManagerObject.GetComponent<fleetManagerScript>().jumpedShips.Add(this.gameObject);

        jumpPosition = this.gameObject.transform.parent.transform.position;

        fleetManagerObject.GetComponent<fleetManagerScript>().jumpedShipsCounter++;



       // print(this.name + " Jump Position " + jumpPosition);
    }

    public void jumpIn()
    {
        Instantiate(jumpFlare, agent.destination + jumpFlareOffset, this.transform.rotation);
        this.gameObject.GetComponent<shipMovementScript>().SetWeaponArmStatus(true);


        jumpTimerCounter = Random.Range(minJumpTimer, maxJumpTimer);
        jumpedOut = false;
        this.gameObject.transform.parent.gameObject.transform.localScale = new Vector3(1, 1, 1);

        agent.Warp(agent.destination);
        
        //TODO: there's a bug where the ships will periodically get jumped in at their origin points, not at their destinations. Try and resolve this.
       // print(this.name + " Current Position " + this.transform.position);
    }

    //This function takes damage and applies it to the ship's shields and hull bars.
    public void takeDamage(int damage)
    {
        

        if(currentShields <= 0)
        {
            currentHull -= damage;

            if (hullImpactEffect != null)
            {
                Instantiate(hullImpactEffect, this.transform.position, this.transform.rotation);
            }

        //    if (hullBar != null)
      //      {
       //         hullBar.fillAmount = (float)currentHull / (float)maxHull;
       //     }

            if (currentHull < maxHull / 3)
            {
                currentCrew -= damage;
            }
            if (currentHull <= 0)
            {
                isAlive = false;
                if(this.gameObject.GetComponent<shipMovementScript>() != null)
                {
                    this.gameObject.GetComponent<shipMovementScript>().deselectShip();
                }
                
                StartCoroutine(destroyShip());
            }
            else if (currentCrew <= 0)
            {
                isDerelict = true;
            }
        }
        else
        {
            currentShields -= damage;

            if (shieldImpactEffect != null)
            {
                Instantiate(shieldImpactEffect, this.transform.position, this.transform.rotation);
            }


          //  if (shieldBar != null)
       //     {
        //        shieldBar.fillAmount = (float)currentShields / (float)maxShields;
        //    }
        }

        lastHit = Time.time;

        updateHullAndShieldBars();

        //print(this.name + " hull: " + currentHull);
    }




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
    }
    public IEnumerator destroyShip()
    {
        //print("Destroying " + this.name);
        if (isFlagship)
        {
            //Trigger game over for game manager
        }

        

        yield return new WaitForSeconds(deathTime / 2);

        if (preExplosionEffect != null)
        {
            Instantiate(preExplosionEffect, transform.position, transform.rotation);
        }

        


        yield return new WaitForSeconds(deathTime);

        if (explosionEffect != null) { 
            if (agent != null)
            { 
                Instantiate(explosionEffect, agent.nextPosition, transform.rotation);
            }
            else
            {
                Instantiate(explosionEffect, transform.position, transform.rotation);
            }
        }
        Destroy(this.gameObject);
    }
}
