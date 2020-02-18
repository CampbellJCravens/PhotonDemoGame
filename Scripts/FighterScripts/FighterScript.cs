using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterScript : MonoBehaviour
{

    Animator animator;

    public int health = 10;

    public enum PlayerState { attacking, dodging, blocking, idle, dead, intro, victory }
    public PlayerState playerState = PlayerState.intro;

    GameObject enemy;
    FighterScript enemyScript;

    bool initialized = false;

    public bool victoryBool = false;

    void fighterScriptStart() {
        //animator = transform.parent.gameObject.GetComponent<Animator>();  
        animator = GetComponent<Animator>();  
        if (tag.Contains("Blue")) {
            enemy = GameObject.FindGameObjectWithTag("RedPlayer");
        } else {
            enemy = GameObject.FindGameObjectWithTag("BluePlayer");
        }
        enemyScript = enemy.GetComponent<FighterScript>();
        introAnimation();
    }

    void introAnimation() {
        playerState = PlayerState.intro;
        string animString = "";
        if (tag.Contains("Blue")) {
            animString = "Intro1";
        } else {
            animString = "Intro2";
        }
        playAnimation(animString);
    }

    public void victoryAnimation() {
        playerState = PlayerState.victory;
        string animString = "";
        if (tag.Contains("Blue")) {
            animString = "Victory1";
        } else {
            animString = "Victory2";
        }
        playAnimation(animString);
        victoryBool = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GamesceneSetup.matchStarted) {
            if (!initialized) {
                initialized = true;
                fighterScriptStart();
            }
        } else {
            return;
        }
        if (playerState == PlayerState.dead || playerState == PlayerState.intro || playerState == PlayerState.victory) { return; }
        checkForDeath();
        //programmedAttacks();
        attackUpdate();
        dodgeUpdate();
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


    void programmedAttacks() {
        if (playerState == PlayerState.dead) { return; }
        timer += Time.deltaTime;
        if (tag == "BluePlayer") {
            if (timer >= 3 && !punch1Blue) {
                playerState = PlayerState.attacking;
                punch1Blue = true;
                playAnimation("Punch");
            } else if (timer >= 6 && !jab1Blue) {
                playerState = PlayerState.attacking;
                jab1Blue = true;
                playAnimation("Jab");
            } else if (timer >= 9 && !block1Blue) {
                playerState = PlayerState.dodging;
                block1Blue = true;
                playAnimation("Block");
            } else if (timer >= 12 && !jump1Blue) {
                playerState = PlayerState.dodging;
                jump1Blue = true;
                playAnimation("Jump");
            } else if (timer >= 13 && !kick1Blue) {
                playerState = PlayerState.attacking;
                kick1Blue = true;
                playAnimation("DownSmash");
            } else if (timer >= 18 && !punch2Blue) {
                playerState = PlayerState.attacking;
                punch2Blue = true;
                playAnimation("Punch");
            } else if (timer >= 22 && !jab2Blue) {
                playerState = PlayerState.attacking;
                jab2Blue = true;
                playAnimation("Jab");
            } else if (timer >= 23 && !kick2Blue) {
                playerState = PlayerState.attacking;
                kick2Blue = true;
                playAnimation("DownSmash");
            }
        }
        if (tag == "RedPlayer") {
            if (timer >= 9 && !punch1Red) {
                playerState = PlayerState.attacking;
                punch1Red = true;
                playAnimation("Punch");
            } else if (timer >= 15 && !jab1Red) {
                playerState = PlayerState.attacking;
                jab1Red = true;
                playAnimation("Jab");
            } else if (timer >= 20 && !punch2Red) {
                playerState = PlayerState.attacking;
                punch2Red = true;
                playAnimation("DownSmash");
            }
        }
    }

    void checkForDeath() {
        if (health <= 0 && playerState != PlayerState.dead) {
            playerState = PlayerState.dead;
            playAnimation("Death");
            enemyScript.victoryBool = true;
        }
    }

    void attackUpdate() {
        if (tag == "BluePlayer") {
            blueAttack();
        } else {
            redAttack();
        }
    }

    public void inputReceived(string inputName) {
        print ("receiving input " + inputName);
        List<string> attacks = new List<string>() {
            "Jab", "DownSmash", "Punch" 
        };
        List<string> dodges = new List<string>() {
            "Jump", "Block"
        };
        if (attacks.Contains(inputName)) {
            playerState = PlayerState.attacking;
        }
        if (dodges.Contains(inputName)) {
            playerState = PlayerState.dodging;
        }
        playAnimation(inputName);
    }

    void redAttack() {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            playerState = PlayerState.attacking;
            playAnimation("Jab");
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            playerState = PlayerState.attacking;
            playAnimation("DownSmash");
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            playerState = PlayerState.attacking;
            playAnimation("Punch");
        }
    }

    void blueAttack() {
        if (Input.GetKey(KeyCode.A)) {
            playerState = PlayerState.attacking;
            playAnimation("Jab");
        }
        if (Input.GetKey(KeyCode.S)) {
            playerState = PlayerState.attacking;
            playAnimation("DownSmash");
        }
        if (Input.GetKey(KeyCode.D)) {
            playerState = PlayerState.attacking;
            playAnimation("Punch");
        }
    }

    void dodgeUpdate() {
        if (tag == "BluePlayer") {
            if (Input.GetKey(KeyCode.LeftApple)) {
                playerState = PlayerState.dodging;
                playAnimation("Jump");
            }
            if (Input.GetKey(KeyCode.W)) {
                playerState = PlayerState.dodging;
                playAnimation("Block");
            }
        } else {
            if (Input.GetKey(KeyCode.RightApple)) {
                playerState = PlayerState.dodging;
                playAnimation("Jump");
            }
            if (Input.GetKey(KeyCode.UpArrow)) {
                playerState = PlayerState.dodging;
                playAnimation("Block");
            }
        }
        
    }

    bool animating = false;

    public void playAnimation(string anim) {
        animator.Play(anim);
        // if (animating) { return; }
        // animating = true;
        // string triggerString = anim + "Trigger";
        // print ("animation: triggering " + triggerString);
        // animator.SetTrigger(triggerString);
        //StartCoroutine(resetAnimatingBool());
    }

    IEnumerator resetAnimatingBool() {
        yield return new WaitForSeconds(0.5f);
        animating = false;
    }

    void debugPrint(string message) {
        if (tag == "RedPlayer") {
            print (message);
        }
    }

    public void revive() {
        health = 10;
        animator.SetTrigger("ReviveTrigger");
    }

    public void onAnimationComplete() {
        if (victoryBool) {
            victoryAnimation();
        } else {
            playerState = PlayerState.idle;
        }
    }

}
