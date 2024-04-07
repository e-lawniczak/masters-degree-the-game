using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MenuHandlerScript : MonoBehaviour
{
    private string errorColor = "FF7979";
    private string defaultColor = "FFFFFF";
    public TextMeshProUGUI errorMsg;
    public Text[] inputs;
    public Toggle[] agreements;
    public LoginResponse Data;
    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync("Start");
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
    public async IEnumerator LoginAndStart()
    {
        using (UnityWebRequest req = new UnityWebRequest())
        {
            yield return req.SendWebRequest();

            switch (req.result)
            {

                case UnityWebRequest.Result.ConnectionError:
                    break;
                case UnityWebRequest.Result.Success:
                    Data = JsonUtility.FromJson<LoginResponse>(req.downloadHandler.text);
                    break;
            }
        }
    }


    private class LoginResponse
    {
        public int id { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
        public string token { get; set; }
        public bool isFirstLogin { get; set; }
    }
}
