using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointVariables : MonoBehaviour
{
    public static bool CheckpointLoaded = false;
    public static int CheckpointId = -1;
    public static int? Data = -1;
    public static int LevelNo = -1;
    public static float PlayerPosX = -1f;
    public static float PlayerPosY = -1f;
    public static int Health = -1;
    public static List<int> DefeatedEnemiesIds = new List<int>();
    public static List<int> CollectedCoinsIds = new List<int>();
    public static int PlaytroughId = -1;
    public static DateTime Date = DateTime.MinValue;
}
