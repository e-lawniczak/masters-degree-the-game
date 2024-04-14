using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundHandler : MonoBehaviour
{

    [SerializeField] private GameObject[] soundSource;

    AudioSource[] AudioSource;
    public static SoundHandler instance;
    private bool IsMuted;
    private bool FirstSwordSwing = false;


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
        IsMuted = false;
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StopAllSounds();
            PlaySound(3);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            StopAllSounds();
            PlaySound(2);
        }
    }
    void StopAllSounds()
    {
        for (int i = 0; i < AudioSource.Length; i++)
        {
            StopSound(i);
        }
    }
    void AssignButtonClick()
    {
        Button[] v = GameObject.FindObjectsByType<Button>(FindObjectsSortMode.None);
        for (int i = 0; i < v.Length; i++)
        {
            v[i].onClick.AddListener(delegate () { PlaySound(0); });
        }
    }
    void AssignVolume(float vol = -1f, bool onlyMusic = false)
    {
        var assign = vol > -1f ? vol : RuntimeVariables.MasterVolume;
        if (!onlyMusic)
        {
            for (int i = 0; i < AudioSource.Length; i++)
            {
                AudioSource[i].volume = assign;
            }
        }
        var f = 0.1f;
        AudioSource[1].volume = (float)(assign * f);
        AudioSource[2].volume = (float)(assign * f);
        AudioSource[3].volume = (float)(assign * f);
    }

    void OnSceneChange(Scene current, Scene next)
    {
        AssignVolume();
        AssignButtonClick();
        if (next.name == SceneNames.Level1)
        {
            StopSound(1);
            PlaySound(2);
        }
        if (next.name == SceneNames.EndScreen)
        {
            StopAllSounds();
            PlaySound(1);
        }
    }

    public void PlaySound(int id)
    {
        AudioSource[id].Play();
    }
    public void StopSound(int id)
    {
        AudioSource[id].Stop();
    }
    public void MuteUnmute()
    {
        Button muteButton = GameObject.FindGameObjectWithTag("MuteButton").GetComponent<Button>();
        if (IsMuted)
        {
            AssignVolume(RuntimeVariables.MasterVolume, true);
            muteButton.GetComponentInChildren<TextMeshProUGUI>().text = "Mute music";
        }
        else
        {
            AssignVolume(0.0f, true);
            muteButton.GetComponentInChildren<TextMeshProUGUI>().text = "Unmute music";
        }
        IsMuted = !IsMuted;
    }
    public void SwingSword()
    {
        int soundtoplay = FirstSwordSwing ? 4 : 5;
        PlaySound(soundtoplay);
        FirstSwordSwing = !FirstSwordSwing;
    }
    public void EnemyHit()
    {
        PlaySound(6);
    }
    public void PlayerHit()
    {
        PlaySound(7);
    }
    public void SpikeHit()
    {
        PlaySound(8);
    }
    public void GrassStep()
    {
        PlaySound(9);
    }
    public void CoinPickup()
    {
        PlaySound(10);
    }
    public void Dash()
    {
        PlaySound(11);
    }
}
