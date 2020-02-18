using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationScene : MonoBehaviour
{

    public enum BasicAttacks { Punch, Jab, Kick, Sweep, LowKick, LowPunch };
    public BasicAttacks basicAttacks = BasicAttacks.Punch;

    public int basicDamage = 0;
    public int basicAccuracy = 100;
    public int basicSpeed = 0;

    public enum HeavyAttacks { AxeKick, Uppercut, ForwardSmash, HeavySmash, DownSmash };
    public HeavyAttacks heavyAttacks = HeavyAttacks.AxeKick;
    public int heavyDamage = 0;
    public int heavyAccuracy = 100;
    public int heavySpeed = 0;

    public enum DefenseMoves { Block, Jump };
    public DefenseMoves defenseMoves = DefenseMoves.Block;

    public int defense = 0;

    public enum HealMoves { BDSM, Heal1 };
    public HealMoves healMoves = HealMoves.Heal1;

    public int healthGain = 0;

    public enum SpecialAttacks { RangeAttack1, SmashCombo, Combo1, MoveAttack1, MoveAttack2 };
    public SpecialAttacks specialAttacks = SpecialAttacks.RangeAttack1;

    public int specialDamage = 0;
    public int specialAccuracy = 100;
    public int specialSpeed = 0;
    public int specialClicksToActivate = 0;
    public int specialDefense = 0;
    public int specialHealthGain = 0;

    public enum IntroPoses { intro1, intro2 };
    public IntroPoses introPoses = IntroPoses.intro1;

    public enum VictoryPoses { victory1, victory2 };
    public VictoryPoses victoryPoses = VictoryPoses.victory1;

    Dictionary<string, string> moveNames = new Dictionary<string, string>();


    // Start is called before the first frame update
    void Start()
    {
        MoveSets.setStatDicts();
        moveNames = new Dictionary<string, string>() {
            { "BDSM", "Choke" },
            { "Heal1", "RangeAttack2" },
        };
    }

    // Update is called once per frame
    void Update()
    {
        displayBasicAttackStats();
    }

    void displayBasicAttackStats() {
        basicDamage = MoveSets.basicAttackStats[basicAttacks.ToString()]["damage"];
        basicAccuracy = MoveSets.basicAttackStats[basicAttacks.ToString()]["accuracy"];
        basicSpeed = MoveSets.basicAttackStats[basicAttacks.ToString()]["speed"];
    }

    void displayHeavyAttackStats() {
        heavyDamage = MoveSets.basicAttackStats[basicAttacks.ToString()]["damage"];
        heavyAccuracy = MoveSets.basicAttackStats[basicAttacks.ToString()]["accuracy"];
        heavySpeed = MoveSets.basicAttackStats[basicAttacks.ToString()]["speed"];
    }
}
