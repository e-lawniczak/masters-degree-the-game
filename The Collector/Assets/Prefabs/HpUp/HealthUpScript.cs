using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpScript : MonoBehaviour
{
    private SoundHandler soundHandler;
    private void Start()
    {
        soundHandler = GameObject.Find("SoundHandler").GetComponent<SoundHandler>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerLogic>() != null)
        {
            collision.gameObject.GetComponent<PlayerLogic>().AddHealth(1);
            soundHandler.CoinPickup();
            Destroy(gameObject, 0.0f);
        }
    }
}
