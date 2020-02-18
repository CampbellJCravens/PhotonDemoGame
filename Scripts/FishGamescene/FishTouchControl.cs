using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class FishTouchControl : MonoBehaviourPunCallbacks
{

    PhotonView photonView;

    FishMovement fishMovement;

    Rigidbody rigidbody;

    public GameObject touchEffect;

    TMPro.TextMeshProUGUI scoreText;

    FishRPCFunctions fishRPCFunctions;

    // Start is called before the first frame update
    void Start()
    {
        fishRPCFunctions = transform.parent.gameObject.GetComponent<FishRPCFunctions>();
        fishMovement = GetComponent<FishMovement>();
        rigidbody = GetComponent<Rigidbody>();
        scoreText = GameObject.Find("Canvas").transform.Find("gameScore").GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {   
        
    }

    void OnMouseDown() {
        print ("rpc: FISH TOUCH DOWN (1)");
        if (fishMovement.fishState == FishMovement.FishState.swimming) {
            catchFish();
        }
    }

    void catchFish() {
        print ("rpc: FISH TOUCH logic (2)");
        print ("rpc: PV isMine (3)");
        //photonView.RPC("RPC_sendInput", RpcTarget.MasterClient, fishMovement.input, GamesceneSetup.playerColor);
        fishRPCFunctions.callRPCSendInput(fishMovement.input);
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ; 
        fishMovement.fishState = FishMovement.FishState.caught;
        print ("SET TO CAUGHT ***********");
        ScoreScript.gameScore += 1;
        scoreText.text = ScoreScript.gameScore.ToString();
        Vector3 effectPoint = new Vector3(transform.position.x, 0, transform.position.z);

        GameObject duplicate = Instantiate(touchEffect, effectPoint, touchEffect.gameObject.transform.rotation);
        StartCoroutine(DestroyEffect(duplicate));
        
    }

    IEnumerator DestroyEffect(GameObject effect) {
        yield return new WaitForSeconds(2);
        Destroy(effect);
    }

    void OnTriggerEnter(Collider other) {
        if (other.name == "ResetFish") {
            resetFish();
        }
    }

    bool resetBool = false;

    void resetFish() {
        if (resetBool) { return; }
        resetBool = true;
        Gamescript.activeFishTypes.Remove(fishMovement.input);
        Gamescript.createNewFish("Fish", Gamescript.randomSpawn());
        fishRPCFunctions.destroyFish();
        // if (photonView.IsMine) {
        //     print ("DESTROYING FISH " + transform.parent.name);
        //     PhotonNetwork.Destroy(this.transform.parent.gameObject);
        // }
    }

    
    // [PunRPC] void RPC_sendInput(string input, string playerColor) {
    //     print ("rpc: running RPC (4)");
    //     if (playerColor == "Blue") {
    //         blueFighterScript.inputReceived(input);
    //     } else {
    //         redFighterScript.inputReceived(input);
    //     }
    // }

    
}
