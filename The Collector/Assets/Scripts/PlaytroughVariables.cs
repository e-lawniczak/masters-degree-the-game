using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytroughVariables : MonoBehaviour
{
    public static bool PlaytroughLoaded = false;
    public static int PlaytroughId = -1;
    public static float TotalTime = 0f;
    public static int TotalPoints = 0;
    public static int CoinsCollected = 0;
    public static int EnemiesDefeated = 0;
    public static float PercentageProgress = 0;
    public static int Deaths = 0;
    public static float TotalEnemyProxTime = 0f;
    public static float StandingStillTime = 0f;
    public static int Score = 0;
    public static bool IsFinished = false;
    public static float LevelTime_1 = 0f;
    public static int LevelPoints_1 = 0;
    public static int LevelEnemies_1 = 0;
    public static int LevelCoins_1 = 0;
    public static int LevelDeaths_1 = 0;
    public static int LevelEndHp_1 = 0;
    public static bool LevelFinished_1 = false;
    public static float LevelTime_2 = 0f;
    public static int LevelPoints_2 = 0;
    public static int LevelEnemies_2 = 0;
    public static int LevelCoins_2 = 0;
    public static int LevelDeaths_2 = 0;
    public static int LevelEndHp_2 = 0;
    public static bool LevelFinished_2 = false;
    public static float LevelTime_3 = 0f;
    public static int LevelPoints_3 = 0;
    public static int LevelEnemies_3 = 0;
    public static int LevelCoins_3 = 0;
    public static int LevelDeaths_3 = 0;
    public static int LevelEndHp_3 = 0;
    public static bool LevelFinished_3 = false;
    public static int UserId = -1;
    public static DateTime StartTime = DateTime.UtcNow;
    public static DateTime? EndTime = null;
    public static DateTime? LastUpdate = null;
}

public class PlaytroughVariablesInstance
{
    public bool PlaytroughLoaded { get; set; }
    public int PlaytroughId { get; set; }
    public float TotalTime { get; set; }
    public int TotalPoints { get; set; }
    public int CoinsCollected { get; set; }
    public int EnemiesDefeated { get; set; }
    public float PercentageProgress { get; set; }
    public int Deaths { get; set; }
    public float TotalEnemyProxTime { get; set; }
    public float StandingStillTime { get; set; }
    public int Score { get; set; }
    public bool IsFinished { get; set; }
    public float LevelTime_1 { get; set; }
    public int LevelPoints_1 { get; set; }
    public int LevelEnemies_1 { get; set; }
    public int LevelCoins_1 { get; set; }
    public int LevelDeaths_1 { get; set; }
    public int LevelEndHp_1 { get; set; }
    public float LevelTime_2 { get; set; }
    public int LevelPoints_2 { get; set; }
    public int LevelEnemies_2 { get; set; }
    public int LevelCoins_2 { get; set; }
    public int LevelDeaths_2 { get; set; }
    public int LevelEndHp_2 { get; set; }
    public float LevelTime_3 { get; set; }
    public int LevelPoints_3 { get; set; }
    public int LevelEnemies_3 { get; set; }
    public int LevelCoins_3 { get; set; }
    public int LevelDeaths_3 { get; set; }
    public int LevelEndHp_3 { get; set; }
    public int UserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime? LastUpdate { get; set; }
}
