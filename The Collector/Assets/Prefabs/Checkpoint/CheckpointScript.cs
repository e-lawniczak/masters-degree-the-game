using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == LayerVariables.Checkpoint)
        {
            var gameEngine = GameObject.Find("GameEngine");
            GameEngine ge = gameEngine.GetComponent<GameEngine>();
            if (ge != null)
            {
                ge.SaveCheckpoint();
            }
        }

    }
}
