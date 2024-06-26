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
            int v = -1;
            bool x = Int32.TryParse(collision.name, out v);
            if (x && RuntimeVariables.CheckpointsVisited.IndexOf(v) == -1)
                RuntimeVariables.CheckpointsVisited.Add(v);
            var gameEngine = GameObject.Find("GameEngine");
            GameEngine ge = gameEngine.GetComponent<GameEngine>();
            if (ge != null && (RuntimeVariables.IsControlGroup || RuntimeVariables.CanNowSaveGame) && ge.GetPlayerCurrentHp() > 0)
            {
                ge.SaveCheckpoint(transform.position);
            }

        }
        if (collision.gameObject.tag == LayerVariables.FinishLine)
        {
            int v = -1;
            bool x = Int32.TryParse(collision.name, out v);
            if (x && RuntimeVariables.CheckpointsVisited.IndexOf(v) == -1)
                RuntimeVariables.CheckpointsVisited.Add(v);
            var gameEngine = GameObject.Find("GameEngine");
            GameEngine ge = gameEngine.GetComponent<GameEngine>();
            if (ge != null && ge.levelFinished == false)
            {
                ge.LevelComplete();
                Destroy(collision.gameObject, 0.0f);
            }
        }

    }
}
