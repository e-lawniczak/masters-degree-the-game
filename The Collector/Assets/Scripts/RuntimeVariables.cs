using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeVariables : MonoBehaviour
{
    public static string PlayerJwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJhN2MzOGMzYi00MmEyLTRlYjctOTE0Yi03ZjY4NWI1MjAzNDUiLCJ1bmlxdWVfbmFtZSI6ImRlZSIsInVpZCI6IjMyIiwibmJmIjoxNzEyNTk4ODI1LCJleHAiOjE3MTI2MDYwMjUsImlhdCI6MTcxMjU5ODgyNSwiaXNzIjoiQ2xvdGhJc3N1ZXIiLCJhdWQiOiJDbG90aEF1ZGllbmNlIn0.0QtdGATQ9jneiNomusRy0wlBNpanTLuQZY71E4sdAsk";
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
    public static int HighScore = 0;
    public static int CurrentLevel = 0;
    public static float CurrentLevelTime = 0f;
    public static int CurrentLevelPoints = 0;
    public static int CurrentLevelCoins= 0;
    public static int CurrentLevelEnemiesDefeated = 0;

}
