using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CatButtonScript : MonoBehaviour
{

    PhotonView photonView;
    float minX = -7.79f;
    float maxX = 7.79f;
    float minY = 4.93f;
    float maxY = -2.98f;
    float moveSpeed = 5f;

    Vector3 targetPosition;

    Vector3 startScale;

    GameObject player;
    FighterScript playerScript;

    bool initialized = false;

    void catButtonScriptStart() {
        photonView = GetComponent<PhotonView>();
        startScale = transform.localScale;
        determinePlayer();
        randomPosition();
    }

    void determinePlayer() {
        print ("determinePlayer playerColor = " + GamesceneSetup.playerColor);
        player = GameObject.FindGameObjectWithTag(GamesceneSetup.playerColor + "Player");
        playerScript = player.GetComponent<FighterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GamesceneSetup.matchStarted) {
            if (!initialized) {
                initialized = true;
                catButtonScriptStart();
            }
        } else {
            return;
        }
        moveButton();
        //programButtonClicks();
    }


    void randomPosition() {
        randomSpeed();
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        targetPosition = new Vector3(x, y, 0.5f);
    }

    void randomSpeed() {
        moveSpeed = Random.Range(3f, 8f);
    }

    void moveButton() {
        if (transform.position != targetPosition) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        } else {
            randomPosition();
        }
    }

    void OnMouseDown() {
        float scaleUp = 0.8f;
        transform.localScale = new Vector3(startScale.x + scaleUp, startScale.y + scaleUp, startScale.z + scaleUp);
        print ("On Mouse Down color: " + GamesceneSetup.playerColor);
        photonView.RPC("RPC_sendInput", RpcTarget.MasterClient);
    }

    void OnMouseUp() {
        transform.localScale = startScale;
    }

    void selectButton(string buttonName) {
        if (name == buttonName) {
            float scaleUp = 0.8f;
            transform.localScale = new Vector3(startScale.x + scaleUp, startScale.y + scaleUp, startScale.z + scaleUp);
            StartCoroutine(deselectButton());
        }
    }

    IEnumerator deselectButton() {
        yield return new WaitForSeconds(0.5f);
        transform.localScale = startScale;
    }

    void RPC_sendInput() {
        print ("sending input color: " + GamesceneSetup.playerColor);
        playerScript.inputReceived(name);
    }

    float timer = 0;
    
    bool punch1Blue = false;
    bool jab1Blue = false;
    bool block1Blue = false;
    bool kick1Blue = false;
    bool jump1Blue = false;
    bool punch2Blue = false;
    bool jab2Blue = false;
    bool kick2Blue = false;
    
    bool punch1Red = false;
    bool jab1Red = false;
    bool punch2Red = false;

    void programButtonClicks() {
        timer += Time.deltaTime;
        // if (timer >= 2 && !punch1Blue) {
        //     punch1Blue = true;
        //     selectButton("Punch");
        // } else if (timer >= 3 && !jab1Blue) {
        //     jab1Blue = true;
        //     selectButton("Jab");
        // } else if (timer >= 4 && !block1Blue) {
        //     block1Blue = true;
        //     selectButton("Block");
        // } else if (timer >= 6 && !jump1Blue) {
        //     jump1Blue = true;
        //     selectButton("Jump");
        // } else if (timer >= 7 && !kick1Blue) {
        //     kick1Blue = true;
        //     selectButton("DownSmash");
        // } else if (timer >= 10 && !punch2Blue) {
        //     punch2Blue = true;
        //     selectButton("Punch");
        // } else if (timer >= 14 && !jab2Blue) {
        //     jab2Blue = true;
        //     selectButton("Jab");
        // } else if (timer >= 15 && !kick2Blue) {
        //     kick2Blue = true;
        //     selectButton("DownSmash");
        // }

        if (timer >= 4 && !punch1Red) {
            punch1Red = true;
            selectButton("Punch");
        } else if (timer >= 8 && !jab1Red) {
            jab1Red = true;
            selectButton("Jab");
        } else if (timer >= 12 && !punch2Red) {
            punch2Red = true;
            selectButton("Punch");
        }
    }
}
