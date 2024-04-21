using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeVariables : MonoBehaviour
{
    public static string PlayerJwtToken = "";
    public static int PlayerId = -1;
    public static string PlayerEmail = "";
    public static string UserName = "";
    public static float MasterVolume = 0.24f;
    public static float SFXVolume = 0.5f;
    public static int TotalPoints = 0;
    public static float TotalTime = 0f;
    public static List<int> defeatedEnemies = new List<int>();
    public static List<int> collectedCoins = new List<int>();
    public static bool GameWon = false;
    public static bool GameStarted = false;
    public static readonly string apiUrl = "https://erykmgr.thinq.pl";
    public static bool IsControlGroup = true;
    public static int HighScore = 0;
    public static int CurrentLevel = 1;
    public static int CurrentHp = -1;
    public static float CurrentLevelTime = 0f;
    public static int CurrentLevelPoints = 0;
    public static int CurrentLevelDeaths = 0;
    public static int CurrentLevelCoins = 0;
    public static int CurrentLevelEnemiesDefeated = 0;
    internal static bool isLoading = false;
    public static readonly int TotalCoinsInGame = 108;
    public static readonly int TotalEnemiesInGame = 41;



    #region DifficultySettings
    public static readonly float BasicEnemySpeed = 5.4f;
    public static readonly int BasicEnemyHp = 5;
    public static readonly float BasicEnemyRecoil = 85f;

    public static readonly float FlyingEnemySpeed = 10f;
    public static readonly float FlyingEnemyLockOnSpeed = 35f;
    public static readonly float FlyingEnemyLockOnWait = 1f;
    public static readonly float FlyingEnemyFirstLockOnWait = 0.1f;
    public static readonly float FlyingEnemyLockOnOffset = 0f;
    public static readonly float FlyingEnemyLockOnRadius = 15f;
    public static readonly float FlyingEnemyActivationRadius = 45f;

    public static readonly int CoinPoints = 25;
    public static readonly float CannonBulletSpeed = 10f;
    public static readonly float CannonBulletReload = 2f;
    public static readonly float CannonBulletLifetime = 5f;

    private static readonly int EnemyPoints = 20;
    public static readonly int BasicEnemyPoints = EnemyPoints + 30;
    public static readonly int FlyingEnemyPoints = EnemyPoints;

    public static readonly float PlayerAttackRadius = 1.25f;
    public static readonly float PlayerAttackRadiusUp = 1.25f;
    public static readonly float PlayerAttackRadiusDown = 1.45f;

    #endregion



}
