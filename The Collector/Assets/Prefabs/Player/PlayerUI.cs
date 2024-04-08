using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static HelperFunctions;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image[] hps;
    [SerializeField] private Image dashImg;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameEngine engine;
    private PlayerLogic playerLogic;
    private DashInfo dashInfo;

    // Start is called before the first frame update
    void Start()
    {
        playerLogic = GetComponentInParent<PlayerLogic>();

        for (int i = 0; i < hps.Length; i++)
        {
            hps[i].transform.position = new Vector3(hps[i].transform.position.x + 65, hps[i].transform.position.y, hps[i].transform.position.z);
        }
        dashImg.enabled = true;
        dashInfo = playerLogic.GetCanDash();
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
        pointsText.text = string.Format("Points: {0}", PlaytroughVariables.TotalPoints);
        timeText.text = string.Format("{0}", PlaytroughVariables.TotalTime.GetValueOrDefault(0f).ToString("F3").Replace(',', '.'));
    }
    private void FixedUpdate()
    {
        dashInfo = playerLogic.GetCanDash();
        float mult = dashInfo.canDash ? 1 : (dashInfo.currentCd / dashInfo.dashCd);
        dashImg.rectTransform.sizeDelta = new Vector2(700f * mult, 100);
    }

}
