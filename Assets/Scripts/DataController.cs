using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataController
{
    /*
    Controls data between scenes
        - Which level is loaded

    Expanding this class to include the Leaderboard API
    */

    public static int LevelSelected {
        // lvls 6 to 10 are speedrunmodes 1 - 5
        get; set;
    }

    public static bool[] unlocked = { false, false, false, false, false };

    public static void unlock(int lvl) {
        try {
            unlocked[lvl-1] = true;
        } catch {
        }
    }

    public static bool getLock(int lvl) {
        try {
            return unlocked[lvl-1];
        } catch {
            return false;
        }
    }
    
}
