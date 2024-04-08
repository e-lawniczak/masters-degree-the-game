using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private TextMeshProUGUI leaderBoards;
    // Start is called before the first frame update
    void Start()
    {
        if (RuntimeVariables.GameWon)
        {
            title.text = "Congratulations!!!\n You won!";
        }
        score.text = string.Format("Your score: {0}", ((PlaytroughVariables.TotalPoints * 1000f) / (PlaytroughVariables.TotalTime)));
        highScore.text = string.Format("High score: {0}", RuntimeVariables.HighScore);
        var leaderboardsArray = GetLeaderboards();
        leaderBoards.text = string.Format("Leaderboards\n{0}", leaderboardsArray);
    }

    public void MoveToMenu()
    {
        SceneManager.LoadSceneAsync(SceneNames.Stats);
    }

    private string GetLeaderboards()
    {
        string str = string.Empty;
        for (int i = 0; i < 10; i++)
        {
            str += string.Format("{0}: {1}\n", "UserName", "1000");
        }
        return str;
    }
}
