using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWIthPlatform : MonoBehaviour
{

    private Rigidbody2D rb;
    private Rigidbody2D playerRb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        collision.collider.transform.SetParent(transform);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        playerRb = null;
        collision.collider.transform.SetParent(null);
    }
    private void Update()
    {
      
    }
    private void FixedUpdate()
    {
        if (playerRb != null)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x + rb.velocity.x, playerRb.velocity.y);
        }
            
    }
}
