using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{

    GameObject player;
    FighterScript playerScript;

    Color healthColor;

    List<GameObject> healthArray = new List<GameObject>();

    int startHealth = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (name.Contains("Blue")) {
            player = GameObject.FindGameObjectWithTag("BluePlayer");
            healthColor = Color.blue;
        } else {
            player = GameObject.FindGameObjectWithTag("RedPlayer");
            healthColor = Color.red;
        }
        playerScript = player.GetComponent<FighterScript>();
        startHealth = playerScript.health;
        for (int i = 0; i < startHealth; i++) {
            createHealthSquare(i);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < startHealth; i++) {
            GameObject health = healthArray[i];
            int healthIndex = i + 1;
            if (playerScript.health < healthIndex) {
                health.SetActive(false);
            } else {
                health.SetActive(true);
            }
        }
    }

    void createHealthSquare(int index) {
        float xScale = 1;
        float yScale = 1;
        float zScale = 0.09f;
        float zDiff = 0.1f;
        float zStart = -0.456f;

        GameObject square = GameObject.CreatePrimitive(PrimitiveType.Cube);
        square.GetComponent<BoxCollider>().enabled = false;
        square.name = "healthSquare" + index;
        square.GetComponent<Renderer>().material.color = healthColor;
        square.transform.localScale = new Vector3(xScale, yScale, zScale);
        square.transform.position = new Vector3(0.01f, 0, zStart + (zDiff * index));
        square.transform.SetParent(this.transform, false);
        healthArray.Add(square);
    }
}
