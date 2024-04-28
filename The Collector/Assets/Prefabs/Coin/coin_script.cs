using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin_script : MonoBehaviour
{
    [SerializeField] private int coinId;
    private SoundHandler soundHandler;
    // Start is called before the first frame update
    private void Start()
    {
        if (RuntimeVariables.collectedCoins.IndexOf(coinId) > -1)
        {
            Destroy(gameObject, 0.0f);
            return;
        }
        soundHandler = GameObject.Find("SoundHandler").GetComponent<SoundHandler>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == LayerVariables.Player && collision.GetType() == typeof(UnityEngine.BoxCollider2D))
        {
            collision.gameObject.GetComponent<PlayerLogic>().AddPoints(RuntimeVariables.CoinPoints);
            PlaytroughVariables.CoinsCollected += 1;
            RuntimeVariables.CurrentLevelCoins += 1;
            RuntimeVariables.collectedCoins.Add(coinId);
            soundHandler.CoinPickup();
            Destroy(gameObject, 0.0f);
        }
    }
    public int GetId()
    {
        return coinId;
    }
    private void Update()
    {
        if (RuntimeVariables.defeatedEnemies.IndexOf(coinId) > -1)
        {
            Destroy(gameObject, 0.0f);
            return;
        }
    }
}
