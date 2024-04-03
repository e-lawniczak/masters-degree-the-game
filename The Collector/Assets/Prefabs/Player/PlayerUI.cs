using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image[] hps;
    private PlayerLogic playerLogic;

    // Start is called before the first frame update
    void Start()
    {
        playerLogic = GetComponentInParent<PlayerLogic>();
        for (int i = 0; i < hps.Length; i++)
        {
            hps[i].transform.position = new Vector3(hps[i].transform.position.x + 60, hps[i].transform.position.y, hps[i].transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hps.Length; i++)
        {
            hps[i].enabled = false;
        }
        for (int i = 0; i < playerLogic.GetCurrentHp(); i++)
        {
            hps[i].enabled = true;
        }
    }
}
