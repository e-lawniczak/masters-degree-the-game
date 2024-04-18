using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static StartGameHandler;

public class GameEngine : MonoBehaviour
{
    private PlayerLogic playerLogic;
    private PlayerController playerController;
    private Transform playerTransform;
    private readonly float globalUpdateInterval = 10f;
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

        if (CheckpointVariables.CheckpointLoaded)
        {
            playerTransform.position = new Vector2(CheckpointVariables.PlayerPosX, CheckpointVariables.PlayerPosY);
            CheckpointVariables.CheckpointLoaded = false;
        }

        if (!RuntimeVariables.GameStarted)
        {
            RuntimeVariables.GameStarted = true;
        }



        _currentLevelPoints = RuntimeVariables.CurrentLevelPoints;
        _currentLevelCoins = RuntimeVariables.CurrentLevelCoins;
        _currentLevelEnemiesDefeated = RuntimeVariables.CurrentLevelEnemiesDefeated;
        SaveCheckpoint(player.transform.position);
    }

    void Update()
    {
        if (RuntimeVariables.PlayerId == -1 || RuntimeVariables.PlayerJwtToken == "") return;

        AddTime();
        CheckIfPointsChanged();
        UpdateInterval();
        CheckIfPlayerAlive();


    }

    private void UpdateInterval()
    {
        counter += Time.deltaTime;
        if (counter > globalUpdateInterval && playerLogic.IsAlive)
        {
            StartCoroutine(UpdatePlaytrough());
            counter = 0;
        }
    }

    private void CheckIfPointsChanged()
    {
        //killing enemies and collecting coins increases points
        if (RuntimeVariables.CurrentLevelPoints != _currentLevelPoints && playerLogic.IsAlive)
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
    private float playerDeadTimer = 1f;
    private float playerDeadCounter = 0f;

    void CheckIfPlayerAlive()
    {
        if (!playerLogic.IsAlive)
        {
            playerDeadCounter += Time.deltaTime;
            if (playerDeadCounter > playerDeadTimer)
            {
                if (RuntimeVariables.IsControlGroup && CheckpointVariables.CheckpointId > 0)
                {
                    LoadFromCheckpoint();
                }
                else
                {
                    PlaytroughVariables.IsFinished = true;
                    PlaytroughVariables.EndTime = DateTime.UtcNow;
                    StartCoroutine(UpdatePlaytrough());
                    EndGame();
                }
            }
        }
    }
    public void OnEndRunClick()
    {
        PlaytroughVariables.IsFinished = true;
        PlaytroughVariables.EndTime = DateTime.UtcNow;

        StartCoroutine(UpdatePlaytrough());
        EndGame();
    }

    //79a9da

    public void LoadFromCheckpoint()
    {
        var level = CheckpointVariables.LevelNo;
        PlaytroughVariables.PlaytroughId = CheckpointVariables.pvi.PlaytroughId;
        //PlaytroughVariables.TotalTime = CheckpointVariables.pvi.TotalTime;
        PlaytroughVariables.TotalPoints = CheckpointVariables.pvi.TotalPoints;
        PlaytroughVariables.CoinsCollected = CheckpointVariables.pvi.CoinsCollected;
        PlaytroughVariables.EnemiesDefeated = CheckpointVariables.pvi.EnemiesDefeated;
        PlaytroughVariables.PercentageProgress = CheckpointVariables.pvi.PercentageProgress;
        //PlaytroughVariables.Deaths = CheckpointVariables.pvi.Deaths;
        //PlaytroughVariables.TotalEnemyProxTime = CheckpointVariables.pvi.TotalEnemyProxTime;
        //PlaytroughVariables.StandingStillTime = CheckpointVariables.pvi.StandingStillTime;
        PlaytroughVariables.Score = CheckpointVariables.pvi.Score;
        PlaytroughVariables.IsFinished = CheckpointVariables.pvi.IsFinished;
        if (level > 1)
        {
            PlaytroughVariables.LevelTime_1 = CheckpointVariables.pvi.LevelTime_1;
            PlaytroughVariables.LevelPoints_1 = CheckpointVariables.pvi.LevelPoints_1;
            PlaytroughVariables.LevelEnemies_1 = CheckpointVariables.pvi.LevelEnemies_1;
            PlaytroughVariables.LevelCoins_1 = CheckpointVariables.pvi.LevelCoins_1;
            PlaytroughVariables.LevelDeaths_1 = CheckpointVariables.pvi.LevelDeaths_1;
            PlaytroughVariables.LevelEndHp_1 = CheckpointVariables.pvi.LevelEndHp_1;
        }
        if (level > 2)
        {
            PlaytroughVariables.LevelTime_1 = CheckpointVariables.pvi.LevelTime_1;
            PlaytroughVariables.LevelPoints_1 = CheckpointVariables.pvi.LevelPoints_1;
            PlaytroughVariables.LevelEnemies_1 = CheckpointVariables.pvi.LevelEnemies_1;
            PlaytroughVariables.LevelCoins_1 = CheckpointVariables.pvi.LevelCoins_1;
            PlaytroughVariables.LevelDeaths_1 = CheckpointVariables.pvi.LevelDeaths_1;
            PlaytroughVariables.LevelEndHp_1 = CheckpointVariables.pvi.LevelEndHp_1;

            PlaytroughVariables.LevelTime_2 = CheckpointVariables.pvi.LevelTime_2;
            PlaytroughVariables.LevelPoints_2 = CheckpointVariables.pvi.LevelPoints_2;
            PlaytroughVariables.LevelEnemies_2 = CheckpointVariables.pvi.LevelEnemies_2;
            PlaytroughVariables.LevelCoins_2 = CheckpointVariables.pvi.LevelCoins_2;
            PlaytroughVariables.LevelDeaths_2 = CheckpointVariables.pvi.LevelDeaths_2;
            PlaytroughVariables.LevelEndHp_2 = CheckpointVariables.pvi.LevelEndHp_2;
        }
        if (level > 3)
        {
            PlaytroughVariables.LevelTime_1 = CheckpointVariables.pvi.LevelTime_1;
            PlaytroughVariables.LevelPoints_1 = CheckpointVariables.pvi.LevelPoints_1;
            PlaytroughVariables.LevelEnemies_1 = CheckpointVariables.pvi.LevelEnemies_1;
            PlaytroughVariables.LevelCoins_1 = CheckpointVariables.pvi.LevelCoins_1;
            PlaytroughVariables.LevelDeaths_1 = CheckpointVariables.pvi.LevelDeaths_1;
            PlaytroughVariables.LevelEndHp_1 = CheckpointVariables.pvi.LevelEndHp_1;

            PlaytroughVariables.LevelTime_2 = CheckpointVariables.pvi.LevelTime_2;
            PlaytroughVariables.LevelPoints_2 = CheckpointVariables.pvi.LevelPoints_2;
            PlaytroughVariables.LevelEnemies_2 = CheckpointVariables.pvi.LevelEnemies_2;
            PlaytroughVariables.LevelCoins_2 = CheckpointVariables.pvi.LevelCoins_2;
            PlaytroughVariables.LevelDeaths_2 = CheckpointVariables.pvi.LevelDeaths_2;
            PlaytroughVariables.LevelEndHp_2 = CheckpointVariables.pvi.LevelEndHp_2;

            PlaytroughVariables.LevelTime_3 = CheckpointVariables.pvi.LevelTime_3;
            PlaytroughVariables.LevelPoints_3 = CheckpointVariables.pvi.LevelPoints_3;
            PlaytroughVariables.LevelEnemies_3 = CheckpointVariables.pvi.LevelEnemies_3;
            PlaytroughVariables.LevelCoins_3 = CheckpointVariables.pvi.LevelCoins_3;
            PlaytroughVariables.LevelDeaths_3 = CheckpointVariables.pvi.LevelDeaths_3;
            PlaytroughVariables.LevelEndHp_3 = CheckpointVariables.pvi.LevelEndHp_3;
        }
        PlaytroughVariables.UserId = CheckpointVariables.pvi.UserId;
        PlaytroughVariables.StartTime = CheckpointVariables.pvi.StartTime;

        RuntimeVariables.defeatedEnemies = CheckpointVariables.DefeatedEnemiesIds.Select(o => o).ToList();
        RuntimeVariables.collectedCoins = CheckpointVariables.CollectedCoinsIds.Select(o => o).ToList();
        RuntimeVariables.CurrentLevelTime = CheckpointVariables.CurrentLevelTime;
        RuntimeVariables.CurrentLevelPoints = CheckpointVariables.CurrentLevelPoints;
        RuntimeVariables.CurrentLevelCoins = CheckpointVariables.CurrentLevelCoins;
        RuntimeVariables.CurrentLevelEnemiesDefeated = CheckpointVariables.CurrentLevelEnemiesDefeated;


        playerLogic.SetHp(CheckpointVariables.Health);
        CheckpointVariables.CheckpointLoaded = true;
        string sceneToLoad = HelperFunctions.SceneToLoad(CheckpointVariables.LevelNo);
        SceneManager.LoadScene(sceneToLoad);
        Time.timeScale = 1.0f;
    }

    public void SaveCheckpoint(Vector3 checkpointTransform)
    {
        if(playerLogic == null)
        {
            playerLogic = GameObject.Find("Player").GetComponent<PlayerLogic>();
        }
        CheckpointVariables.CheckpointLoaded = false;
        CheckpointVariables.CheckpointId = 1;
        CheckpointVariables.Data = null;
        CheckpointVariables.LevelNo = RuntimeVariables.CurrentLevel;
        if (playerTransform != null)
        {
            CheckpointVariables.PlayerPosX = playerTransform.position.x;
            CheckpointVariables.PlayerPosY = playerTransform.position.y;
        }
        else
        {
            CheckpointVariables.PlayerPosX = checkpointTransform.x;
            CheckpointVariables.PlayerPosY = checkpointTransform.y;
        }
        CheckpointVariables.Health = playerLogic.GetCurrentHp();

        CheckpointVariables.DefeatedEnemiesIds = RuntimeVariables.defeatedEnemies.Select(o => o).ToList();
        CheckpointVariables.CollectedCoinsIds = RuntimeVariables.collectedCoins.Select(o => o).ToList();
        CheckpointVariables.CurrentLevelTime = RuntimeVariables.CurrentLevelTime;
        CheckpointVariables.CurrentLevelPoints = RuntimeVariables.CurrentLevelPoints;
        CheckpointVariables.CurrentLevelCoins = RuntimeVariables.CurrentLevelCoins;
        CheckpointVariables.CurrentLevelEnemiesDefeated = RuntimeVariables.CurrentLevelEnemiesDefeated;

        CheckpointVariables.PlaytroughId = PlaytroughVariables.PlaytroughId;
        CheckpointVariables.Date = DateTime.Now;
        CheckpointVariables.Points = PlaytroughVariables.TotalPoints;
        CheckpointVariables.pvi = new PlaytroughVariablesInstance
        {
            PlaytroughLoaded = PlaytroughVariables.PlaytroughLoaded,
            PlaytroughId = PlaytroughVariables.PlaytroughId,
            TotalTime = PlaytroughVariables.TotalTime,
            TotalPoints = PlaytroughVariables.TotalPoints,
            CoinsCollected = PlaytroughVariables.CoinsCollected,
            EnemiesDefeated = PlaytroughVariables.EnemiesDefeated,
            PercentageProgress = PlaytroughVariables.PercentageProgress,
            Deaths = PlaytroughVariables.Deaths,
            TotalEnemyProxTime = PlaytroughVariables.TotalEnemyProxTime,
            StandingStillTime = PlaytroughVariables.StandingStillTime,
            Score = PlaytroughVariables.Score,
            IsFinished = PlaytroughVariables.IsFinished,
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
            UserId = PlaytroughVariables.UserId,
            StartTime = PlaytroughVariables.StartTime,
            EndTime = PlaytroughVariables.EndTime,
            LastUpdate = PlaytroughVariables.LastUpdate,
            DefeatedEnemiesIds = PlaytroughVariables.DefeatedEnemiesIds,
            CollectedCoinsIds = PlaytroughVariables.CollectedCoinsIds,
        }
        ;

    }



    IEnumerator UpdatePlaytrough()
    {
        StartGameHandler.PlaytroughData obj = PreparePlaytroughUpdate();
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
            if (levelFinished)
            {
                RuntimeVariables.CurrentLevel = RuntimeVariables.CurrentLevel + 1;
                RuntimeVariables.CurrentLevelTime = 0f;
                RuntimeVariables.CurrentLevelPoints = 0;
                RuntimeVariables.CurrentLevelCoins = 0;
                RuntimeVariables.CurrentLevelDeaths = 0;
                RuntimeVariables.CurrentLevelEnemiesDefeated = 0;

                if (RuntimeVariables.CurrentLevel - 1 == 3)
                {
                    EndGame();
                }
                else
                {
                    SceneManager.LoadScene(HelperFunctions.SceneToLoad(RuntimeVariables.CurrentLevel));

                }
            }

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
            PercentageProgress = HelperFunctions.GetPercentCompletion(),
            Deaths = PlaytroughVariables.Deaths,
            TotalEnemyProxTime = PlaytroughVariables.TotalEnemyProxTime,
            StandingStillTime = PlaytroughVariables.StandingStillTime,
            Score = HelperFunctions.CalculateFinalScore(PlaytroughVariables.TotalPoints, PlaytroughVariables.TotalTime),
            IsFinished = PlaytroughVariables.IsFinished,
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
            StartTime = PlaytroughVariables.StartTime.Ticks,
            EndTime = PlaytroughVariables.EndTime.HasValue ? PlaytroughVariables.EndTime.Value.Ticks : 0,
            LastUpdate = DateTime.UtcNow.Ticks,
            DefeatedEnemiesIds = string.Join(',', RuntimeVariables.defeatedEnemies),
            CollectedCoinsIds = string.Join(',', RuntimeVariables.collectedCoins),
        };


        switch (RuntimeVariables.CurrentLevel)
        {
            case 1:
                pd.LevelTime_1 = RuntimeVariables.CurrentLevelTime;
                pd.LevelPoints_1 = RuntimeVariables.CurrentLevelPoints;
                pd.LevelCoins_1 = RuntimeVariables.CurrentLevelCoins;
                pd.LevelEnemies_1 = RuntimeVariables.CurrentLevelEnemiesDefeated;
                pd.LevelDeaths_1 = playerLogic.IsAlive ? PlaytroughVariables.LevelDeaths_1 : PlaytroughVariables.LevelDeaths_1 + 1;
                pd.LevelEndHp_1 = levelFinished ? playerLogic.GetCurrentHp() : PlaytroughVariables.LevelEndHp_1;
                pd.LevelFinished_1 = levelFinished;
                break;
            case 2:
                pd.LevelTime_2 = RuntimeVariables.CurrentLevelTime;
                pd.LevelPoints_2 = RuntimeVariables.CurrentLevelPoints;
                pd.LevelCoins_2 = RuntimeVariables.CurrentLevelCoins;
                pd.LevelEnemies_2 = RuntimeVariables.CurrentLevelEnemiesDefeated;
                pd.LevelDeaths_2 = playerLogic.IsAlive ? PlaytroughVariables.LevelDeaths_2 : PlaytroughVariables.LevelDeaths_2 + 1;
                pd.LevelEndHp_2 = levelFinished ? playerLogic.GetCurrentHp() : PlaytroughVariables.LevelEndHp_2;
                pd.LevelFinished_2 = levelFinished;
                break;
            case 3:
                pd.LevelTime_3 = RuntimeVariables.CurrentLevelTime;
                pd.LevelPoints_3 = RuntimeVariables.CurrentLevelPoints;
                pd.LevelCoins_3 = RuntimeVariables.CurrentLevelCoins;
                pd.LevelEnemies_3 = RuntimeVariables.CurrentLevelEnemiesDefeated;
                pd.LevelDeaths_3 = playerLogic.IsAlive ? PlaytroughVariables.LevelDeaths_3 : PlaytroughVariables.LevelDeaths_3 + 1;
                pd.LevelEndHp_3 = levelFinished ? playerLogic.GetCurrentHp() : PlaytroughVariables.LevelEndHp_3;
                pd.LevelFinished_3 = levelFinished;
                break;
            default:
                break;

        }

        return pd;
    }

    #region Start and End 
    private void EndGame()
    {
        SceneManager.LoadScene(SceneNames.EndScreen);
    }
    public void MuteSound()
    {
        GameObject.Find("SoundHandler").GetComponent<SoundHandler>().MuteUnmute();
    }

    internal void LevelComplete()
    {
        levelFinished = true;
        if (RuntimeVariables.CurrentLevel == 1)
        {
            PlaytroughVariables.LevelFinished_1 = true;
            PlaytroughVariables.LevelTime_1 = RuntimeVariables.CurrentLevelTime;
            PlaytroughVariables.LevelPoints_1 = RuntimeVariables.CurrentLevelPoints;
            PlaytroughVariables.LevelEnemies_1 = RuntimeVariables.CurrentLevelEnemiesDefeated;
            PlaytroughVariables.LevelCoins_1 = RuntimeVariables.CurrentLevelCoins;
            PlaytroughVariables.LevelDeaths_1 = RuntimeVariables.CurrentLevelDeaths;
            PlaytroughVariables.LevelEndHp_1 = playerLogic.GetCurrentHp();
        }
        else if (RuntimeVariables.CurrentLevel == 2)
        {
            PlaytroughVariables.LevelFinished_2 = true;
            PlaytroughVariables.LevelTime_2 = RuntimeVariables.CurrentLevelTime;
            PlaytroughVariables.LevelPoints_2 = RuntimeVariables.CurrentLevelPoints;
            PlaytroughVariables.LevelEnemies_2 = RuntimeVariables.CurrentLevelEnemiesDefeated;
            PlaytroughVariables.LevelCoins_2 = RuntimeVariables.CurrentLevelCoins;
            PlaytroughVariables.LevelDeaths_2 = RuntimeVariables.CurrentLevelDeaths;
            PlaytroughVariables.LevelEndHp_2 = playerLogic.GetCurrentHp();
        }
        else
        {
            PlaytroughVariables.LevelFinished_3 = true;
            PlaytroughVariables.LevelTime_3 = RuntimeVariables.CurrentLevelTime;
            PlaytroughVariables.LevelPoints_3 = RuntimeVariables.CurrentLevelPoints;
            PlaytroughVariables.LevelEnemies_3 = RuntimeVariables.CurrentLevelEnemiesDefeated;
            PlaytroughVariables.LevelCoins_3 = RuntimeVariables.CurrentLevelCoins;
            PlaytroughVariables.LevelDeaths_3 = RuntimeVariables.CurrentLevelDeaths;
            PlaytroughVariables.LevelEndHp_3 = playerLogic.GetCurrentHp();
        }
        if (RuntimeVariables.CurrentLevel == 3)
        {
            RuntimeVariables.GameWon = true;
            PlaytroughVariables.IsFinished = true;
            PlaytroughVariables.EndTime = DateTime.UtcNow;
        }
        RuntimeVariables.CurrentHp = playerLogic.GetCurrentHp();
        StartCoroutine(UpdatePlaytrough());
    }


    #endregion


}
