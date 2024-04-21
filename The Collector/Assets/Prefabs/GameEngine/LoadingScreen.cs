using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject pauseScreen;
    private GameEngine gameEngine;
    private bool IsLoading;
    void Start()
    {
     
        loadingScreen.SetActive(false);
        gameEngine = gameObject.GetComponent<GameEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (RuntimeVariables.isLoading)
        {
            pauseScreen.SetActive(false);
            loadingScreen.SetActive(true);
            Time.timeScale = 1f;
        }
        else
        {
            loadingScreen.SetActive(false) ;
        }
    }
}
