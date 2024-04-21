using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBottomTrigger : MonoBehaviour
{
    [SerializeField] private MovingPlatformEnchanced parentPlatform;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != LayerVariables.Coin)
        {
            parentPlatform.ChangeDirection();
        }
    }
}
