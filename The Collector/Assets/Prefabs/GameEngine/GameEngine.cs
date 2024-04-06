using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class GameEngine : MonoBehaviour
{
    [SerializeField] private PlayerLogic playerLogic;
    [SerializeField] private PlayerController playerController;
    private int totalPoints;
    private float totalTime = 0f;
    private float globalUpdateInterval = 10f;
    private float counter = 0f;

    private string Data;

    void Start()
    {
        FetchData();
    }

    void Update()
    {

        if (playerLogic == null) return;
        if (playerController == null) return;
        totalTime += Time.deltaTime;
        if (playerLogic.GetPoints() > totalPoints)
        {
            totalPoints = playerLogic.GetPoints();
        }
        else if (playerLogic.GetPoints() == 0)
        {
            playerLogic.AddPoints(totalPoints);
        }
        UpdateData();
    }
    private IEnumerator UpdateData()
    {
        if (counter >= globalUpdateInterval)
        {
            using (UnityWebRequest req = new UnityWebRequest("", "POST"))
            {
                yield return req.SendWebRequest();

                switch (req.result)
                {

                    case UnityWebRequest.Result.ConnectionError:
                        break;
                    case UnityWebRequest.Result.Success:

                        break;
                }
            }
            counter = 0;
        }
        else
        {
            counter += Time.deltaTime;
        }

    }
    private IEnumerator FetchData()
    {
        using (UnityWebRequest req = new UnityWebRequest())
        {
            yield return req.SendWebRequest();

            switch (req.result)
            {

                case UnityWebRequest.Result.ConnectionError:
                    break;
                case UnityWebRequest.Result.Success:
                    Data = JsonUtility.FromJson<string>(req.downloadHandler.text);
                    break;
            }
        }
    }
    public int GetPoints()
    {
        return totalPoints;
    }
    public float GetTime()
    {
        return totalTime;
    }
}
