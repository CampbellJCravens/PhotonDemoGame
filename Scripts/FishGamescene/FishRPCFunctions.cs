using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FishRPCFunctions : MonoBehaviour
{
    
    FighterScript blueFighterScript;
    FighterScript redFighterScript;

    PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        blueFighterScript = GameObject.FindGameObjectWithTag("BluePlayer").GetComponent<FighterScript>();
        redFighterScript = GameObject.FindGameObjectWithTag("RedPlayer").GetComponent<FighterScript>();
    }

    public void callRPCSendInput(string input) {
        photonView.RPC("RPC_sendInput", RpcTarget.MasterClient, input, GamesceneSetup.playerColor);
    }

    public void destroyFish() {
        PhotonNetwork.Destroy(this.gameObject);
    }


    [PunRPC] void RPC_sendInput(string input, string playerColor) {
        print ("rpc: running RPC (4)");
        if (playerColor == "Blue") {
            blueFighterScript.inputReceived(input);
        } else {
            redFighterScript.inputReceived(input);
        }
    }
}
