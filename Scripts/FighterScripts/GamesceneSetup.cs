using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GamesceneSetup : MonoBehaviourPunCallbacks
{

    public bool showCatScene = false;
    public static string playerColor = "MasterClient";
    public static bool matchStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        if (DelayStartLobbyController.photonActive) {
            if (PhotonNetwork.IsMasterClient) {
                print ("I AM MASTER CLIENT");
                GameObject.Find("HumanScene").transform.Find("Presentation").gameObject.SetActive(true);
                GameObject.Find("CatScene").transform.Find("Presentation").gameObject.SetActive(false);
            } else {
                print ("i am NOT master client");
                GameObject.Find("HumanScene").transform.Find("Presentation").gameObject.SetActive(false);
                GameObject.Find("CatScene").transform.Find("Presentation").gameObject.SetActive(true);
            }
        } else {
                print ("photon not active");
            if (showCatScene) {
                GameObject.Find("HumanScene").transform.Find("Presentation").gameObject.SetActive(false);
                GameObject.Find("CatScene").transform.Find("Presentation").gameObject.SetActive(true);
            } else {
                GameObject.Find("HumanScene").transform.Find("Presentation").gameObject.SetActive(true);
                GameObject.Find("CatScene").transform.Find("Presentation").gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        searchForBothPlayers();
    }

    void searchForBothPlayers() {
        if (matchStarted) { return; }
        print ("BluePlayer exists = " + GameObject.FindGameObjectWithTag("BluePlayer"));
        print ("RedPlayer exists = " + GameObject.FindGameObjectWithTag("RedPlayer"));
        if (GameObject.FindGameObjectWithTag("BluePlayer") && GameObject.FindGameObjectWithTag("RedPlayer")) {
            print ("MATCH STARTING");
            matchStarted = true;
        }
    }

}
