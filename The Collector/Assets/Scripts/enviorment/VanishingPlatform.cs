using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject parentObject;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == LayerVariables.Player)
        {
            parentObject.GetComponent<VanishingBlock>().platformTouched = true;
        }
    }
}
