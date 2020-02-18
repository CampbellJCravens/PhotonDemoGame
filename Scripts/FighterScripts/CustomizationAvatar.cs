using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationAvatar : MonoBehaviour
{

    Animator animator;
    
    string animString = "";


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (name == "Heavy") {
            animString = MoveSets.heavyAttack;
        } else if (name == "Heal") {
            animString = MoveSets.heal;
        } else if (name == "Defense") {
            animString = MoveSets.defense;
        } else if (name == "Basic") {
            animString = MoveSets.basicAttack;
        } else if (name == "Special") {
            animString = MoveSets.specialAttack;
        } else if (name == "Intro") {
            animString = MoveSets.intro;
        } else if (name == "Victory") {
            animString = MoveSets.victory;
        } 
        animator.Play(animString);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onAnimationComplete() {
        animator.Play(animString);
    }
}
