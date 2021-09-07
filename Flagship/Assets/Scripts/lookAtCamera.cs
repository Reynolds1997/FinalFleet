using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtCamera : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Leave empty to look at main camera")]
    private GameObject Target = null;

    private Vector3 TargetPosition;

    public GameObject attachedShip = null;
    public float barOffset = 2f;

    public float xRotate = 0;
    public float yRotate = 0;
    public float zRotate = 0;

    void Start()
    {
        if (Target == null)
        {
            Target = Camera.main.gameObject;
        }
    }


    void Update()
    {
        if(attachedShip == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            transform.rotation = Camera.main.transform.rotation;
            transform.rotation *= Quaternion.Euler(xRotate, yRotate, zRotate);
            transform.position = attachedShip.transform.position;
            transform.position += new Vector3(0, barOffset, 0);
        }
        
    }
    /*
    private void LateUpdate()
    {
        if (TargetPosition != Target.transform.position)
        {
            TargetPosition = Target.transform.position;
            transform.LookAt(TargetPosition);
        }
    }*/


/*
    public class Test : MonoBehaviour
    {
        public Transform ship;

        void Update()
        {
            if (ship == null)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.position = ship.position + transform.up * 0.8f;
            }
        }
    }
    */
}
