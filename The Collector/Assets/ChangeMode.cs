using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ChangeMode : MonoBehaviour
{

    [SerializeField] private GameObject changeModeScreen;
    private GameEngine gameEngine;
    // Start is called before the first frame update
    void Start()
    {
        gameEngine = GetComponent<GameEngine>();
        if (RuntimeVariables.PlayerAttempts > 9 && RuntimeVariables.CurrentLevel == 1 && !RuntimeVariables.CanNowSaveGame && !RuntimeVariables.IsControlGroup)
        {
            changeModeScreen.SetActive(true);
        }
    }

    public void CancelChange()
    {
        changeModeScreen.SetActive(false);
    }
    public void ConfirmChange()
    {
        StartCoroutine(ChangeModeApi());
    }
    IEnumerator ChangeModeApi()
    {
        UnityWebRequest req = UnityWebRequest.Post(RuntimeVariables.apiUrl + "/api/users/changeMode", JsonUtility.ToJson(new UserChangeMode { userId = RuntimeVariables.PlayerId }), "application/json");
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
            gameEngine.EndRunOnModeChange();
            changeModeScreen.SetActive(false);

        }
    }
    [Serializable]
    public class UserChangeMode
    {
        public int userId;
    }
}
