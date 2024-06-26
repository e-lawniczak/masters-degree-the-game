using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static StartGameHandler;

public class HelperFunctions : MonoBehaviour
{
    public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }
    public class DashInfo
    {
        public float dashCd { get; set; }
        public float currentCd { get; set; }
        public bool canDash { get; set; }
    }
    public static int CalculateFinalScore(int totalPoints, float totalTime)
    {
        //int score = totalPoints * 1000;
        //score = (int)(score / (totalTime + 1f));

        int score = (int)Math.Max(ScoreFuncSimplest(totalPoints, totalTime), 0);

        return score;
    }
   
    private static float ScoreFuncSimplest(int points, float totalTime, float pointsWeight = 0.15f, float timeWeight = 150f)
    {
        var timeInMinutes = totalTime / 60f;
        float p1 = points;
        float p2 = 8000f * (Math.Max(0f, GetPercentCompletionSimple(true)) / 100f) / Math.Max(1f, totalTime);
        float p3 = 8000f * (float)(Math.Max(0f, GetPercentCompletionSimple(true)) / 100f);
        float p4 = RuntimeVariables.GameFinished ? 3500f * (float)(RuntimeVariables.CurrentHp / 7f) : 0;
        //Debug.Log("P1: " + p1);
        //Debug.Log("P2: " + p2);
        //Debug.Log("P3: " + p3);
        //Debug.Log("P4: " + p4);
        return p1 + p2 + p3 + p4;
    }
    public static float GetPercentCompletionSimple(bool getOnlyLevels = false)
    {
        float percentCompletion = 0;

        percentCompletion = (float)((float)RuntimeVariables.CheckpointsVisited.Count / (float)RuntimeVariables.TotalCheckpoints) * 100f;
        return percentCompletion;
    }
    public static float GetPercentCompletion(bool getOnlyLevels = false)
    {
        float percentCompletion = 0;

        percentCompletion += PlaytroughVariables.LevelFinished_1 ? 25 : 0;
        percentCompletion += PlaytroughVariables.LevelFinished_2 ? 25 : 0;
        percentCompletion += PlaytroughVariables.LevelFinished_3 ? 25 : 0;

        if (getOnlyLevels)
            return (float)(percentCompletion / 75f) * 100f;

        percentCompletion += (PlaytroughVariables.CoinsCollected);
        percentCompletion += (PlaytroughVariables.EnemiesDefeated);

        percentCompletion = (float)(percentCompletion / ((25 * 3) + RuntimeVariables.TotalCoinsInGame + RuntimeVariables.TotalEnemiesInGame)) * 100f;


        return percentCompletion;
    }

    public static string SceneToLoad(int level)
    {
        var sceneToLoad = SceneNames.Test;
        switch (level)
        {

            case 1:
                sceneToLoad = SceneNames.Level1;
                break;
            case 2:
                sceneToLoad = SceneNames.Level2;
                break;
            case 3:
                sceneToLoad = SceneNames.Level3;
                break;
            default:
                sceneToLoad = SceneNames.Test;
                break;

        }
        return sceneToLoad;
    }


    internal static void ResetPlaytrough()
    {
        RuntimeVariables.TotalPoints = 0;
        RuntimeVariables.TotalTime = 0;
        RuntimeVariables.defeatedEnemies.Clear();
        RuntimeVariables.collectedCoins.Clear();
        RuntimeVariables.GameWon = false;
        RuntimeVariables.GameStarted = false;
        //RuntimeVariables.HighScore = 0;
        RuntimeVariables.CurrentLevel = 1;
        RuntimeVariables.CurrentLevelTime = 0;
        RuntimeVariables.CurrentLevelPoints = 0;
        RuntimeVariables.CurrentLevelCoins = 0;
        RuntimeVariables.CurrentLevelEnemiesDefeated = 0;
        RuntimeVariables.CurrentHp = -1;


        PlaytroughVariables.PlaytroughLoaded = false;
        PlaytroughVariables.PlaytroughId = -1;
        PlaytroughVariables.TotalTime = 0f;
        PlaytroughVariables.TotalPoints = 0;
        PlaytroughVariables.CoinsCollected = 0;
        PlaytroughVariables.EnemiesDefeated = 0;
        PlaytroughVariables.PercentageProgress = 0;
        PlaytroughVariables.Deaths = 0;
        PlaytroughVariables.TotalEnemyProxTime = 0f;
        PlaytroughVariables.StandingStillTime = 0f;
        PlaytroughVariables.Score = 0;
        PlaytroughVariables.IsFinished = false;
        PlaytroughVariables.LevelTime_1 = 0f;
        PlaytroughVariables.LevelPoints_1 = 0;
        PlaytroughVariables.LevelEnemies_1 = 0;
        PlaytroughVariables.LevelCoins_1 = 0;
        PlaytroughVariables.LevelDeaths_1 = 0;
        PlaytroughVariables.LevelEndHp_1 = 0;
        PlaytroughVariables.LevelFinished_1 = false;
        PlaytroughVariables.LevelTime_2 = 0f;
        PlaytroughVariables.LevelPoints_2 = 0;
        PlaytroughVariables.LevelEnemies_2 = 0;
        PlaytroughVariables.LevelCoins_2 = 0;
        PlaytroughVariables.LevelDeaths_2 = 0;
        PlaytroughVariables.LevelEndHp_2 = 0;
        PlaytroughVariables.LevelFinished_2 = false;
        PlaytroughVariables.LevelTime_3 = 0f;
        PlaytroughVariables.LevelPoints_3 = 0;
        PlaytroughVariables.LevelEnemies_3 = 0;
        PlaytroughVariables.LevelCoins_3 = 0;
        PlaytroughVariables.LevelDeaths_3 = 0;
        PlaytroughVariables.LevelEndHp_3 = 0;
        PlaytroughVariables.LevelFinished_3 = false;
        PlaytroughVariables.UserId = -1;
        PlaytroughVariables.StartTime = DateTime.UtcNow;
        PlaytroughVariables.EndTime = null;
        PlaytroughVariables.LastUpdate = null;
        PlaytroughVariables.DefeatedEnemiesIds = "";
        PlaytroughVariables.CollectedCoinsIds = "";

    }
}
