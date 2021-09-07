using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class torpedoScript : MonoBehaviour
{

    public string playerShipTag = "playerShip";
    public string enemyShipTag = "enemyShip";
    public float moveSpeed = 5f;
    //This determines how long it takes after the torpedo has left the tube for it to become live. Useful for preventing accidental detonations on launching.
    public float safetyTime = 1f;

    public float rotateSpeed = 200f;

    public GameObject targetShip;

    private Rigidbody rb;

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;

    //public bool isEnemyTorpedo = true;

    public int torpedoDamage = 5;
    private float launchTime;

    private bool hasDetonated = false;

    public GameObject torpedoModel;

    public GameObject explosionEffect;

    public float detonationRange = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        launchTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {


        if (targetShip == null)
        {
            //detonateWarhead(null);
        }
        //If target ship is still active, fly forward towards it.
        else
        {
            if (this.transform.position.y != targetShip.transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, targetShip.transform.position.y, transform.position.z);
            }

            Quaternion neededRotation = Quaternion.LookRotation((targetShip.transform.position - transform.position));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, neededRotation, Time.deltaTime * rotateSpeed);

            rb.velocity = transform.forward * moveSpeed;
        }

        //If the targetShip isn't null, but it's already jumped out or the torpedo has been destroyed, detonate. 
        if (targetShip!= null && (this.gameObject.GetComponent<shipStatsManagerScript>().currentHull <= 0 || targetShip.GetComponent<shipStatsManagerScript>().jumpedOut) && hasDetonated == false)
        {          
            detonateWarhead(null);
        }
    
        //If the targetShip is very close, detonate the warhead. This is to help avoid torpedoes getting stuck on colliders.
        if(targetShip != null && Vector3.Distance(this.transform.position,targetShip.transform.position) <= detonationRange && hasDetonated == false)
        {
            detonateWarhead(targetShip);
        }


    }

    void OnCollisionEnter(Collision collision)
    {
        bool doNotDetonate = false;

        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;
        
        if(collision.gameObject.CompareTag(playerShipTag) || collision.gameObject.CompareTag(enemyShipTag))
        {
            if (collision.gameObject.GetComponent<shipStatsManagerScript>().isTorpedo)
            {
                doNotDetonate = true;
            }
            else
            {
                doNotDetonate = false;
            }
        }

        if (!doNotDetonate && Time.time > launchTime+safetyTime && !hasDetonated)
        {
            detonateWarhead(collision.gameObject);
            hasDetonated = true;
        }
    }


    void detonateWarhead(GameObject impactTarget)
    {
        //print("Detonating!!!");

        moveSpeed = 0f;
        hasDetonated = true;

        torpedoModel.GetComponent<MeshRenderer>().enabled = false;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        
        //print(impactTarget);
        if (impactTarget != null)
        {
            impactTarget.GetComponent<shipStatsManagerScript>().takeDamage(torpedoDamage);
        }
        StartCoroutine(this.gameObject.GetComponent<shipStatsManagerScript>().destroyShip());
    }
}
