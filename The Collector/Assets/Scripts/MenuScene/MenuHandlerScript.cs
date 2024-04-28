using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq;

public class MenuHandlerScript : MonoBehaviour
{
    public TextMeshProUGUI errorMsg;
    [SerializeField] private TMP_InputField UserName;
    [SerializeField] private TMP_InputField Password;
    [SerializeField] private TMP_InputField Email;
    [SerializeField] private GameEngine gameEngine;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private GameObject buttonsToHide;

    private Color errorColor = new Color(1f, (float)(121f / 255f), (float)(121f / 255f));
    private string defaultColor = "FFFFFF";

    private string salt = "$2a$12$LdSGL/4rQGQYLbXbJH3ks.";


    private LoginResponse Data = new LoginResponse();
    private StatResponse Stats = new StatResponse();
    private string Error = "";

    private void Start()
    {
        if (statsText != null)
        {
            StartCoroutine(GetInitialPlayerData());
        }

    }
    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync(SceneNames.Start);
    }
    public void SurveyLink()
    {
        Application.OpenURL("www.google.com");
    }

    public void OpenScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);

    }
    public void OpenLink(string url)
    {
        Application.OpenURL(url);

    }
    public void LoginAndStart()
    {
        buttonsToHide.SetActive(false);
        StartCoroutine(LoginCall());
    }
    public void LogPlayer()
    {
        RuntimeVariables.PlayerJwtToken = Data.token;
        RuntimeVariables.PlayerId = Data.id;
        RuntimeVariables.PlayerEmail = Data.email;
        if (Data.isFirstLogin)
        {
            SceneManager.LoadScene(SceneNames.Metrics);
        }
        else
        {
            SceneManager.LoadScene(SceneNames.Stats);
        }
    }
    public void SetStats()
    {
        if (Stats.userName != "" && Stats.userName != null)
        {
            statsText.text = String.Format(
                "User name: {0} \n" +
                "Email: {1} \n" +
                "Attempts: {2} \n" +
                "Deaths: {3} \n" +
                "High score: {4} \n" +
                "",
                 Stats.userName, Stats.email, Stats.attempts, Stats.deaths, Stats.highScore);
            
        }
    }
    public IEnumerator GetInitialPlayerData()
    {
        UnityWebRequest req = UnityWebRequest.Get(RuntimeVariables.apiUrl + "/api/users/getUserData/" + RuntimeVariables.PlayerId.ToString());
        req.useHttpContinue = false;
        req.SetRequestHeader("Authorization", "Bearer " + RuntimeVariables.PlayerJwtToken);
        yield return req.SendWebRequest();


        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(req.downloadHandler.text);
            Error = req.downloadHandler.text;
            Debug.LogError(req.error);
        }
        else
        {
            Stats = JsonUtility.FromJson<StatResponse>(req.downloadHandler.text);
            RuntimeVariables.HighScore = Stats.highScore;
            RuntimeVariables.CanNowSaveGame = Stats.canNowSaveGame;
            RuntimeVariables.PlayerAttempts = Stats.attempts;
            SetStats();
        }
    }

    public void SetErrorMsg(string msg)
    {
        errorMsg.text = msg;
        errorMsg.color = errorColor;
        buttonsToHide.SetActive(true);
    }
    public IEnumerator LoginCall()
    {
        var flag = true;
        if (UserName.text.Length <= 0)
        {
            SetErrorMsg("Username cannot be empty");
            flag = false;
        }
        else if (Password.text.Length < 8)
        {
            SetErrorMsg("Password must be at least 8 characters long");
            flag = false;
        }
        else if (UserName.text.Any(x => Char.IsWhiteSpace(x)))
        {
            SetErrorMsg("Please use only digits, letters and _");
            flag = false;
        }

        if (flag)
        {
            var obj = new UserLogin
            {
                UserName = UserName.text,
                Password = getSHA256Hash(Password.text),
                Email = Email.text.Length == 0 ? "" : Email.text
            };

            UnityWebRequest req = UnityWebRequest.Post(RuntimeVariables.apiUrl + "/api/auth/credentials", JsonUtility.ToJson(obj), "application/json");
            req.useHttpContinue = false;
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(req.downloadHandler.text);
                Error = req.downloadHandler.text;
                Debug.LogError(req.error);
                SetErrorMsg(Error);
            }
            else
            {
                Data = JsonUtility.FromJson<LoginResponse>(req.downloadHandler.text);
                LogPlayer();
            }
        }
    }

    [Serializable]
    private class UserLogin
    {
        public string UserName;
        public string Password;
        public string Email;
    }
    [Serializable]
    private class LoginResponse
    {
        public int id;
        public string email;
        public string userName;
        public string token;
        public bool isFirstLogin;
        public string Message;
        public bool IsSuccess;
    }

    [Serializable]
    private class StatResponse
    {
        public string userName;
        public string email;
        public bool isControlGroup;
        public bool firstLogin;
        public bool canNowSaveGame;
        public int attempts;
        public int deaths;
        public int highScore;
    }
    private string getSHA256Hash(string text)
    {
        byte[] bytes = Encoding.Unicode.GetBytes(text);
        SHA256Managed hashstring = new SHA256Managed();
        byte[] hash = hashstring.ComputeHash(bytes);
        string hashString = string.Empty;
        foreach (byte x in hash)
        {
            hashString += String.Format("{0:x2}", x);
        }
        return hashString;
    }
}
