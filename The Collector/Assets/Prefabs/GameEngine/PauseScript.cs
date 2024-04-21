using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private bool IsPaused;

    // Start is called before the first frame update
    void Start()
    {
        IsPaused = false;
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsPaused && !RuntimeVariables.isLoading)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu?.SetActive(true);
        Time.timeScale = 0.0f;
        IsPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu?.SetActive(false);
        Time.timeScale = 1.0f;
        IsPaused = false;
    }
}
