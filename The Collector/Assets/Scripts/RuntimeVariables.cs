using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeVariables : MonoBehaviour
{
    public static string PlayerJwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI3M2NlMDBlOS04OTk3LTQ3NTAtYjM0Ni04ZmZjOTAxYmY0MGIiLCJ1bmlxdWVfbmFtZSI6ImRlZSIsInVpZCI6IjMyIiwibmJmIjoxNzEyNzg1NDUwLCJleHAiOjE3MTI3OTI2NTAsImlhdCI6MTcxMjc4NTQ1MCwiaXNzIjoiQ2xvdGhJc3N1ZXIiLCJhdWQiOiJDbG90aEF1ZGllbmNlIn0.IUtD5do6xGtb_MCJ7DoF2SSdCCgjg17B2Cm-rqlCdpI";
    public static int PlayerId = 32;
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
    public static int CurrentLevel = 0;
    public static float CurrentLevelTime = 0f;
    public static int CurrentLevelPoints = 0;
    public static int CurrentLevelCoins= 0;
    public static int CurrentLevelEnemiesDefeated = 0;

}
