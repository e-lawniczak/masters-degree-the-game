using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipEnemy : MonoBehaviour
{
    [SerializeField] private BasicEnemy go;
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(LayerVariables.Environment) && gameObject.name == "WallDetector")
        {
            go.Flip();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(LayerVariables.Environment) && gameObject.name == "GroundDetector")
        {
            go.Flip();
        }
    }

}
