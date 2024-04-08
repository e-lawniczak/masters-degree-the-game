using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI volumeText;
    [SerializeField] Slider volume;

    // Start is called before the first frame update
    void Start()
    {
        volumeText.text = string.Format("{0}/100", (int)(RuntimeVariables.MasterVolume * 100));
        volume.value = RuntimeVariables.MasterVolume;
    }

    // Update is called once per frame
    void Update()
    {
        volumeText.text = string.Format("{0}/100", (int)(RuntimeVariables.MasterVolume * 100));
    }

    public void OnVolumeChange()
    {
        RuntimeVariables.MasterVolume = volume.value;
    }
}
