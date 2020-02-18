using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSets : MonoBehaviour
{

    // Active animations
    public static string intro = "Intro1";
    public static string victory = "Victory1";
    public static string basicAttack = "Punch";
    public static string heavyAttack = "AxeKick";
    public static string defense = "Block";
    public static string heal = "Choke";
    public static string specialAttack = "RangeAttack1";


    // Dictionaries that have each moves stats
    
    public static Dictionary<string, Dictionary<string, int>> basicAttackStats = new Dictionary<string, Dictionary<string, int>>();

    public static Dictionary<string, Dictionary<string, int>> heavyAttackStats = new Dictionary<string, Dictionary<string, int>>();

    // Dictionaries with all move names and their animations
    // Key is the Move's name i.e. "HealingWish"
    // Value is the animation string

    Dictionary<string, string> introPoses = new Dictionary<string, string>() {
        { "Intro1", "Intro1"},
        { "Intro1", "Intro1"}
    };
    Dictionary<string, string> victoryPoses = new Dictionary<string, string>() {
        { "Victory1", "Victory1" },
        { "Victory2", "Victory2" }
    };
    Dictionary<string, string> basicAttacks = new Dictionary<string, string>() {
        { "Punch", "Punch" },
        { "Jab", "Jab" },
        { "Kick", "Kick" },
        { "Sweep", "Sweep" },
        { "LowKick" , "LowKick" },
        { "LowPunch", "LowPunch" },
    };
    Dictionary<string, string> heavyAttacks = new Dictionary<string, string>() {
        { "AxeKick", "AxeKick" },
        { "Uppercut", "Uppercut" },
        { "ForwardSmash", "ForwardSmash" },
        { "HeavySmash", "HeavySmash" },
        { "DownSmash", "HeavySmash" }
    };
    Dictionary<string, string> defenses = new Dictionary<string, string>() {
        {"Block", "Block"},
        {"Jump", "Jump"}
    };
    Dictionary<string, string> heals = new Dictionary<string, string>() {
        {"Healing Wish", "RangeAttack2"},
        {"BDSM", "Choke"},
    };
    Dictionary<string, string> specialAttacks = new Dictionary<string, string>() {
        { "Kamehameha", "RangeAttack1" },
        { "SmashCombo", "SmashCombo" },
        { "Combo1", "Combo1" },
        { "MoveAttack1", "MoveAttack1"},
        { "MoveAttack2", "MoveAttack2" }
    };

    public static void setStatDicts() {
        initBasicAttackStats();
        initHeavyAttackStats();
    }

    static void initBasicAttackStats() {
        basicAttackStats["Punch"] = new Dictionary<string, int>() {
            { "damage", 60 },
            { "accuracy", 100 },
            { "speed", 50 },
            { "cost", 1 },
        };
        basicAttackStats["Jab"] = new Dictionary<string, int>() {
            { "damage", 60 },
            { "accuracy", 100 },
            { "speed", 50 },
            { "cost", 1 },
        };
        basicAttackStats["Kick"] = new Dictionary<string, int>() {
            { "damage", 60 },
            { "accuracy", 100 },
            { "speed", 50 },
            { "cost", 1 },
        };
        basicAttackStats["Sweep"] = new Dictionary<string, int>() {
            { "damage", 60 },
            { "accuracy", 100 },
            { "speed", 50 },
            { "cost", 1 },
        };
        basicAttackStats["LowKick"] = new Dictionary<string, int>() {
            { "damage", 60 },
            { "accuracy", 100 },
            { "speed", 50 },
            { "cost", 1 },
        };
        basicAttackStats["LowPunch"] = new Dictionary<string, int>() {
            { "damage", 60 },
            { "accuracy", 100 },
            { "speed", 50 },
            { "cost", 1 },
        };
    }

    static void initHeavyAttackStats() {
        heavyAttackStats["Uppercut"] = new Dictionary<string, int>() {
            { "damage", 120 },
            { "accuracy", 80 },
            { "speed", 20 },
            { "cost", 2 },
        };
        heavyAttackStats["AxeKick"] = new Dictionary<string, int>() {
            { "damage", 80 },
            { "accuracy", 90 },
            { "speed", 40 },
            { "cost", 2 },
        };
        heavyAttackStats["ForwardSmash"] = new Dictionary<string, int>() {
            { "damage", 60 },
            { "accuracy", 80 },
            { "speed", 50 },
            { "cost", 2 },
        };
        heavyAttackStats["HeavySmash"] = new Dictionary<string, int>() {
            { "damage", 60 },
            { "accuracy", 80 },
            { "speed", 50 },
            { "cost", 2 },
        };
        heavyAttackStats["DownSmash"] = new Dictionary<string, int>() {
            { "damage", 60 },
            { "accuracy", 80 },
            { "speed", 50 },
            { "cost", 2 },
        };
    }

    static void initDefenseMoves() {
        heavyAttackStats["Block"] = new Dictionary<string, int>() {
            { "damageReduction", 60 },
            { "accuracy", 100 },
            { "speed", 100 },
            { "cost", 2 },
            { "healBonus", 0 },
            { "damageBonus", 0 },
            { "specialBonus", 0 },
        };
        heavyAttackStats["Jump"] = new Dictionary<string, int>() {
            { "damageReduction", 100},
            { "accuracy", 90 },
            { "speed", 100 },
            { "cost", 3 },
            { "healBonus", 0 },
            { "damageBonus", 0 },
            { "specialBonus", 0 },
        };
    }


}
