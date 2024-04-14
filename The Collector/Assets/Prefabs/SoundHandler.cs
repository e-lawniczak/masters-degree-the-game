using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundHandler : MonoBehaviour
{

    [SerializeField] private GameObject[] soundSource;

    AudioSource[] AudioSource;
    public static SoundHandler instance;


    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChange;
        List<AudioSource> t = new List<AudioSource>();
        for (int i = 0; i < soundSource.Length; i++)
        {
            foreach (var item in soundSource[i].GetComponents<AudioSource>())
            {
                t.Add(item);
            };
        }
        AudioSource = t.ToArray();
        AssignVolume();
        AssignButtonClick();
        PlaySound(1);
    }
    void AssignButtonClick()
    {
        Button[] v = GameObject.FindObjectsByType<Button>(FindObjectsSortMode.None);
        for (int i = 0; i < v.Length; i++)
        {
            v[i].onClick.AddListener(delegate () { PlaySound(0); });
        }
    }
    void AssignVolume()
    {
        for (int i = 0; i < AudioSource.Length; i++)
        {
            AudioSource[i].volume = RuntimeVariables.MasterVolume;
        }
        AudioSource[1].volume = (float)(RuntimeVariables.MasterVolume / 2f);
    }

    void OnSceneChange(Scene current, Scene next)
    {
        AssignVolume();
        AssignButtonClick();
    }

    private void Update()
    {

    }
    public void PlaySound(int id)
    {
        AudioSource[id].Play();
    }
}
