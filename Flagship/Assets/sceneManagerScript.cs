using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneManagerScript : MonoBehaviour
{

    public Material mat1;

    public Material mat2;

    public GameObject activeSceneData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadScene()
    {
        //Delete all objects in scene aside from the ships in the fleet.
        //Get data from activeSceneData and use that to construct the next scene.


        


        RenderSettings.skybox = mat2;
        
    }
}
