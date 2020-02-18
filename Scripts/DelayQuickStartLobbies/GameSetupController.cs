using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class GameSetupController : MonoBehaviourPunCallbacks
{
    

    GameObject bluePlayer; 
    GameObject redPlayer;

    GameObject blueHealthBar;
    GameObject redHealthBar; 

    GameObject humanScene;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }
        humanScene = GameObject.Find("HumanScene");
        // if (DelayStartLobbyController.photonActive) {
        //     createPlayer();
        //     addHealthBars();
        // }
    }

    void createPlayer() {
        Vector3 bluePos = new Vector3(-2.2f, 0, 0);
        Vector3 redPos = new Vector3(-0.913f, 0, 0);
        bluePlayer = PhotonNetwork.Instantiate(Path.Combine("FighterPrefabs", "BlueFighter"), bluePos, Quaternion.identity);
        bluePlayer.transform.localEulerAngles = new Vector3(0, 90, 0);
        bluePlayer.transform.SetParent(humanScene.transform);
        redPlayer = PhotonNetwork.Instantiate(Path.Combine("FighterPrefabs", "RedFighter"), redPos, Quaternion.identity);
        redPlayer.transform.localEulerAngles = new Vector3(0, -90, 0);
        redPlayer.transform.SetParent(humanScene.transform);
    }

    void Update() {

    }

    void addHealthBars() {
        Vector3 bluePos = new Vector3(-2.06f, 0.54f, 0);
        blueHealthBar = PhotonNetwork.Instantiate(Path.Combine("FighterPrefabs", "BlueHealthBar"), bluePos, Quaternion.identity);
        Vector3 redPos = new Vector3(-2.06f, -0.732f, 0);
        redHealthBar = PhotonNetwork.Instantiate(Path.Combine("FighterPrefabs", "RedHealthBar"), redPos, Quaternion.identity);
        blueHealthBar.transform.localEulerAngles = new Vector3(0, 90, 0);
        redHealthBar.transform.localEulerAngles = new Vector3(0, 90, 0);
    }

}
