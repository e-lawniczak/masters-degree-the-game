using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject parentObject;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == LayerVariables.Player)
        {
            parentObject.GetComponent<VanishingBlock>().platformTouched = true;
        }
    }
}
