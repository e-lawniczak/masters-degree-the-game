using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using static StartGameHandler;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class StartGameHandler : MonoBehaviour
{
    public GameObject toHide;
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
        StartCoroutine(StartGameAsync());
    }
    public IEnumerator StartGameAsync()
    {
        if (toHide != null)
            toHide.SetActive(false);
        yield return StartCoroutine(FetchInitialPlaytroughData());
        if (_StartGameData == null)
        {
            yield return null;
        }
        else
        {
            StartFromBegining();
        }
    }


    //public void StartFromCheckpoint()
    //{
    //    Debug.Log("Checkpoint start");
    //    //AssignRuntimeVariables();
    //    string sceneToLoad = HelperFunctions.SceneToLoad(CheckpointVariables.LevelNo);
    //    SceneManager.LoadSceneAsync(sceneToLoad);
    //}
    private void StartFromBegining()
    {
        RuntimeVariables.CurrentLevel = 1;
        HelperFunctions.ResetPlaytrough();
        SceneManager.LoadSceneAsync(HelperFunctions.SceneToLoad(RuntimeVariables.CurrentLevel));
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
            RuntimeVariables.IsControlGroup = _StartGameData.isControlGroup;
            PlaytroughVariables.PlaytroughId = _StartGameData.currentPlaytrough.HasValue ? _StartGameData.currentPlaytrough.Value : -1;
            RuntimeVariables.HighScore = _StartGameData.highScore;

        }
    }
 
    //IEnumerator FetchPlaytrough()
    //{
    //    UnityWebRequest req = UnityWebRequest.Get(RuntimeVariables.apiUrl + "/api/playtrough/getPlaytrough/" + _StartGameData.currentPlaytrough.ToString());
    //    req.useHttpContinue = false;
    //    req.SetRequestHeader("Authorization", "Bearer " + RuntimeVariables.PlayerJwtToken);
    //    yield return req.SendWebRequest();


    //    if (req.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.LogError(req.downloadHandler.text);
    //        Debug.LogError(req.error);
    //    }
    //    else
    //    {
    //        _PlaytroughData = JsonUtility.FromJson<PlaytroughData>(req.downloadHandler.text);

    //    }
    //}
    //IEnumerator FetchCheckpoint()
    //{
    //    UnityWebRequest req = UnityWebRequest.Get(RuntimeVariables.apiUrl + "/api/playtrough/getCheckpoint/" + _StartGameData.currentPlaytrough.ToString());
    //    req.useHttpContinue = false;
    //    req.SetRequestHeader("Authorization", "Bearer " + RuntimeVariables.PlayerJwtToken);
    //    yield return req.SendWebRequest();


    //    if (req.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.LogError(req.downloadHandler.text);
    //        Debug.LogError(req.error);
    //    }
    //    else
    //    {
    //        _CheckpointData = JsonUtility.FromJson<CheckpointData>(req.downloadHandler.text);

    //    }
    //}

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
        public float TotalTime;
        public int TotalPoints;
        public int CoinsCollected;
        public int EnemiesDefeated;
        public float PercentageProgress;
        public int Deaths;
        public float TotalEnemyProxTime;
        public float StandingStillTime;
        public int Score;
        public bool IsFinished;
        public float LevelTime_1;
        public int LevelPoints_1;
        public int LevelEnemies_1;
        public int LevelCoins_1;
        public int LevelDeaths_1;
        public int LevelEndHp_1;
        public bool LevelFinished_1;
        public float LevelTime_2;
        public int LevelPoints_2;
        public int LevelEnemies_2;
        public int LevelCoins_2;
        public int LevelDeaths_2;
        public int LevelEndHp_2;
        public bool LevelFinished_2;
        public float LevelTime_3;
        public int LevelPoints_3;
        public int LevelEnemies_3;
        public int LevelCoins_3;
        public int LevelDeaths_3;
        public int LevelEndHp_3;
        public bool LevelFinished_3;
        public int UserId;
        public long StartTime;
        public long EndTime;
        public long LastUpdate;
    }
    [Serializable]
    public class CheckpointData
    {
        public int CheckpointId;
        public string Data;
        public int LevelNo;
        public float PlayerPosX;
        public float PlayerPosY;
        public int Health;
        public int[] DefeatedEnemiesIds;
        public int[] CollectedCoinsIds;
        public int PlaytroughId;
        public DateTime Date;
        public int Points;
    }
    [Serializable]
    public class CheckpointJsonData
    {
        public int SavedLevelEnemies;
        public int SavedLevelCoins;
    }
}
