using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin_script : MonoBehaviour
{
    [SerializeField] private int coinId;
    // Start is called before the first frame update
    private void Start()
    {
        if (RuntimeVariables.collectedCoins.IndexOf(coinId) > -1)
        {
            Destroy(this, 0.0f);
            return;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == LayerVariables.Player)
        {
            collision.gameObject.GetComponent<PlayerLogic>().AddPoints(25);
            PlaytroughVariables.CoinsCollected += 1;
            RuntimeVariables.CurrentLevelCoins += 1;
            Destroy(this.gameObject);
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
            Destroy(this, 0.0f);
            return;
        }
    }
}
