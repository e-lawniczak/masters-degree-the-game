using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin_script : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == LayerVariables.Player)
        {
            Debug.Log("Points up!");
            Destroy(this.gameObject);
        }
    }
}
