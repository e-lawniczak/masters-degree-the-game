using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytroughVariables : MonoBehaviour
{
    public static bool PlaytroughLoaded = false;
    public static int PlaytroughId = -1;
    public static float? TotalTime = 0f;
    public static int? TotalPoints = 0;
    public static int? CoinsCollected = 0;
    public static int? EnemiesDefeated = 0;
    public static int? PercentageProgress = 0;
    public static int? Deaths = 0;
    public static float TotalEnemyProxTime = 0f;
    public static float StandingStillTime = 0f;
    public static int Score = 0;
    public static bool IsFinished = false;
    public static float? LevelTime_1 = 0f;
    public static int? LevelPoints_1 = 0;
    public static int? LevelEnemies_1 = 0;
    public static int? LevelCoins_1 = 0;
    public static int? LevelDeaths_1 = 0;
    public static int? LevelEndHp_1 = 0;
    public static float? LevelTime_2 = 0f;
    public static int? LevelPoints_2 = 0;
    public static int? LevelEnemies_2 = 0;
    public static int? LevelCoins_2 = 0;
    public static int? LevelDeaths_2 = 0;
    public static int? LevelEndHp_2 = 0;
    public static float? LevelTime_3 = 0f;
    public static int? LevelPoints_3 = 0;
    public static int? LevelEnemies_3 = 0;
    public static int? LevelCoins_3 = 0;
    public static int? LevelDeaths_3 = 0;
    public static int? LevelEndHp_3 = 0;
    public static int UserId = -1;
    public static DateTime StartTime = DateTime.UtcNow;
    public static DateTime? EndTime = null;
    public static DateTime? LastUpdate = null;
}
