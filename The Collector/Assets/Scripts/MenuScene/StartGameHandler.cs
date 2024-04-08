using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StartGameHandler;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class StartGameHandler : MonoBehaviour
{
    private StartGameData _StartGameData = null;
    private PlaytroughData _PlaytroughData = null;
    private CheckpointData _CheckpointData = null;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

    }
    public void StartGame()
    {
        StartCoroutine(FetchInitialPlaytroughData());
        if (_StartGameData == null)
        {
            return;
        }
        if (!_StartGameData.isControlGroup || !_StartGameData.currentPlaytrough.HasValue)
        {
            StartFromBegining();
        }
        else
        {
            StartCoroutine(FetchPlaytrough());
            if (_PlaytroughData == null)
            {
                return;
            }
            StartCoroutine(FetchCheckpoint());
            if (_CheckpointData == null)
            {
                return;
            }
            StartFromCheckpoint();
        }
    }


    private void StartFromCheckpoint()
    {
        Debug.Log("Checkpoint start");
        AssignRuntimeVariables();
        var sceneToLoad = SceneNames.Test;
        switch (_CheckpointData.LevelNo)
        {

            case 1:
                sceneToLoad = SceneNames.Level1;
                break;
            case 2:
                sceneToLoad = SceneNames.Level1;
                break;
            case 3:
                sceneToLoad = SceneNames.Level1;
                break;
            default:
                sceneToLoad = SceneNames.Test;
                break;

        }

        SceneManager.LoadSceneAsync(sceneToLoad);
    }
    private void StartFromBegining()
    {
        Debug.Log("No save start");
        SceneManager.LoadSceneAsync(SceneNames.Test);
    }
    IEnumerator FetchInitialPlaytroughData()
    {
        UnityWebRequest req = UnityWebRequest.Get(RuntimeVariables.apiUrl + "/api/playtrough/getInitial/" + RuntimeVariables.PlayerId.ToString());
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
            _StartGameData = JsonUtility.FromJson<StartGameData>(req.downloadHandler.text);
        }
    }
    IEnumerator FetchPlaytrough()
    {
        UnityWebRequest req = UnityWebRequest.Get(RuntimeVariables.apiUrl + "/api/playtrough/getPlaytrough/" + _StartGameData.currentPlaytrough.ToString());
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
            _PlaytroughData = JsonUtility.FromJson<PlaytroughData>(req.downloadHandler.text);
        }
    }
    IEnumerator FetchCheckpoint()
    {
        UnityWebRequest req = UnityWebRequest.Get(RuntimeVariables.apiUrl + "/api/playtrough/getCheckpoint/" + _StartGameData.currentPlaytrough.ToString());
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
            _CheckpointData = JsonUtility.FromJson<CheckpointData>(req.downloadHandler.text);
        }
    }
    private void AssignRuntimeVariables()
    {
        RuntimeVariables.collectedCoins = _CheckpointData.CollectedCoinsIds;
        RuntimeVariables.defeatedEnemies = _CheckpointData.DefeatedEnemiesIds;

        CheckpointVariables.CheckpointLoaded = true;
        CheckpointVariables.CheckpointId = _CheckpointData.CheckpointId;
        CheckpointVariables.Data = _CheckpointData.Data;
        CheckpointVariables.LevelNo = _CheckpointData.LevelNo;
        CheckpointVariables.PlayerPosX = _CheckpointData.PlayerPosX;
        CheckpointVariables.PlayerPosY = _CheckpointData.PlayerPosY;
        CheckpointVariables.Health = _CheckpointData.Health;
        CheckpointVariables.DefeatedEnemiesIds = _CheckpointData.DefeatedEnemiesIds;
        CheckpointVariables.CollectedCoinsIds = _CheckpointData.CollectedCoinsIds;
        CheckpointVariables.PlaytroughId = _CheckpointData.PlaytroughId;
        CheckpointVariables.Date = _CheckpointData.Date;

        PlaytroughVariables.PlaytroughLoaded = true;
        PlaytroughVariables.PlaytroughId = _PlaytroughData.PlaytroughId;
        PlaytroughVariables.TotalTime = _PlaytroughData.TotalTime;
        PlaytroughVariables.TotalPoints = _PlaytroughData.TotalPoints;
        PlaytroughVariables.CoinsCollected = _PlaytroughData.CoinsCollected;
        PlaytroughVariables.EnemiesDefeated = _PlaytroughData.EnemiesDefeated;
        PlaytroughVariables.PercentageProgress = _PlaytroughData.PercentageProgress;
        PlaytroughVariables.Deaths = _PlaytroughData.Deaths;
        PlaytroughVariables.TotalEnemyProxTime = _PlaytroughData.TotalEnemyProxTime;
        PlaytroughVariables.StandingStillTime = _PlaytroughData.StandingStillTime;
        PlaytroughVariables.Score = _PlaytroughData.Score;
        PlaytroughVariables.IsFinished = _PlaytroughData.IsFinished;
        PlaytroughVariables.LevelTime_1 = _PlaytroughData.LevelTime_1;
        PlaytroughVariables.LevelPoints_1 = _PlaytroughData.LevelPoints_1;
        PlaytroughVariables.LevelEnemies_1 = _PlaytroughData.LevelEnemies_1;
        PlaytroughVariables.LevelCoins_1 = _PlaytroughData.LevelCoins_1;
        PlaytroughVariables.LevelDeaths_1 = _PlaytroughData.LevelDeaths_1;
        PlaytroughVariables.LevelEndHp_1 = _PlaytroughData.LevelEndHp_1;
        PlaytroughVariables.LevelTime_2 = _PlaytroughData.LevelTime_2;
        PlaytroughVariables.LevelPoints_2 = _PlaytroughData.LevelPoints_2;
        PlaytroughVariables.LevelEnemies_2 = _PlaytroughData.LevelEnemies_2;
        PlaytroughVariables.LevelCoins_2 = _PlaytroughData.LevelCoins_2;
        PlaytroughVariables.LevelDeaths_2 = _PlaytroughData.LevelDeaths_2;
        PlaytroughVariables.LevelEndHp_2 = _PlaytroughData.LevelEndHp_2;
        PlaytroughVariables.LevelTime_3 = _PlaytroughData.LevelTime_3;
        PlaytroughVariables.LevelPoints_3 = _PlaytroughData.LevelPoints_3;
        PlaytroughVariables.LevelEnemies_3 = _PlaytroughData.LevelEnemies_3;
        PlaytroughVariables.LevelCoins_3 = _PlaytroughData.LevelCoins_3;
        PlaytroughVariables.LevelDeaths_3 = _PlaytroughData.LevelDeaths_3;
        PlaytroughVariables.LevelEndHp_3 = _PlaytroughData.LevelEndHp_3;
        PlaytroughVariables.UserId = _PlaytroughData.UserId;
        PlaytroughVariables.StartTime = _PlaytroughData.StartTime;
        PlaytroughVariables.EndTime = _PlaytroughData.EndTime;
        PlaytroughVariables.LastUpdate = _PlaytroughData.LastUpdate;
    }
    [Serializable]
    public class StartGameData
    {
        public int? currentPlaytrough;
        public int highScore;
        public bool isControlGroup;
    }
    [Serializable]
    public class PlaytroughData
    {
        public int PlaytroughId;
        public float? TotalTime;
        public int? TotalPoints;
        public int? CoinsCollected;
        public int? EnemiesDefeated;
        public int? PercentageProgress;
        public int? Deaths;
        public float TotalEnemyProxTime;
        public float StandingStillTime;
        public int Score;
        public bool IsFinished;
        public float? LevelTime_1;
        public int? LevelPoints_1;
        public int? LevelEnemies_1;
        public int? LevelCoins_1;
        public int? LevelDeaths_1;
        public int? LevelEndHp_1;
        public float? LevelTime_2;
        public int? LevelPoints_2;
        public int? LevelEnemies_2;
        public int? LevelCoins_2;
        public int? LevelDeaths_2;
        public int? LevelEndHp_2;
        public float? LevelTime_3;
        public int? LevelPoints_3;
        public int? LevelEnemies_3;
        public int? LevelCoins_3;
        public int? LevelDeaths_3;
        public int? LevelEndHp_3;
        public int UserId;
        public DateTime StartTime;
        public DateTime? EndTime;
        public DateTime? LastUpdate;
    }
    [Serializable]
    public class CheckpointData
    {
        public int CheckpointId;
        public int? Data;
        public int LevelNo;
        public float PlayerPosX;
        public float PlayerPosY;
        public int Health;
        public List<int> DefeatedEnemiesIds;
        public List<int> CollectedCoinsIds;
        public int PlaytroughId;
        public DateTime Date;
    }
}
