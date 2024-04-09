using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeVariables : MonoBehaviour
{
    public static string PlayerJwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJlYmMwM2M5Ny00OWE0LTQ1ZDAtOTdjMi04Njg5NGQ4MDA2YWMiLCJ1bmlxdWVfbmFtZSI6ImRlZSIsInVpZCI6IjMyIiwibmJmIjoxNzEyNjY2NjA2LCJleHAiOjE3MTI2NzM4MDYsImlhdCI6MTcxMjY2NjYwNiwiaXNzIjoiQ2xvdGhJc3N1ZXIiLCJhdWQiOiJDbG90aEF1ZGllbmNlIn0.rckujQQfJPScWmpa_E9vR9YUysAmWrtc2M-IvG_-pq4";
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
