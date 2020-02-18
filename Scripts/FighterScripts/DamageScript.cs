using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{

    int damage = 0;

    GameObject player;
    
    FighterScript playerScript;
    GameObject enemy;
    FighterScript enemyScript;

    bool initialized = false;

    void damageScriptStart() {
        if (gameObject.layer == 8) {
            player = GameObject.FindGameObjectWithTag("BluePlayer");
            enemy = GameObject.FindGameObjectWithTag("RedPlayer");
        } else if (gameObject.layer == 9) {
            player = GameObject.FindGameObjectWithTag("RedPlayer");
            enemy = GameObject.FindGameObjectWithTag("BluePlayer");
        }
        playerScript = player.gameObject.GetComponent<FighterScript>();
        enemyScript = enemy.gameObject.GetComponent<FighterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GamesceneSetup.matchStarted) {
            if (!initialized) {
                initialized = true;
                damageScriptStart();
            }
        } else {
            return;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (playerScript.playerState != FighterScript.PlayerState.attacking) { return; }
        if (enemyScript.playerState == FighterScript.PlayerState.dodging) { return; }
        if (enemyScript.playerState == FighterScript.PlayerState.dead) { return; }
        print ("attack: Trigger between " + name + " and " + other.name);
        if (!GamesceneSetup.matchStarted) { return; }
        if (other.gameObject == enemy) {
            print ("attack: Trigger with enemy");
                print ("attack: is attacking");
                print ("attack: enemy is not dodging");
            if (tag == "Punch") {
                damage = 2;
            } else if (tag == "Jab") {
                damage = 1;
            } else if (tag == "DownSmash") {
                damage = 2 ;
            } else {
                damage = 0;
            }
            print ("attack: damage = " + damage);
            enemyScript.health -= damage;
            enemyScript.playAnimation("LightHit");
        } 
    }
}
