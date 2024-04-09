using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static StartGameHandler;

public class GameEngine : MonoBehaviour
{
    private PlayerLogic playerLogic;
    private PlayerController playerController;
    private Transform playerTransform;
    private float globalUpdateInterval = 60f;
    private float counter = 0f;
    private bool levelFinished = false;

    private int _currentLevelPoints = 0;
    private int _currentLevelCoins = 0;
    private int _currentLevelEnemiesDefeated = 0;


    void Start()
    {
        if (RuntimeVariables.PlayerId == -1 || RuntimeVariables.PlayerJwtToken == "") return;
        var player = GameObject.Find("Player");
        if (player == null)
        {
            return;
        };
        playerLogic = player.GetComponent<PlayerLogic>();
        playerController = player.GetComponent<PlayerController>();
        playerTransform = player.GetComponent<Transform>();
        if (playerController == null || playerLogic == null)
        {
            EndGame();
            return;
        }

        RuntimeVariables.CurrentLevel = 1;

        if (CheckpointVariables.CheckpointLoaded && !RuntimeVariables.GameStarted)
        {
            playerTransform.position = new Vector2(CheckpointVariables.PlayerPosX, CheckpointVariables.PlayerPosY);
            RuntimeVariables.defeatedEnemies = CheckpointVariables.DefeatedEnemiesIds;
            RuntimeVariables.collectedCoins = CheckpointVariables.CollectedCoinsIds;
            RuntimeVariables.CurrentLevel = CheckpointVariables.LevelNo;

        }
        if (PlaytroughVariables.PlaytroughLoaded && !RuntimeVariables.GameStarted)
        {
            RuntimeVariables.CurrentLevelPoints = PlaytroughVariables.TotalPoints;
            RuntimeVariables.CurrentLevelTime = PlaytroughVariables.TotalTime;
            RuntimeVariables.TotalPoints = PlaytroughVariables.TotalPoints;
            RuntimeVariables.TotalTime = PlaytroughVariables.TotalTime;

            RuntimeVariables.CurrentLevelPoints = RuntimeVariables.CurrentLevelPoints - PlaytroughVariables.LevelPoints_1 - PlaytroughVariables.LevelPoints_2;
            RuntimeVariables.CurrentLevelTime = RuntimeVariables.CurrentLevelTime - PlaytroughVariables.LevelTime_1 - PlaytroughVariables.LevelTime_2;
            RuntimeVariables.CurrentLevelCoins = PlaytroughVariables.CoinsCollected - PlaytroughVariables.LevelCoins_1 - PlaytroughVariables.LevelCoins_2;
            RuntimeVariables.CurrentLevelEnemiesDefeated = PlaytroughVariables.EnemiesDefeated - PlaytroughVariables.LevelEnemies_1 - PlaytroughVariables.LevelEnemies_2;
        }
        if (!RuntimeVariables.GameStarted)
        {
            RuntimeVariables.GameStarted = true;
        }

        _currentLevelPoints = RuntimeVariables.CurrentLevelPoints;
        _currentLevelCoins = RuntimeVariables.CurrentLevelCoins;
        _currentLevelEnemiesDefeated = RuntimeVariables.CurrentLevelEnemiesDefeated;

    }

    void Update()
    {
        if (RuntimeVariables.PlayerId == -1 || RuntimeVariables.PlayerJwtToken == "") return;

        CheckIfPlayerAlive();
        AddTime();
        CheckIfPointsChanged();
        //CheckIfEnemiesChanged();
        UpdateInterval();


    }

    private void UpdateInterval()
    {
        counter += Time.deltaTime;
        if (counter > globalUpdateInterval)
        {
            StartCoroutine(UpdatePlaytrough());
            counter = 0;
        }
    }

    //private void CheckIfEnemiesChanged()
    //{
    //    throw new NotImplementedException();
    //}

    private void CheckIfPointsChanged()
    {
        //killing enemies and collecting coins increases points
        if (RuntimeVariables.CurrentLevelPoints != _currentLevelPoints)
        {
            StartCoroutine(UpdatePlaytrough());
            _currentLevelPoints = RuntimeVariables.CurrentLevelPoints;
        }
    }

    void AddTime()
    {
        PlaytroughVariables.TotalTime += Time.deltaTime;
        RuntimeVariables.CurrentLevelTime += Time.deltaTime;
    }

    void CheckIfPlayerAlive()
    {
        if (!playerLogic.IsAlive)
        {
            StartCoroutine(UpdatePlaytrough());
            EndGame();
        }
    }

    IEnumerator UpdatePlaytrough()
    {
        StartGameHandler.PlaytroughData obj = PreparePlaytroughUpdate();
        Debug.Log(obj.EndTime);
        UnityWebRequest req = UnityWebRequest.Post(RuntimeVariables.apiUrl + "/api/playtrough/updatePlaytrough", JsonUtility.ToJson(obj), "application/json");
        req.useHttpContinue = false;
        req.SetRequestHeader("Authorization", "Bearer " + RuntimeVariables.PlayerJwtToken);
        yield return req.SendWebRequest();


        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(req.downloadHandler.text);
            Debug.LogError(req.error);
        }
        else
        {
            PlaytroughVariables.PlaytroughId = Convert.ToInt32(req.downloadHandler.text);
            Debug.Log(req.downloadHandler.text);
        }
    }

    private PlaytroughData PreparePlaytroughUpdate()
    {
        var pd = new PlaytroughData
        {
            PlaytroughId = PlaytroughVariables.PlaytroughId,
            TotalTime = PlaytroughVariables.TotalTime,
            TotalPoints = PlaytroughVariables.TotalPoints,
            CoinsCollected = PlaytroughVariables.CoinsCollected,
            EnemiesDefeated = PlaytroughVariables.EnemiesDefeated,
            PercentageProgress = PlaytroughVariables.PercentageProgress,
            Deaths = playerLogic.IsAlive ? PlaytroughVariables.Deaths : PlaytroughVariables.Deaths + 1,
            TotalEnemyProxTime = PlaytroughVariables.TotalEnemyProxTime,
            StandingStillTime = PlaytroughVariables.StandingStillTime,
            Score = PlaytroughVariables.Score,
            IsFinished = playerLogic.IsAlive ? PlaytroughVariables.IsFinished : true,
            LevelTime_1 = PlaytroughVariables.LevelTime_1,
            LevelPoints_1 = PlaytroughVariables.LevelPoints_1,
            LevelEnemies_1 = PlaytroughVariables.LevelEnemies_1,
            LevelCoins_1 = PlaytroughVariables.LevelCoins_1,
            LevelDeaths_1 = PlaytroughVariables.LevelDeaths_1,
            LevelEndHp_1 = PlaytroughVariables.LevelEndHp_1,
            LevelTime_2 = PlaytroughVariables.LevelTime_2,
            LevelPoints_2 = PlaytroughVariables.LevelPoints_2,
            LevelEnemies_2 = PlaytroughVariables.LevelEnemies_2,
            LevelCoins_2 = PlaytroughVariables.LevelCoins_2,
            LevelDeaths_2 = PlaytroughVariables.LevelDeaths_2,
            LevelEndHp_2 = PlaytroughVariables.LevelEndHp_2,
            LevelTime_3 = PlaytroughVariables.LevelTime_3,
            LevelPoints_3 = PlaytroughVariables.LevelPoints_3,
            LevelEnemies_3 = PlaytroughVariables.LevelEnemies_3,
            LevelCoins_3 = PlaytroughVariables.LevelCoins_3,
            LevelDeaths_3 = PlaytroughVariables.LevelDeaths_3,
            LevelEndHp_3 = PlaytroughVariables.LevelEndHp_3,
            UserId = RuntimeVariables.PlayerId,
            StartTime = PlaytroughVariables.StartTime,
            EndTime = RuntimeVariables.GameWon ? DateTime.UtcNow : !playerLogic.IsAlive ? DateTime.UtcNow : null,
            LastUpdate = DateTime.UtcNow,
        };

        switch (RuntimeVariables.CurrentLevel)
        {
            case 1:
                pd.LevelTime_1 = RuntimeVariables.CurrentLevelTime;
                pd.LevelPoints_1 = RuntimeVariables.CurrentLevelPoints;
                pd.LevelCoins_1 = RuntimeVariables.CurrentLevelCoins;
                pd.LevelEnemies_1 = RuntimeVariables.CurrentLevelEnemiesDefeated;
                pd.LevelDeaths_1 = playerLogic.IsAlive ? PlaytroughVariables.LevelDeaths_1 : PlaytroughVariables.LevelDeaths_1 + 1;
                pd.LevelEndHp_1 = levelFinished ? PlaytroughVariables.LevelEndHp_1 : playerLogic.GetCurrentHp();
                break;
            case 2:
                pd.LevelTime_2 = RuntimeVariables.CurrentLevelTime;
                pd.LevelPoints_2 = RuntimeVariables.CurrentLevelPoints;
                pd.LevelCoins_2 = RuntimeVariables.CurrentLevelCoins;
                pd.LevelEnemies_2 = RuntimeVariables.CurrentLevelEnemiesDefeated;
                pd.LevelDeaths_2 = playerLogic.IsAlive ? PlaytroughVariables.LevelDeaths_2 : PlaytroughVariables.LevelDeaths_2 + 1;
                pd.LevelEndHp_2 = levelFinished ? PlaytroughVariables.LevelEndHp_2 : playerLogic.GetCurrentHp();
                break;
            case 3:
                pd.LevelTime_3 = RuntimeVariables.CurrentLevelTime;
                pd.LevelPoints_3 = RuntimeVariables.CurrentLevelPoints;
                pd.LevelCoins_3 = RuntimeVariables.CurrentLevelCoins;
                pd.LevelEnemies_3 = RuntimeVariables.CurrentLevelEnemiesDefeated;
                pd.LevelDeaths_3 = playerLogic.IsAlive ? PlaytroughVariables.LevelDeaths_3 : PlaytroughVariables.LevelDeaths_3 + 1;
                pd.LevelEndHp_3 = levelFinished ? PlaytroughVariables.LevelEndHp_3 : playerLogic.GetCurrentHp();
                break;
            default:
                break;

        }

        return pd;
    }

    #region Start and End 
    private void EndGame()
    {
        SceneManager.LoadSceneAsync(SceneNames.EndScreen);
    }


    #endregion


}
