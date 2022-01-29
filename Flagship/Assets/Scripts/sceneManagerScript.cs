using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManagerScript : MonoBehaviour
{

    public Material mat1;

    public Material mat2;

    public GameObject activeSceneData;

    public string enemyShipTag = "enemyShip";
    public string neutralShipTag = "neutralShip";
    public string terrainObjectTag = "terrainObject";

    public GameObject[] enemyShips;

    public GameObject[] neutralShips;
    public GameObject[] terrainObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadLevel("MainMenu");
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            LoadLevel(SceneManager.GetActiveScene().name);
        }
    }

    public void LoadScene()
    {
        
        ClearScene();

        //Get data from activeSceneData and use that to construct the next scene.



        RenderSettings.skybox = mat2;
        
    }
    //Delete all objects in scene aside from the ships in the fleet.
    public void ClearScene()
    {
        enemyShips = GameObject.FindGameObjectsWithTag(enemyShipTag);
        terrainObjects = GameObject.FindGameObjectsWithTag(terrainObjectTag);

        foreach(GameObject enemyShip in enemyShips)
        {
            Destroy(enemyShip);
        }
    }


    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
