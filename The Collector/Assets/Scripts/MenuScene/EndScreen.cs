using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private TextMeshProUGUI leaderBoards;
    private string Boards;
    // Start is called before the first frame update
    void Start()
    {
        if (RuntimeVariables.GameWon)
        {
            title.text = "Congratulations!!!\n You won!";
        }
        var finalScore = HelperFunctions.CalculateFinalScore(PlaytroughVariables.TotalPoints, PlaytroughVariables.TotalTime);
        score.text = string.Format("Your score: {0}", finalScore);
        if(finalScore > RuntimeVariables.HighScore)
        {
            RuntimeVariables.HighScore = finalScore;
        }
        highScore.text = string.Format("High score: {0}", RuntimeVariables.HighScore);
        StartCoroutine(GetLeaderboards());

    }

    public void MoveToMenu()
    {
        SceneManager.LoadSceneAsync(SceneNames.Stats);
    }

    private IEnumerator GetLeaderboards()
    {

        UnityWebRequest req = UnityWebRequest.Get(RuntimeVariables.apiUrl + "/api/leaderboards/get");
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
            var lb = JsonUtility.FromJson<LBRes>(req.downloadHandler.text);
            new List<Leaderboards>(lb.leaderboards).ForEach(s =>
            {
                Boards += string.Format("{0}: {1}\n", s.userName, s.highScore);
            });
            leaderBoards.text = string.Format("<b>Leaderboards</b>\n\n{0}", Boards);

        }
    }
    
    public class LBRes
    {
        public Leaderboards[] leaderboards;
    }

    [Serializable]
    public class Leaderboards
    {
        public string userName;
        public int highScore;

    }
}
