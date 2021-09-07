using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpFlashScript : MonoBehaviour
{

    public float flashTime = 1f;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(startTime + flashTime < Time.time)
        {
            Destroy(this.gameObject);
        }
    }
}
